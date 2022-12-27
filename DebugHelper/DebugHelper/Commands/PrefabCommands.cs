using DebugHelper.Systems;
using UnityEngine;
using SMLHelper.V2.Commands;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UWE;

namespace DebugHelper.Commands
{
    public static class PrefabCommands
    {
        private static GameObject signPrefab;

        [ConsoleCommand("spawnp")]
        public static void SpawnByPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                ErrorMessage.AddMessage("Correct syntax: 'spawnp [path]'.");
            }
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab != null)
            {
                ErrorMessage.AddMessage($"Spawned prefab '{prefab.name}' successfully.");
                Object.Instantiate(prefab, Player.main.transform.position + (Player.main.transform.forward * 5f), Quaternion.identity);
            }
            else
            {
                ErrorMessage.AddMessage($"Failed to load prefab by path '{path}'.");
            }
        }

        [ConsoleCommand("spawnc")]
        public static void SpawnByClassId(string classId)
        {
            if (string.IsNullOrEmpty(classId))
            {
                ErrorMessage.AddMessage("Correct syntax: 'spawnc [ClassId]'.");
            }
            CoroutineHost.StartCoroutine(SpawncCoroutine(classId));
        }

        private static IEnumerator SpawncCoroutine(string classId)
        {
            var prefabRequest = PrefabDatabase.GetPrefabAsync(classId);
            yield return prefabRequest;
            if (prefabRequest.TryGetPrefab(out GameObject prefab))
            {
                ErrorMessage.AddMessage($"Spawned prefab '{prefab.name}' successfully.");
                Utils.CreatePrefab(prefab, 12f, false);
            }
            else
            {
                ErrorMessage.AddMessage($"Failed to load prefab by ClassId '{classId}'.");
            }
        }

        [ConsoleCommand("search")]
        public static void Search(float distance)
        {
            float maxDist = float.MaxValue;
            if (distance >= 0f) maxDist = distance;
            foreach (PrefabIdentifier prefabIdentifier in Object.FindObjectsOfType<PrefabIdentifier>())
            {
                var dist = Vector3.Distance(SNCameraRoot.main.transform.position, prefabIdentifier.transform.position);
                if (dist < maxDist)
                {
                    var techType = CraftData.GetTechType(prefabIdentifier.gameObject);
                    if (techType == TechType.None)
                    {
                        ErrorMessage.AddMessage($"Found object '{prefabIdentifier.gameObject.name}' ({(int)dist}m away)");
                    }
                    else
                    {
                        ErrorMessage.AddMessage($"Found object '{prefabIdentifier.gameObject.name}' with TechType '{techType}' ({(int)dist}m away)");
                    }
                }
            }
        }

        [ConsoleCommand("entgal")]
        public static void EntityGallery()
        {
            CoroutineHost.StartCoroutine(EntgalCoroutine());
        }

        private static IEnumerator EntgalCoroutine()
        {
            if (signPrefab == null)
            {
                IPrefabRequest request = PrefabDatabase.GetPrefabAsync("19524fc9-f1cc-4bc9-9404-94aaaf3e81a0");
                yield return request;
                request.TryGetPrefab(out signPrefab);
            }

            Vector3 offset = new Vector3(-1250f, -10f, -1250f);
            Player.main.SetPosition(offset);
            GameObject[] objects = Resources.LoadAll<GameObject>("WorldEntities");
            const float verticalSpacing = 10f;
            const float horizontalSpacing = 30f;
            Vector3 vertical = Vector3.forward;
            Vector3 horizontal = Vector3.right;
            int verticalOffset = 0;
            int horizontalIterations = 0;
            for (int i = 0; i < objects.Length; i++)
            {
                SpawnEntgalEntity(objects[i], offset + (vertical * (verticalSpacing * verticalOffset)) + (horizontal * (horizontalSpacing * horizontalIterations)));
                if (verticalOffset < 400)
                {
                    verticalOffset++;
                }
                else
                {
                    verticalOffset = 0;
                    horizontalIterations++;
                }
                yield return null;
            }
            ErrorMessage.AddMessage("Successfully spawned " + objects.Length.ToString() + " entities.");
        }

        static void SpawnEntgalEntity(GameObject prefab, Vector3 position)
        {
            if (prefab.name == "Cold") return;
            GameObject spawnedPrefab = Object.Instantiate(prefab, position, Quaternion.identity);
            Rigidbody rb = spawnedPrefab.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
            GameObject signGameObject = Object.Instantiate(signPrefab, position + (Vector3.right * 2f), Quaternion.LookRotation(Vector3.right));
            GenericSign sign = signGameObject.GetComponent<GenericSign>();
            sign.key = prefab.name;
            sign.UpdateCanvas();
        }

        private static List<ClassIDButton> classIDButtons = new List<ClassIDButton>();

        [ConsoleCommand("showclassids")]
        public static void ShowClassIDs(float inRange, bool hideMessage = false)
        {
            HideClassIDs();
            var comparePosition = SNCameraRoot.main.transform.position;
            var actualDistanceThreshold = inRange < 0f ? float.MaxValue : inRange;
            var all = Object.FindObjectsOfType<PrefabIdentifier>();
            var squareDistance = actualDistanceThreshold * actualDistanceThreshold;
            int count = 0;
            foreach (var prefabIdentifier in all)
            {
                if (Vector3.SqrMagnitude(prefabIdentifier.transform.position - comparePosition) < squareDistance)
                {
                    var component = prefabIdentifier.gameObject.EnsureComponent<ClassIDButton>();
                    component.prefabIdentifier = prefabIdentifier;
                    classIDButtons.Add(component);
                    count++;
                }
            }
            if (!hideMessage) ErrorMessage.AddMessage($"Showing {count} ClassIDs within a range of {actualDistanceThreshold} meters.\nUse the keybinds specified in mod options to copy them to your clipboard.");
        }

        [ConsoleCommand("hideclassids")]
        public static void HideClassIDs()
        {
            foreach (var rendered in classIDButtons)
            {
                if (rendered != null)
                {
                    Object.DestroyImmediate(rendered);
                }
            }
            classIDButtons.Clear();
        }

        private class ClassIDButton : CopyToClipboardDebugIcon
        {
            public PrefabIdentifier prefabIdentifier;

            private Color vanillaColor = Color.white;
            private Color moddedColor = Color.cyan;
            private Color unimportantColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            private static int vanillaClassIDLength = 36; // MOST modded ClassIDs will be nowhere near this long. However we can expect there to be exceptions!
            private static int vanillaClassIDDashes = 4; // MOST modded ClassIDs will NOT have 4 dashes. However we can expect there to be exceptions!
            private static string cellRootClassID = "55d7ab35-de97-4d95-af6c-ac8d03bb54ca";

            private bool Destroyed
            {
                get
                {
                    return prefabIdentifier == null;
                }
            }

            private bool Invalid
            {
                get
                {
                    return Destroyed || string.IsNullOrEmpty(prefabIdentifier.ClassId);
                }
            }

            private bool IsUnimportant
            {
                get
                {
                    return prefabIdentifier.ClassId.Equals(cellRootClassID);
                }
            }

            private bool ModdedPrefabGuess
            {
                get
                {
                    return prefabIdentifier.ClassId.Length != vanillaClassIDLength || prefabIdentifier.ClassId.Count(c => c == '-') != vanillaClassIDDashes;
                }
            }

            public override string Label
            {
                get
                {
                    if (Destroyed) return "Destroyed";
                    if (Invalid) return "Invalid";
                    return prefabIdentifier.ClassId;
                }
            }

            public override Vector3 Position => transform.position;

            public override Color Color
            {
                get
                {
                    if (Invalid)
                    {
                        return invalidColor;
                    }
                    if (IsUnimportant)
                    {
                        return unimportantColor;
                    }
                    if (ModdedPrefabGuess)
                    {
                        return moddedColor;
                    }
                    return vanillaColor;
                }
            }

            public override void OnInteract()
            {
                if (Invalid)
                {
                    ErrorMessage.AddMessage("Failed to find ClassID.");
                    return;
                }
                CopyToClipboard(prefabIdentifier.ClassId);
                ErrorMessage.AddMessage($"ClassID '{prefabIdentifier.ClassId}' on object '{prefabIdentifier.gameObject.name}' copied to clipboard.\nThis prefab can be spawned with the `spawnc` command.");
            }
        }

        private static List<SpawnInfoButton> coordinatedSpawnButtons = new List<SpawnInfoButton>();

        [ConsoleCommand("showspawninfo")]
        public static void ShowSpawnInfo(float inRange, bool hideMessage = false)
        {
            HideSpawnInfo();
            var comparePosition = SNCameraRoot.main.transform.position;
            var actualDistanceThreshold = inRange < 0f ? float.MaxValue : inRange;
            var all = Object.FindObjectsOfType<PrefabIdentifier>();
            var squareDistance = actualDistanceThreshold * actualDistanceThreshold;
            int count = 0;
            foreach (var prefabIdentifier in all)
            {
                var prefabTransform = prefabIdentifier.transform;
                if (Vector3.SqrMagnitude(prefabTransform.position - comparePosition) < squareDistance)
                {
                    var component = prefabIdentifier.gameObject.EnsureComponent<SpawnInfoButton>();
                    component.gameObjectName = prefabIdentifier.gameObject.name;
                    component.classID = prefabIdentifier.ClassId;
                    component.position = prefabTransform.position;
                    component.rotation = prefabTransform.rotation;
                    coordinatedSpawnButtons.Add(component);
                    count++;
                }
            }
            if (!hideMessage) ErrorMessage.AddMessage($"Showing {count} ClassIDs within a range of {actualDistanceThreshold} meters.\nUse the keybinds specified in mod options to copy them to your clipboard.");
        }

        [ConsoleCommand("hidespawninfo")]
        public static void HideSpawnInfo()
        {
            foreach (var rendered in coordinatedSpawnButtons)
            {
                if (rendered != null)
                {
                    Object.DestroyImmediate(rendered);
                }
            }
            coordinatedSpawnButtons.Clear();
        }

        private class SpawnInfoButton : CopyToClipboardDebugIcon
        {
            public string gameObjectName;

            public string classID;
            public Vector3 position;
            public Quaternion rotation;

            private static Color color = new Color(0f, 1f, 195f / 255f);

            public override string Label
            {
                get
                {
                    return gameObjectName;
                }
            }

            public override Vector3 Position => position;

            public override Color Color
            {
                get
                {
                    if (string.IsNullOrEmpty(classID))
                    {
                        return invalidColor;
                    }
                    return Color.magenta;
                }
            }

            public static string GenerateSpawnInfo(string classId, Vector3 position, Vector3 eulerAngles)
            {
                string RoundFloat(float num)
                {
                    return num.ToString("F1") + "f";
                }
                return string.Format("new SpawnInfo(\"{0}\", {1}, {2})",
                    classId,
                    $"new Vector3({RoundFloat(position.x)}, {RoundFloat(position.y)}, {RoundFloat(position.z)})",
                    $"new Vector3({RoundFloat(eulerAngles.x)}, {RoundFloat(eulerAngles.y)}, {RoundFloat(eulerAngles.z)})"
                    );
            }

            private Vector3 EulerAngles
            {
                get
                {
                    return rotation.eulerAngles;
                }
            }

            public override void OnInteract()
            {
                var spawnInfo = GenerateSpawnInfo(classID, position, EulerAngles);
                CopyToClipboard(spawnInfo);
                ErrorMessage.AddMessage($"A basic constructor for SpawnInfo with the last known position/rotation of object '{gameObjectName}' has been copied to clipboard.");
            }
        }
    }
}
