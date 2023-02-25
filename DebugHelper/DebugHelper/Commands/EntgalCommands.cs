using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SMLHelper.Commands;
using UnityEngine;
using UWE;

namespace DebugHelper.Commands
{
    public static class EntgalCommands
    {
        private static GameObject signPrefab;

        private const string signClassId = "d41c1bcf-3993-478b-a22e-08e73461168e";

        private static float xSpacing = 4;

        private static float zSpacing = 4;


        [ConsoleCommand("entgal")]
        public static void EntityGallery()
        {
            CoroutineHost.StartCoroutine(EntgalCoroutine());
        }

        private static IEnumerator EntgalCoroutine()
        {
            string[] classIds = PrefabDatabase.prefabFiles.Keys.ToArray();
            ErrorMessage.AddMessage($"Spawning entgal with {classIds.Length} entities.");
            Vector3 worldPos = new Vector3(-1600, -30f, -1000);
            Player.main.SetPosition(worldPos + new Vector3(-10, 0, 0));
            for (int i = 0; i < classIds.Length; i++)
            {
                yield return InspectPrefab(classIds[i], worldPos);
                worldPos += new Vector3(0, 0, zSpacing);
                if (worldPos.z > 1000)
                {
                    worldPos = new Vector3(worldPos.x + xSpacing, worldPos.y, 0);
                }
            }
        }

        private static IEnumerator InspectPrefab(string classId, Vector3 pos)
        {
            if (signPrefab == null)
            {
                yield return GetSignPrefab();
            }
            var request = PrefabDatabase.GetPrefabAsync(classId);
            yield return request;
            if (!request.TryGetPrefab(out GameObject prefab))
            {
                yield break;
            }
            if (prefab.name == "Cold")
            {
                yield break;
            }
            if (classId == "ce23b9ee-fd98-4677-9919-20248356f7cf")
            {
                yield break;
            }
            GameObject spawned = Object.Instantiate(prefab);
            spawned.transform.position = pos;

            GameObject sign = SpawnSign(pos + new Vector3(3, 0, 0), Vector3.up * -90, prefab.name, Vector3.one);
            GameObject sign2 = SpawnSign(pos + new Vector3(3, -0.5f, 0), Vector3.up * -90, classId, Vector3.one * 0.5f);
            yield return null;
        }

        private static IEnumerator GetSignPrefab()
        {
            IPrefabRequest request = PrefabDatabase.GetPrefabAsync(signClassId);
            yield return request;
            if (request.TryGetPrefab(out signPrefab)) ;
        }

        private static GameObject SpawnSign(Vector3 coords, Vector3 rotation, string text, Vector3 scale)
        {
            if (signPrefab == null)
            {
                ErrorMessage.AddMessage("Sign prefab failed to load.");
                return null;
            }
            var signObj = Object.Instantiate(signPrefab, coords, Quaternion.Euler(rotation));
            signObj.transform.localScale = scale;
            GenericSign sign = signObj.GetComponentInChildren<GenericSign>();
            sign.key = text;
            sign.showBackground = true;
            sign.UpdateCanvas();
            return signObj;
        }
    }
}
