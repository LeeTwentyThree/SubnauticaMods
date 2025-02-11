using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Nautilus.Extensions;
using UnityEngine;

namespace GenerateBasePrefabCommand;

public static class GenerateBasePrefabCode
{
    private const string PathPrefix = "Assets/Prefabs/Base/GeneratorPieces/";
    
    public static void ConvertBaseToPrefabConstructionCode(Base @base)
    {
        var cells = @base.cellObjects;

        var spawnablePieces = new Dictionary<string, List<SpawnablePiece>>();
        foreach (var cell in cells)
        {
            if (cell == null) continue;
            foreach (Transform childObject in cell)
            {
                var name = childObject.name.TrimClone();
                if (!spawnablePieces.TryGetValue(name, out var piecesList))
                {
                    piecesList = new List<SpawnablePiece>();
                    spawnablePieces.Add(name, piecesList);
                }
                piecesList.Add(new SpawnablePiece(name, childObject.transform.localPosition + cell.localPosition, childObject.transform.localRotation));

            }
        }

        var sb = new StringBuilder();

        sb.AppendLine($"// Generated with {PluginInfo.PLUGIN_GUID} ({PluginInfo.PLUGIN_VERSION})");
        sb.AppendLine("// Generated code is intended to be used in an IEnumerator/Coroutine for asynchronous prefab creation");
        sb.AppendLine("// See https://subnauticamodding.github.io/Nautilus/api/Nautilus.Assets.CustomPrefab.html#Nautilus_Assets_CustomPrefab_SetGameObject_System_Func_IOut_UnityEngine_GameObject__System_Collections_IEnumerator__");
        sb.Append("\n\n\n");

        int requestIndex = 1;
        foreach (var entry in spawnablePieces)
        {
            var requestVariable = $"request{requestIndex}";
            sb.AppendLine($"var {requestVariable} = PrefabDatabase.GetPrefabForFilenameAsync(\"{PathPrefix}{entry.Key}.prefab\");");
            sb.AppendLine($"yield return {requestVariable};");
            sb.AppendLine($"if (!{requestVariable}.TryGetPrefab(out var {GetPrefabVariableName(entry.Key)}))");
            sb.AppendLine("{");
            sb.AppendLine($"    Plugin.Logger.LogError(\"Failed to load prefab with name {entry.Key}!\");");
            sb.AppendLine("    yield break;");
            sb.AppendLine("}");
            requestIndex++;
        }

        int objIndex = 1;
        foreach (var entry in spawnablePieces)
        {
            foreach (var piece in entry.Value)
            {
                var variableName = $"child{objIndex}";
                sb.AppendLine($"var {variableName} = Object.Instantiate({GetPrefabVariableName(entry.Key)}, obj.transform);");
                sb.AppendLine($"{variableName}.transform.localPosition = {FormatVector3(piece.LocalPosition)};");
                sb.AppendLine($"{variableName}.transform.localRotation = {FormatQuaternion(piece.LocalRotation)};");
                sb.AppendLine($"{variableName}.SetActive(true);");
                sb.AppendLine($"StripComponents({variableName});");
                objIndex++;
            }
        }

        var text = sb.ToString();
        var dateString = System.DateTime.Now.ToLongDateString();
        var modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var outputFolder = Path.Combine(modFolder, "Generated");
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }
        var path = Path.Combine(outputFolder,
            $"BaseGenerationCode-{dateString}.txt");
        File.WriteAllText(path, text);
        ErrorMessage.AddMessage($"Successfully created base generation code at path '{path}'.");
    }

    private static string FormatVector3(Vector3 vector3)
    {
        return $"new Vector3({vector3.x}f, {vector3.y}f, {vector3.z}f)";
    }
    
    private static string FormatQuaternion(Quaternion quaternion)
    {
        return $"new Quaternion({quaternion.x}f, {quaternion.y}f, {quaternion.z}f, {quaternion.w}f)";
    }
    
    private static string GetPrefabVariableName(string prefabName)
    {
        if (prefabName.Length <= 1) return prefabName;
        return char.ToLower(prefabName[0]) + prefabName.Substring(1);
    }
    
    private readonly struct SpawnablePiece
    {
        public string Name { get; }
        public string AddressablePath { get; }
        public Vector3 LocalPosition { get; }
        public Quaternion LocalRotation { get; }

        public SpawnablePiece(string name, Vector3 localPosition, Quaternion localRotation)
        {
            Name = name;
            AddressablePath = PathPrefix + name + ".prefab";
            LocalPosition = localPosition;
            LocalRotation = localRotation;
        }
    }
}