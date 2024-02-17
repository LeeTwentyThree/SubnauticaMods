using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using UnityEngine;
using Nautilus.Assets.PrefabTemplates;

namespace TheRedPlague.PrefabFiles;

public static class IslandElevator
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("");

    public static void Register()
    {
        var islandElevator = new CustomPrefab(Info);
        var elevatorTemplate = new CloneTemplate(islandElevator.Info, "51e58608-a80b-4135-9143-add4ce77a42f");
        elevatorTemplate.ModifyPrefab += ModifyPrefab;
        islandElevator.SetGameObject(elevatorTemplate);
        islandElevator.SetSpawns(new SpawnLocation(new Vector3(-48.000f, 56.000f, -40.000f), Vector3.up * 90,
            new Vector3(1.5f, 3.45f, 1.5f)));
        islandElevator.Register();
    }

    private static void ModifyPrefab(GameObject prefab)
    {
        var transform = prefab.transform;
        transform.Find("precursor_gun_Elevator_shell").gameObject.SetActive(false);
        transform.Find("mesh").gameObject.SetActive(false);
        transform.Find("Elevator_Collision").gameObject.SetActive(false);
        transform.Find("CullVolumeManager").gameObject.SetActive(false);
        transform.Find("Occluder_precursor_gun_Elevator_shell").gameObject.SetActive(false);
        var fxParent = transform.Find("FX");
        foreach (var renderer in fxParent.GetComponentsInChildren<Renderer>(true))
        {
            if (renderer.gameObject.name.Contains("SquareLights"))
            {
                renderer.material.color = Color.red;
            }
            else if (renderer.gameObject.name.Contains("Tube"))
            {
                var materials = renderer.materials;
                materials[0].color = new Color(5, 2, 0.27f);
                materials[1].color = new Color(2, 0.38f, 0.43f);
                renderer.materials = materials;
            }
        }

        prefab.GetComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;

        prefab.transform.Find("FX/Point light").GetComponent<Light>().color = Color.red;
    }
}