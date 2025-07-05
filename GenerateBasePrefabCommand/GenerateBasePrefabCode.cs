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

    public static void ConvertBaseToPrefabConstructionCode(Base @base, BaseGenerationSettings settings)
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

                var spawnable = new SpawnablePiece(name, childObject.transform.localPosition + cell.localPosition,
                    childObject.transform.localRotation);

                if (settings.IncludeSupports)
                {
                    var baseFoundationPiece = childObject.gameObject.GetComponentInChildren<BaseFoundationPiece>();
                    if (baseFoundationPiece != null)
                    {
                        var lengths = new float[baseFoundationPiece.pillars.Length];
                        for (var i = 0; i < lengths.Length; i++)
                        {
                            lengths[i] = baseFoundationPiece.pillars[i].adjustable.localScale.z;
                        }

                        var pathToLegs = Utility.GetPathToChild(baseFoundationPiece.transform, childObject);
                        spawnable.FoundationData = new FoundationData(lengths, pathToLegs,
                            baseFoundationPiece.pillars[0].bottom != null);
                    }
                }

                piecesList.Add(spawnable);
            }
        }

        var sb = new StringBuilder();

        sb.AppendLine($"// Generated with {PluginInfo.PLUGIN_GUID} ({PluginInfo.PLUGIN_VERSION})");
        sb.AppendLine(
            "// Generated code is intended to be used in an IEnumerator/Coroutine for asynchronous prefab creation");
        sb.AppendLine(
            "// See https://subnauticamodding.github.io/Nautilus/api/Nautilus.Assets.CustomPrefab.html#Nautilus_Assets_CustomPrefab_SetGameObject_System_Func_IOut_UnityEngine_GameObject__System_Collections_IEnumerator__");
        if (settings.IncludeSupports)
        {
            sb.AppendLine(
                "// Base supports are enabled; please make sure you strip the BaseFoundationPiece component for these to work properly!");
        }

        sb.Append("\n\n\n");

        sb.AppendLine("// Prepare prefabs");
        int requestIndex = 1;
        foreach (var entry in spawnablePieces)
        {
            var requestVariable = $"request{requestIndex}";
            sb.AppendLine(
                $"var {requestVariable} = PrefabDatabase.GetPrefabForFilenameAsync(\"{PathPrefix}{entry.Key}.prefab\");");
            sb.AppendLine($"yield return {requestVariable};");
            sb.AppendLine($"if (!{requestVariable}.TryGetPrefab(out var {GetPrefabVariableName(entry.Key)}))");
            sb.AppendLine("{");
            sb.AppendLine($"    Plugin.Logger.LogError(\"Failed to load prefab with name {entry.Key}!\");");
            sb.AppendLine("    yield break;");
            sb.AppendLine("}");
            requestIndex++;
        }

        sb.AppendLine("\n// Instantiate prefabs");
        int objIndex = 1;
        int legsIndex = 1;
        foreach (var entry in spawnablePieces)
        {
            foreach (var piece in entry.Value)
            {
                var variableName = $"child{objIndex}";
                sb.AppendLine(
                    $"var {variableName} = Object.Instantiate({GetPrefabVariableName(entry.Key)}, obj.transform);");
                sb.AppendLine($"{variableName}.transform.localPosition = {FormatVector3(piece.LocalPosition)};");
                sb.AppendLine($"{variableName}.transform.localRotation = {FormatQuaternion(piece.LocalRotation)};");
                sb.AppendLine($"{variableName}.SetActive(true);");
                if (settings.IncludeSupports)
                {
                    if (piece.FoundationData.HasLegs)
                    {
                        var pillarsVariable = $"legs{legsIndex}";
                        var legsCount = piece.FoundationData.LegLengths.Length;
                        if (string.IsNullOrEmpty(piece.FoundationData.PathToLegs))
                        {
                            sb.AppendLine(
                                $"var {pillarsVariable} = {variableName}.GetComponent<BaseFoundationPiece>().pillars;");
                        }
                        else
                        {
                            sb.AppendLine(
                                $"var {pillarsVariable} = {variableName}.transform.Find(\"{piece.FoundationData.PathToLegs}\").GetComponent<BaseFoundationPiece>().pillars;");
                        }

                        for (int i = 0; i < legsCount; i++)
                        {
                            var legLength = piece.FoundationData.LegLengths[i];
                            sb.AppendLine($"{pillarsVariable}[{i}].root.SetActive(true);");
                            sb.AppendLine(
                                $"{pillarsVariable}[{i}].adjustable.localScale = new Vector3(1, 1, {legLength}f);");
                            if (piece.FoundationData.HasFeet)
                                sb.AppendLine(
                                    $"{pillarsVariable}[{i}].bottom.position = {pillarsVariable}[{i}].adjustable.position + {pillarsVariable}[{i}].adjustable.forward * {legLength}f;");
                        }

                        legsIndex++;
                    }
                }
                sb.AppendLine($"StripComponents({variableName});");
                objIndex++;
            }
        }

        var text = sb.ToString();
        var dateString = System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
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

    private struct SpawnablePiece
    {
        public string Name { get; }
        public string AddressablePath { get; }
        public Vector3 LocalPosition { get; }
        public Quaternion LocalRotation { get; }
        public FoundationData FoundationData { get; set; }

        public SpawnablePiece(string name, Vector3 localPosition, Quaternion localRotation)
        {
            Name = name;
            AddressablePath = PathPrefix + name + ".prefab";
            LocalPosition = localPosition;
            LocalRotation = localRotation;
        }
    }

    private readonly struct FoundationData
    {
        public float[] LegLengths { get; }
        public string PathToLegs { get; }
        public bool HasLegs { get; }
        public bool HasFeet { get; }

        public FoundationData(float[] legLengths, string pathToLegs, bool hasFeet)
        {
            LegLengths = legLengths;
            PathToLegs = pathToLegs;
            HasLegs = legLengths.Length > 0;
            HasFeet = hasFeet;
        }
    }
}