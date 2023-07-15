using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DeathContainer.Mono;

internal class DeathContainerBehaviour : MonoBehaviour
{
    private StorageContainer _container;
    private TextMeshProUGUI _text;
    private PingInstance _ping;

    public static List<Pickupable> droppedItems;

    private bool _isLoadingItems;
    private bool _disabled;

    private void Awake()
    {
        _container = GetComponentInChildren<StorageContainer>(true);
        _text = GetComponentInChildren<TextMeshProUGUI>(true);
        _ping = gameObject.EnsureComponent<PingInstance>();
    }

    private IEnumerator Start()
    {
        yield return null;
        var classId = GetComponent<PrefabIdentifier>().Id;
        if (SaveData.Main.graves.TryGetValue(classId, out var grave))
        {
            UpdateData(grave.deathNumber, grave.coords);
        }
        if (SaveData.Main.obtainedGraves.Contains(classId))
        {
            DisableInventory();
        }
    }

    // Should only be called ONCE in the container's lifetime
    public void Setup()
    {
        Vector2Int inventorySize = new Vector2Int(6, 8);
        var inventory = Inventory.main;
        if (inventory != null)
        {
            inventorySize = new Vector2Int(inventory.container.sizeX, inventory.container.sizeY);
        }
        _container.Resize(inventorySize.x, inventorySize.y);

        if (droppedItems == null)
            return;

        foreach (var item in droppedItems)
        {
            if (item == null)
                continue;
            item.gameObject.SetActive(false);
            InventoryItem ii = new InventoryItem(item);
            _container.container.UnsafeAdd(ii);
        }

        int deaths = SaveData.Main.deaths;

        UpdateData(deaths + 1, transform.position);

        SaveData.Main.deaths++;
        SaveData.Main.graves.Add(GetComponent<PrefabIdentifier>().Id, new SaveContainer(transform.position, deaths + 1, "Died lol"));

        _ping.SetColor(2);

        _isLoadingItems = false;
    }

    private void Update()
    {
        if (!_disabled && _isLoadingItems && _container != null && _container.container.count == 0)
        {
            DisableInventory();
        }
    }

    private void DisableInventory()
    {
        transform.Find("StorageContainer").gameObject.SetActive(false);
        _disabled = true;
    }

    private void UpdateData(int deathNumber, Vector3 coordinates)
    {
        _text.color = Color.red;
        _text.text = "DEATH #" + deathNumber;
        _ping.SetLabel($"Death #{deathNumber} ({(int)coordinates.x}, {(int)coordinates.y}, {(int)coordinates.z})");
    }

    public static void SpawnDeathContainer(Vector3 atPosition)
    {
        UWE.CoroutineHost.StartCoroutine(SpawnDeathContainerAsync(atPosition));
    }

    private static IEnumerator SpawnDeathContainerAsync(Vector3 position)
    {
        var task = CraftData.GetPrefabForTechTypeAsync(Items.Prefabs.DeathContainerPrefab.Info.TechType);
        yield return task;
        var spawned = Instantiate(task.GetResult());
        spawned.transform.position = position;
        var container = spawned.GetComponent<DeathContainerBehaviour>();
        container._isLoadingItems = true;
        yield return null;
        container.Setup();
    }
}
