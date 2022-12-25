using System.Collections.Generic;
using UnityEngine;
using SMLHelper.V2.Assets;
using UWE;
using System.Collections;

namespace ShipMod.Prefabs
{
    public class SeaVoyagerFragment : Spawnable
    {
        private GameObject _model;
        private float _mass;

        private GameObject _prefab;

        public SeaVoyagerFragment(string classId, GameObject model, float mass) : base(classId, "Sea Voyager fragment", "A fragment of a destroyed Sea Voyager.")
        {
            _model = model;
            _mass = mass;
        }

        public override WorldEntityInfo EntityInfo => new WorldEntityInfo()
        {
            classId = ClassID,
            cellLevel = LargeWorldEntity.CellLevel.Medium,
            localScale = Vector3.one,
            slotType = EntitySlot.Type.Medium,
            techType = TechType
        };

        public override GameObject GetGameObject()
        {
            if (_prefab == null)
            {
                _prefab = Object.Instantiate(_model);
                _prefab.SetActive(false);
                _prefab.AddComponent<PrefabIdentifier>().ClassId = ClassID;
                _prefab.AddComponent<TechTag>().type = QPatch.seaVoyagerFragmentTechType;
                _prefab.AddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Medium;
                Helpers.ApplyMaterials(_prefab);
                _prefab.AddComponent<SkyApplier>().renderers = _prefab.GetComponentsInChildren<Renderer>(true);
                var rb = _prefab.AddComponent<Rigidbody>();
                rb.mass = _mass;
                rb.useGravity = false;
                rb.isKinematic = true;
                var wf = _prefab.AddComponent<WorldForces>();
                wf.useRigidbody = rb;
                _prefab.SetActive(true);
            }
            return _prefab;
        }

        protected override void ProcessPrefab(GameObject go)
        {
            ModPrefabCache.AddPrefab(go);
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            gameObject.Set(GetGameObject());
            yield break;
        }

        public override List<LootDistributionData.BiomeData> BiomesToSpawnIn => new List<LootDistributionData.BiomeData>()
        {
            new LootDistributionData.BiomeData()
            {
                biome = BiomeType.Kelp_Sand,
                probability = 0.05f,
                count = 1
            },
            new LootDistributionData.BiomeData()
            {
                biome = BiomeType.Kelp_GrassSparse,
                probability = 0.05f,
                count = 1
            }
        };
    }
}
