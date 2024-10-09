using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Handlers;
using UnityEngine;
using UWE;

namespace TheRedPlague.PrefabFiles.Equipment;

public static class InfectionSamplerFragment
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("InfectionSamplerFragment");

    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(new CloneTemplate(Info, "f4146f7a-d334-404a-abdc-dff98365eb10")
        {
            ModifyPrefab = obj =>
            {
                var rb = obj.GetComponent<Rigidbody>();
                if (rb) rb.isKinematic = true;
            }
        });
        prefab.SetSpawns(new WorldEntityInfo()
            {
                classId = Info.ClassID,
                techType = Info.TechType,
                prefabZUp = true,
                cellLevel = LargeWorldEntity.CellLevel.Near,
                slotType = EntitySlot.Type.Small
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.CrashZone_Sand,
                count = 1,
                probability = 0.2f
            },
            new LootDistributionData.BiomeData
            {
                biome = BiomeType.CrashZone_Rock,
                count = 1,
                probability = 0.1f
            });
        prefab.Register();
        PDAHandler.AddCustomScannerEntry(Info.TechType, InfectionSamplerTool.Info.TechType, true, 2);
    }
}