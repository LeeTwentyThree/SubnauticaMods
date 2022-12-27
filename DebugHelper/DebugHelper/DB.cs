using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using QModManager.API;
using UnityEngine;

/* Global namespace and short name to make it as accessible as possible
 * Class to help with the REPL console */
public static class DB
{
    public static Harmony harmony;
    public static MethodInfo returnFalse;
    private static MethodInfo echo;
    private static MethodInfo echoWithArgs;
    private static MethodInfo echoWithReturn;
    
    internal static void Setup()
    {
        harmony = DebugHelper.Main.harmony;
        returnFalse = AccessTools.Method(typeof(DB), nameof(False));
        echo = AccessTools.Method(typeof(DB), nameof(Echo));
        echoWithArgs = AccessTools.Method(typeof(DB), nameof(EchoArgs));
        echoWithReturn = AccessTools.Method(typeof(DB), nameof(EchoReturn));

        var allMods = QModServices.Main.GetAllMods();
        foreach (var mod in allMods)
        {
            if (mod.IsLoaded)
            {
                knownAssemblyNames.Add(mod.Id);
            }
        }
    }

    private static string GetMethodNameText(MethodBase method)
    {
        var methodName = $"{method.DeclaringType.FullName}.{method.Name}";
        return ColorCode.FormatMethodName(methodName);
    }

    private static void Echo(MethodBase __originalMethod)
    {
        var message = $"<color={ColorCode.title}>Running method</color> {GetMethodNameText(__originalMethod)}";
        if (__originalMethod is MethodInfo method)
        {
            message += $" (returns {ColorCode.FormatType(method.ReturnType.ToString())})";
        }
        if (__originalMethod.IsVirtual)
        {
            message += " (virtual)";
        }
        ErrorMessage.AddMessage(message);
    }

    private static void EchoArgs(object[] __args, MethodBase __originalMethod)
    {
        var message = $"{GetMethodNameText(__originalMethod)} <color={ColorCode.title}>arguments:</color>\n";
        var argNames = __originalMethod.GetParameters();
        if (__args != null)
        {
            for (int i = 0; i < __args.Length; i++)
            {
                var valueString = ColorCode.FormatValue(__args[i]);

                message += $"{ColorCode.FormatArgName(argNames[i].ToString())}: {valueString}";

                if (i < __args.Length - 1)
                {
                    message += ", \n";
                }
            }
        }
        ErrorMessage.AddMessage(message);
    }

    private static void EchoReturn(object __result, MethodBase __originalMethod)
    {
        ErrorMessage.AddMessage($"{GetMethodNameText(__originalMethod)} <color={ColorCode.title}>returned:</color> {ColorCode.FormatValue(__result)}");
    }

    private static bool False()
    {
        return false;
    }

    #region ListenBasic
    public static string ListenBasic(MethodInfo original, bool prefix = false) // whenever the method is run, shows information about it on screen
    {
        if(original == null)
        {
            return "Could not find method to listen for";
        }

        if (prefix) harmony.Patch(original, new HarmonyMethod(echo));
        else harmony.Patch(original, null, new HarmonyMethod(echo));
        return "Patched";
    }

    public static string ListenBasic(string location, bool prefix = false)
    {
        return ListenBasic(Method(location), prefix);
    }

    public static string ListenBasic(string typeName, string methodName, bool prefix = false)
    {
        return ListenBasic(Method(typeName, methodName), prefix);
    }

    public static string ListenBasic(System.Type type, string methodName, bool prefix = false)
    {
        return ListenBasic(Method(type, methodName), prefix);
    }
    #endregion

    #region Listen args
    public static string ListenArgs(MethodInfo original, bool prefix = false) // whenever the method is run, shows information about it on screen
    {
        if(original.GetParameters().Length <= 0)
        {
            return "Method does not have arguments";
        }

        if (prefix) harmony.Patch(original, new HarmonyMethod(echoWithArgs));
        else harmony.Patch(original, null, new HarmonyMethod(echoWithArgs));
        return "Patch applied";
    }

    public static string ListenArgs(string location, bool prefix = false)
    {
        return ListenArgs(Method(location), prefix);
    }

