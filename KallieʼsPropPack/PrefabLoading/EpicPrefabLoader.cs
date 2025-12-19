using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KallieʼsPropPack.PrefabLoading;

public class EpicPrefabLoader
{
    public EpicPrefabLoader(IEnumerable<Assembly> assemblies, AssetBundle bundle)
    {
        _dependencyData = new DependencyData(assemblies);
        Bundle = bundle;
    }

    private AssetBundle Bundle { get; }

    private readonly DependencyData _dependencyData;

    public void LoadPrefabs(LoadedPrefabRegistrationData data)
    {
        _dependencyData.ResolveDependencies(data.families);

        data.families.ForEach(family =>
        {
            var variantCount = 1;
            if (family.variants != null && family.variants.Length > 0)
            {
                variantCount = family.variants.Length;
            }
            if (!_dependencyData.TryGetFactory(family.factoryClass, out var factory))
            {
                Plugin.Logger.LogError($"Failed to find factory class '{family.factoryClass}' for prefab family '{family.name}'");
                return;
            }
            foreach (var prefabName in family.prefabs)
            {
                for (int i = 0; i < variantCount; i++)
                {
                    var variant = family.variants == null || family.variants.Length == 0 ? null : family.variants?[i];
                    var prefix = data.prefix + family.prefix;
                    var postfix = family.postfix + data.postfix;
                    var classId = prefix + prefabName + variant?.postfix + postfix;
                    var info = PrefabInfo.WithTechType(classId);
                    var prefab = new CustomPrefab(info);
                    WorldEntityDatabaseHandler.AddCustomInfo(info.ClassID, info.TechType, Vector3.one, family.zUp, family.cellLevel);
                    prefab.SetGameObject(new EpicPrefabTemplate(info, Bundle, prefabName, family, variant, factory));
                    prefab.Register();
                }   
            }
        });
    }

    private class EpicPrefabTemplate : PrefabTemplate
    {
        public AssetBundle Bundle { get; }
        public string PrefabName { get; }
        public LoadedPrefabRegistrationData.PrefabFamily Family { get; }
        public IEpicPrefabFactory Factory { get; }
        public LoadedPrefabRegistrationData.PrefabVariant Variant { get; } // Can be null

        public EpicPrefabTemplate(PrefabInfo info, AssetBundle bundle, string prefabName,
            LoadedPrefabRegistrationData.PrefabFamily family, LoadedPrefabRegistrationData.PrefabVariant variant,
            IEpicPrefabFactory factory) : base(info)
        {
            Bundle = bundle;
            PrefabName = prefabName;
            Family = family;
            Variant = variant;
            Factory = factory;
        }

        public override IEnumerator GetPrefabAsync(TaskResult<GameObject> gameObject)
        {
            var request = Bundle.LoadAssetAsync<GameObject>(PrefabName);
            yield return new WaitUntil(() => request.isDone);
            var prefabAsset = request.asset as GameObject;
            if (prefabAsset == null)
            {
                Plugin.Logger.LogError($"Failed to load prefab '{PrefabName}' from Bundle '{Bundle}'");
            }

            var prefab = Object.Instantiate(prefabAsset);
            prefab.SetActive(false);
            PrefabUtils.AddBasicComponents(prefab, info.ClassID, info.TechType, Family.cellLevel);
            var colliders = prefab.GetComponentsInChildren<Collider>(true);
            foreach (var collider in colliders)
            {
                collider.gameObject.EnsureComponent<VFXSurface>().surfaceType = Family.surfaceType;
            }

            MaterialUtils.ApplySNShaders(prefab, Family.materialSettings.shininess,
                Family.materialSettings.specularIntensity, Family.materialSettings.glowStrength, Factory.MaterialModifiers);

            if (Family.constructionObstacle)
            {
                prefab.AddComponent<ConstructionObstacle>();
            }
            
            if (Variant != null)
            {
                if (!string.IsNullOrEmpty(Variant.childModelPath))
                {
                    prefab.transform.Find(Variant.childModelPath).gameObject.SetActive(true);
                }
            }
            
            var variantParameters = Variant?.parameters ?? Array.Empty<LoadedPrefabRegistrationData.Parameter>();
            yield return Factory.BuildVariant(prefab, variantParameters);
            gameObject.Set(prefab);
        }
    }

    private class DependencyData
    {
        private IEnumerable<Assembly> SourceAssemblies { get; }
        private Dictionary<string, IEpicPrefabFactory> PrefabFactories { get; } = new();

        public DependencyData(IEnumerable<Assembly> sourceAssemblies)
        {
            SourceAssemblies = sourceAssemblies;
        }

        public bool TryGetFactory(string factoryClass, out IEpicPrefabFactory factory)
        {
            return PrefabFactories.TryGetValue(factoryClass, out factory);
        }

        public void ResolveDependencies(IEnumerable<LoadedPrefabRegistrationData.PrefabFamily> families)
        {
            foreach (var family in families)
            {
                var className = family.factoryClass;
                if (PrefabFactories.ContainsKey(className))
                {
                    continue;
                }

                Type factoryType = null;
                foreach (var assembly in SourceAssemblies)
                {
                    factoryType = assembly.GetType(className);
                    if (factoryType != null) break;
                }

                if (factoryType == null)
                {
                    Plugin.Logger.LogError(
                        $"Failed to load prefab factory class '{className}' from Source Assemblies {string.Join(", ", SourceAssemblies)}");
                    continue;
                }

                try
                {
                    var factoryClassInstance = Activator.CreateInstance(factoryType) as IEpicPrefabFactory;
                    PrefabFactories.Add(className, factoryClassInstance);
                }
                catch (Exception e)
                {
                    Plugin.Logger.LogError($"Exception thrown while instantiating factory class '{factoryType}': " + e);
                }
            }
        }
    }
}