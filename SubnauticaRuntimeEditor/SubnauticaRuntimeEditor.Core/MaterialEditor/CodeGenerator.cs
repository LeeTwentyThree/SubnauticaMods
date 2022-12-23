using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace SubnauticaRuntimeEditor.Core.MaterialEditor
{
    public static class CodeGenerator
    {
        private static readonly string FOLDER_NAME = "MaterialEditor";
        private static readonly string FOLDER_DIRECTORY = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), FOLDER_NAME);
        private static readonly string FILE_NAME_PREFIX = "Material-";
        private static readonly string FILE_NAME_POSTFIX = ".txt";

        public static void GenerateShaderCode(Material material)
        {
            Directory.CreateDirectory(FOLDER_DIRECTORY);
            string filename = GenerateFileName();

            using (var writer = new StreamWriter(Path.Combine(FOLDER_DIRECTORY, filename), false))
            {
                WriteHeader(material, writer);

                if (material.shader.name.Equals(MaterialEditorViewer.MARMOSETUBER_SHADER_NAME))
                {
                    writer.WriteLine("");
                    WriteKeywords(material, writer);
                }

                writer.WriteLine("");
                WriteProperties(material, writer);
            }
        }

        private static string GenerateFileName()
        {
            return $"{FILE_NAME_PREFIX}{DateTime.Now:yyyy-MM-dd-H-mm-ss-ff}{FILE_NAME_POSTFIX}";
        }

        private static void WriteHeader(Material material, TextWriter writer)
        {
            writer.WriteLine($"Code generated from Material: {material.name}");
            writer.WriteLine("");
            writer.WriteLine("using UnityEngine;");
            writer.WriteLine("");
            writer.WriteLine("Material material;");
            writer.WriteLine($"material.shader = Shader.Find(\"{material.shader.name}\");");
        }

        private static void WriteKeywords(Material material, TextWriter writer)
        {
            var shaderKeywords = new HashSet<string>(material.shaderKeywords);
            foreach (Keywords keyword in Enum.GetValues(typeof(Keywords)))
            {
                if (shaderKeywords.Contains(keyword.ToString()))
                    writer.WriteLine($"material.EnableKeyword(\"{keyword.ToString()}\");");
                else
                    writer.WriteLine($"material.DisableKeyword(\"{keyword.ToString()}\");");
            }
        }

        private static void WriteProperties(Material material, TextWriter writer)
        {
            foreach (var property in MaterialEditorPropertyTypes.TYPES.Values)
                writer.WriteLine($"material.{property.GenerateSetPropertyCode(material)};");
        }
    }
}