    public static string ListenArgs(string typeName, string methodName, bool prefix = false)
    {
        return ListenArgs(Method(typeName, methodName), prefix);
    }

    public static string ListenArgs(System.Type type, string methodName, bool prefix = false)
    {
        return ListenArgs(Method(type, methodName), prefix);
    }
    #endregion
    
    #region Listen return
    public static string ListenReturn(MethodInfo original, bool prefix = false) // whenever the method is run, shows information about it on screen
    {
        if (original.ReturnType == typeof(void))
        {
            return "Method does not have return type";
        }

        if (prefix) harmony.Patch(original, new HarmonyMethod(echoWithReturn));
        else harmony.Patch(original, null, new HarmonyMethod(echoWithReturn));
        return "Patch applied";
    }

    public static string ListenReturn(string location, bool prefix = false)
    {
        return ListenReturn(Method(location), prefix);
    }

    public static string ListenReturn(string typeName, string methodName, bool prefix = false)
    {
        return ListenReturn(Method(typeName, methodName), prefix);
    }

    public static string ListenReturn(System.Type type, string methodName, bool prefix = false)
    {
        return ListenReturn(Method(type, methodName), prefix);
    }
    #endregion

    #region Listen (all)
    public static string Listen(MethodInfo original, bool prefix = false) // whenever the method is run, shows information about it on screen
    {
        if (original == null)
        {
            return $"<color={ColorCode.error}>Could not find method to listen for!</color>";
        }

        string message = $"Patched method '{original.Name}', listening for";

        bool shouldFixGrammar = true;

        //patch the main listen
        if (prefix) harmony.Patch(original, new HarmonyMethod(echo));
        else harmony.Patch(original, null, new HarmonyMethod(echo));

        bool includeAnd = False();

        //patch the listen with return type
        if (original.ReturnType != typeof(void))
        {
            if (prefix) harmony.Patch(original, new HarmonyMethod(echoWithReturn));
            else harmony.Patch(original, null, new HarmonyMethod(echoWithReturn));
            message += " return values";
            includeAnd = true;
            shouldFixGrammar = false;
        }

        //patch the listen with arguments
        if(original.GetParameters().Length > 0)
        {
            if (prefix) harmony.Patch(original, new HarmonyMethod(echoWithArgs));
            else harmony.Patch(original, null, new HarmonyMethod(echoWithArgs));
            if (includeAnd) message += " and";
            message += " arguments";
            shouldFixGrammar = false;
        }

        if (shouldFixGrammar) message += " method calls";

        return $"<color={ColorCode.success}>{message}.</color>";
    }

    public static string Listen(string location, bool prefix = false)
    {
        return Listen(Method(location), prefix);
    }

    public static string Listen(string typeName, string methodName, bool prefix = false)
    {
        return Listen(Method(typeName, methodName), prefix);
    }

    public static string Listen(System.Type type, string methodName, bool prefix = false)
    {
        return Listen(Method(type, methodName), prefix);
    }
    #endregion

    #region Mute
    public static void Mute(MethodInfo original) // forces this method to never run
    {
        harmony.Patch(original, new HarmonyMethod(returnFalse));
    }

    public static void Mute(string location)
    {
        harmony.Patch(Method(location), new HarmonyMethod(returnFalse));
    }

    public static void Mute(string typeName, string methodName)
    {
        harmony.Patch(Method(typeName, methodName), new HarmonyMethod(returnFalse));
    }

    public static void Mute(System.Type type, string methodName)
    {
        harmony.Patch(Method(type, methodName), new HarmonyMethod(returnFalse));
    }
    #endregion

    public static MethodInfo Method(string location) // fastest way to reference a method ("Creature.Start")
    {
        var split = location.Split('.');
        var typeName = split[split.Length - 2];
        var methodName = split[split.Length - 1];
        return Method(typeName, methodName);
    }

    public static MethodInfo Method(string typeName, string methodName) // fast way to reference a method ("Creature", "Start")
    {
        return Method(TypeByName(typeName), methodName);
    }

