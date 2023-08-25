using UnityEngine;
using System.Collections;

namespace MonkeySayMonkeyGet.VoiceCommands;

public class Create : VoiceCommandBase
{
    public override bool AllowPartialInput => false;

    public override Priority Priority => Priority.Low;

    protected override bool IsValid(SpeechInput input)
    {
        var mentionedTechType = PhraseManager.GetReferencedTechType(input, out var _);
        if (mentionedTechType != TechType.None)
        {
            return true;
        }
        return false;
    }

    protected override void Perform(SpeechInput input)
    {
        UWE.CoroutineHost.StartCoroutine(CreateAsync(input));
    }

    private IEnumerator CreateAsync(SpeechInput input)
    {
        var hasToPickupItems = Utils.CannotDropItems();
        var wantsToPickupItems = PhraseManager.ContainsPhrase(input, PhraseManager.Give);

        var mentionedTechType = PhraseManager.GetReferencedTechType(input, out var _);

        var task = CraftData.GetPrefabForTechTypeAsync(mentionedTechType);

        yield return task;

        var prefab = task.GetResult();

        if (prefab == null)
        {
            yield break;
        }

        var pickupable = prefab.GetComponent<Pickupable>();
        var canBePickedUp = pickupable != null && pickupable.isPickupable;
        if (hasToPickupItems && !canBePickedUp)
        {
            yield break;
        }

        var inventory = Inventory.main;
        var itemSize = CraftData.GetItemSize(mentionedTechType);

        var mentionedNumber = PhraseManager.GetReferencedNumber(input, 1);
        for (int i = 0; i < mentionedNumber; i++)
        {
            if (hasToPickupItems || (canBePickedUp && wantsToPickupItems && inventory.HasRoomFor(itemSize.x, itemSize.y)))
            {
                AddToInventory(SpawnFromPrefab(prefab));
            }
            else if (!hasToPickupItems)
            {
                SpawnFromPrefab(prefab);
            }
        }
    }

    private GameObject SpawnFromPrefab(GameObject prefab)
    {
        var spawned = global::Utils.CreatePrefab(prefab);
        spawned.SetActive(true);
        return spawned;
    }

    private void AddToInventory(GameObject obj)
    {
        Inventory.main.ForcePickup(obj.GetComponent<Pickupable>());
    }

    public override float MinimumDelay => 2f;
}