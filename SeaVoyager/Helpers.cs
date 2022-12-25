using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FMOD;
using FMODUnity;

namespace ShipMod
{
    public static class Helpers
    {
        public static GameObject FindChild(GameObject parent, string byName)
        {
            foreach (Transform child in parent.transform)
            {
                if (child.gameObject.name == byName)
                {
                    return child.gameObject;
                }
                GameObject recursive = FindChild(child.gameObject, byName);
                if(recursive)
                {
                    return recursive;
                }
            }
            return null;
        }
    }

    public static class GameObjectExtensions
    {
        public static GameObject SearchChild(this GameObject gameObject, string byName)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.name == byName)
                {
                    return child.gameObject;
                }
                GameObject recursive = SearchChild(child.gameObject, byName);
                if (recursive)
                {
                    return recursive;
                }
            }
            return null;
        }
        public static T SearchComponent<T>(this GameObject gameObject, string gameObjectName)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.gameObject.name == gameObjectName)
                {
                    return child.gameObject.GetComponent<T>();
                }
                GameObject recursive = SearchChild(child.gameObject, gameObjectName);
                if (recursive)
                {
                    return recursive.GetComponent<T>();
                }
            }
            return default;
        }
    }
}
