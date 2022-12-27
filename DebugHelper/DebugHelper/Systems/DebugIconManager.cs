using DebugHelper.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace DebugHelper.Systems
{
    public class DebugIconManager : MonoBehaviour
    {
        public static DebugIconManager Main
        {
            get
            {
                if (main == null)
                {
                    main = new GameObject("DebugIconManager").AddComponent<DebugIconManager>();
                }
                return main;
            }
        }

        private static DebugIconManager main;

        private RectTransform canvas;
        private GameObject debugIconInstancePrefab;

        public const float kInactiveComponentAlpha = 0.3f;

        private void Awake()
        {
            var canvasObj = Instantiate(DebugHelper.Main.assetBundle.LoadAsset<GameObject>("DebugIconCanvas"), transform);
            canvas = canvasObj.GetComponent<RectTransform>();
            debugIconInstancePrefab = DebugHelper.Main.assetBundle.LoadAsset<GameObject>("DebugIconInstance");
        }

        public void Register(IDebugIcon debugIcon)
        {
            AddToPool(debugIcon);
        }

        public void Unregister(IDebugIcon debugIcon)
        {
            RemoveFromPool(debugIcon);
        }

        private void LateUpdate()
        {
            foreach (var poolItem in pool)
            {
                if (!poolItem.CheckIfUnused())
                {
                    poolItem.UpdateUI();
                }
            }
            CleanupPool();
        }

        private void CleanupPool() // called every frame, just removes up to ONE per frame
        {
            foreach (var poolItem in pool)
            {
                if (poolItem.CheckIfUnused())
                {
                    if (poolItem.instance != null)
                    {
                        Destroy(poolItem.instance.gameObject);
                    }
                    pool.Remove(poolItem);
                    return;
                }
            }
        }

        private void AddToPool(IDebugIcon debugIcon)
        {
            foreach (var poolItem in pool)
            {
                if (poolItem.CheckIfUnused())
                {
                    poolItem.AssignInterface(debugIcon);
                    return;
                }
            }
            var item = new PoolItem();
            item.AssignInterface(debugIcon);
            pool.Add(item);
        }

        private void RemoveFromPool(IDebugIcon debugIcon)
        {
            if (debugIcon == null) return;
            foreach (var poolItem in pool)
            {
                if (poolItem.debugIcon == debugIcon)
                {
                    poolItem.Remove();
                    return;
                }
            }
        }

        private List<PoolItem> pool = new List<PoolItem>();

        private class PoolItem
        {
            public IDebugIcon debugIcon;
            public DebugIconInstance instance;
            public UnityEngine.Object componentForInterface;
            public bool removed;

            public PoolItem()
            {
            }

            public void AssignInterface(IDebugIcon debugIcon)
            {
                removed = false;
                this.debugIcon = debugIcon;
                if (instance == null)
                {
                    CreateUIObject(debugIcon);
                }
                debugIcon.OnCreation(instance);
                instance.gameObject.SetActive(true);
                if (debugIcon is UnityEngine.Object component)
                {
                    componentForInterface = component;
                }
            }

            public void UpdateUI()
            {
                instance.UpdateAppearance(debugIcon);
            }

            public bool CheckIfUnused()
            {
                if (removed || instance == null || !instance.gameObject.activeSelf || debugIcon == null)
                {
                    return true;
                }
                if (debugIcon is UnityEngine.Object component)
                {
                    if (component == null)
                    {
                        return true;
                    }
                }
                return false;
            }

            private void CreateUIObject(IDebugIcon debugIcon)
            {
                instance = Instantiate(Main.debugIconInstancePrefab).AddComponent<DebugIconInstance>();
                instance.GetComponent<RectTransform>().SetParent(Main.canvas);
                instance.UpdateAppearance(debugIcon);
            }

            public void Remove()
            {
                debugIcon = null;
                componentForInterface = null;
                if (instance) instance.gameObject.SetActive(false);
                removed = true;
            }
        }

        public static class Icons
        {
            public static Sprite Audio { get; private set; }
            public static Sprite AudioLooping { get; private set; }
            public static Sprite CheckSymbol { get; private set; }
            public static Sprite Circle { get; private set; }
            public static Sprite Clipboard { get; private set; }
            public static Sprite CubeHollow { get; private set; }
            public static Sprite CubeSolid { get; private set; }
            public static Sprite CubeWireframe { get; private set; }
            public static Sprite Diamond { get; private set; }
            public static Sprite GearHolllow { get; private set; }
            public static Sprite GearSolid { get; private set; }
            public static Sprite Globe { get; private set; }
            public static Sprite Health { get; private set; }
            public static Sprite Light { get; private set; }
            public static Sprite Lightning { get; private set; }
            public static Sprite Peeper { get; private set; }
            public static Sprite Pointer { get; private set; }
            public static Sprite Question { get; private set; }
            public static Sprite Spotlight { get; private set; }
            public static Sprite Square { get; private set; }
            public static Sprite Sun { get; private set; }
            public static Sprite XSymbol { get; private set; }

            internal static void LoadIcons(AssetBundle assets)
            {
                Audio = assets.LoadAsset<Sprite>("Icon-Audio");
                AudioLooping = assets.LoadAsset<Sprite>("Icon-Audio-Looping");
                CheckSymbol = assets.LoadAsset<Sprite>("Icon-Check");
                Circle = assets.LoadAsset<Sprite>("Icon-Circle");
                Clipboard = assets.LoadAsset<Sprite>("Icon-Clipboard");
                CubeHollow = assets.LoadAsset<Sprite>("Icon-Cube-Hollow");
                CubeSolid = assets.LoadAsset<Sprite>("Icon-Cube-Solid");
                CubeWireframe = assets.LoadAsset<Sprite>("Icon-Cube-Wireframe");
                Diamond = assets.LoadAsset<Sprite>("Icon-Diamond");
                GearHolllow = assets.LoadAsset<Sprite>("Icon-Gear-Hollow");
                GearSolid = assets.LoadAsset<Sprite>("Icon-Gear-Solid");
                Globe = assets.LoadAsset<Sprite>("Icon-Globe");
                Health = assets.LoadAsset<Sprite>("Icon-Health");
                Light = assets.LoadAsset<Sprite>("Icon-Light");
                Lightning = assets.LoadAsset<Sprite>("Icon-Lightning");
                Peeper = assets.LoadAsset<Sprite>("Icon-Peeper");
                Pointer = assets.LoadAsset<Sprite>("Icon-Pointer");
                Question = assets.LoadAsset<Sprite>("Icon-Question");
                Spotlight = assets.LoadAsset<Sprite>("Icon-Spotlight");
                Square = assets.LoadAsset<Sprite>("Icon-Square");
                Sun = assets.LoadAsset<Sprite>("Icon-Sun");
                XSymbol = assets.LoadAsset<Sprite>("Icon-X");
            }
        }
    }
}
