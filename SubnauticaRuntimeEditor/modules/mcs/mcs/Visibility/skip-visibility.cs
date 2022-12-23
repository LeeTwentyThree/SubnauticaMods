using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using MonoMod.RuntimeDetour;

namespace Mono.CSharp
{
    internal static class SkipVisibilityExt
    {
        static readonly bool _IsMono = Type.GetType("Mono.Runtime") != null;
        static readonly bool _MonoAssemblyNameHasArch = new AssemblyName("Dummy, ProcessorArchitecture=MSIL").ProcessorArchitecture == ProcessorArchitecture.MSIL;

        static readonly FieldInfo dynAssField =
            typeof(AssemblyBuilder).GetField("dynamic_assembly", BindingFlags.Instance | BindingFlags.NonPublic);

        static List<Assembly> generatedAssemblies = new List<Assembly>();

        static SkipVisibilityExt()
        {
            new Hook(typeof(AppDomain).GetMethod(nameof(AppDomain.GetAssemblies)), typeof(SkipVisibilityExt).GetMethod(nameof(GetAssembliesPatch))).Apply();
        }

        public static Assembly[] GetAssembliesPatch(Func<AppDomain, Assembly[]> orig, AppDomain self)
        {
            var result = new List<Assembly>(orig(self));
            result.AddRange(generatedAssemblies);
            return result.ToArray();
        }
        
        public static unsafe void MarkCorlibInternal(this AssemblyBuilder asm)
        {
            if (!_IsMono || dynAssField == null)
                return;

            var asmPtr = (UIntPtr) dynAssField.GetValue(asm);

            if (asmPtr == UIntPtr.Zero)
                return;
            
            var offs = 
                // ref_count (4 + padding)
                IntPtr.Size +
                // basedir
                IntPtr.Size +

                // aname
                // name
                IntPtr.Size +
                // culture
                IntPtr.Size +
                // hash_value
                IntPtr.Size +
                // public_key
                IntPtr.Size +
                // public_key_token (17 + padding)
                20 +
                // hash_alg
                4 +
                // hash_len
                4 +
                // flags
                4 +

                // major, minor, build, revision[, arch] (10 framework / 20 core + padding)
                (
                    !_MonoAssemblyNameHasArch ? (
                        typeof(object).Assembly.GetName().Name == "System.Private.CoreLib" ? 
                            16 : 8
                    ) : (
                        typeof(object).Assembly.GetName().Name == "System.Private.CoreLib" ? 
                            (IntPtr.Size == 4 ? 20 : 24) :
                            (IntPtr.Size == 4 ? 12 : 16)
                    )
                ) +

                // image
                IntPtr.Size +
                // friend_assembly_names
                IntPtr.Size +
                // friend_assembly_names_inited
                1 +
                // in_gac
                1 +
                // dynamic
                1; ;

            var corlibInternalPtr = (byte*) ((long) asmPtr + offs);
            *corlibInternalPtr = 1;

            generatedAssemblies.Add(asm);
        }
    }
}