    public static MethodInfo Method(System.Type type, string methodName) // fast-ish way to reference a method (typeof(Creature), "Start")
    {
        return AccessTools.Method(type, methodName);
    }

    public static System.Type TypeByName(string typeName)
    {
        foreach (var known in knownAssemblyNames)
        {
            var type = System.Type.GetType($"{typeName},{known}");
            if (type != null)
            {
                return type;
            }
        }
        return null;
    }

    private static List<string> knownAssemblyNames = new List<string>() { "Assembly-CSharp", "Assembly-CSharp-firstpass", "UnityEngine", "UnityEngine.CoreModule", "UnityEngine.PhysicsModule", "SMLHelper" };

    public static string Help
    {
        get
        {
            return $"<color={ColorCode.replTitle}>Useful methods:</color>\n" +
                $"<color={ColorCode.codeSegment}>" +
                "- Listen(MethodInfo original, bool prefix = false): Outputs the returned value and all parameters passed into the method when it is called.\n" +
                "- Mute(MethodInfo original): Stops a method from being called.\n" +
                "- Method(string location): Returns a MethodInfo by its name (ex: \"Peeper.Start\")\n" +
                "- Method(System.Type type, string methodName): Also returns a MethodInfo (ex: typeof(Peeper), \"Start\")\n" +
                "</color>" +

                $"<color={ColorCode.replTitle}>All methods:</color>\n" +
                $"<color={ColorCode.codeSegment}>" +
                "Listen(MethodInfo method, bool prefix = false)\n" +
                "Listen(string location, bool prefix = false)\n" +
                "Listen(string typeName, string methodName, bool prefix = false)\n" +
                "Listen(Type type, string methodName, bool prefix = false)\n" +
                "Mute(MethodInfo original)\n" +
                "Mute(string location)\n" +
                "Mute(string typeName, string methodName)\n" +
                "Mute(Type type, string methodName)\n" +
                "</color>" +
                $"<color={ColorCode.replTitle}>More info here:</color> <color={ColorCode.url}>https://github.com/LeeTwentyThree/Lee23-SubnauticaMods/blob/main/Downloads/DownloadPages/DebugHelper.md</color>";
            }
    }

    private static class ColorCode
    {
        public static string error = "#fcf25b";
        public static string success = "#a8ff9e";
        public static string title = "#ff00b3";
        public static string replTitle = "#00ffbb";
        public static string methodName = "#fcf25b";
        public static string type = "#00ffc3";
        public static string arg = "#ff5900";
        public static string namespacePath = "#ffffff";
        public static string white = "#ffffff";
        public static string keywords = "#569cd6";
        public static string url = "#569cd6";
        public static string codeSegment = "#cffaff";

        public static string FormatType(string path)
        {
            string failed = $"<color={error}>Unknown</color>";
            if (string.IsNullOrEmpty(path)) return failed;
            var split = path.Split('.');
            if (split.Length == 0) return failed;

            string typeNamespace = "";
            for (int i = 0; i < split.Length - 1; i++)
            {
                typeNamespace += split[i] + ".";
            }

            return $"<color={namespacePath}>{typeNamespace}</color><color={type}>{split[split.Length - 1]}</color>";
        }

        public static string FormatMethodName(string path)
        {
            string failed = $"<color={error}>Unknown</color>";
            if (string.IsNullOrEmpty(path)) return failed;
            var split = path.Split('.');
            if (split.Length < 2 || string.IsNullOrEmpty(split[split.Length - 1])) return failed;

            string methodNamespace = "";
            for (int i = 0; i < split.Length - 2; i++)
            {
                methodNamespace += split[i] + ".";
            }

            return $"<color={namespacePath}>{methodNamespace}</color><color={type}>{split[split.Length - 2]}</color>.<color={methodName}>{split[split.Length - 1]}</color>";
        }

        public static string FormatValue(object value)
        {
            if (value == null)
            {
                return $"<color={keywords}>null</color>";
            }
            return value.ToString();
        }

        public static string FormatArgName(string arg)
        {
            var split = arg.Split(' ');
            return $"<color={type}>{split[0]}</color> <color={white}>{split[1]}</color>";
        }
    }
}
