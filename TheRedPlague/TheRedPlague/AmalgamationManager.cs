using System.Collections;
using UnityEngine;

namespace TheRedPlague;

public static class AmalgamationManager
{
    private static readonly int InfectionHeightStrength = Shader.PropertyToID("_InfectionHeightStrength");

    public static void AmalgamateCreature(GameObject host)
    {
        UWE.CoroutineHost.StartCoroutine(AmalgamateCreatureInternal(host));
    }

    private static IEnumerator AmalgamateCreatureInternal(GameObject host)
    {
        var techType = CraftData.GetTechType(host);
        
        if (!AmalgamationSettingsDatabase.SettingsList.TryGetValue(techType, out var settings))
            yield break;

        foreach (var attachPoint in settings.AttachPoints)
        {
            foreach (var bone in attachPoint.PathToAffectedBone)
            {
                if (Random.value <= attachPoint.Probability)
                {
                    yield return AttachCreatureToHost(host, attachPoint, bone);
                }
            }
        }
    }

    private static IEnumerator AttachCreatureToHost(GameObject host, ParasiteAttachPoint parasiteAttachPoint, string chosenBone)
    {
        // Shrink the bone
        var attachmentBone = host.transform.Find(chosenBone);
        foreach (var unaffectedChild in parasiteAttachPoint.UnaffectedChildObjects)
        {
            attachmentBone.Find(unaffectedChild).parent = attachmentBone.parent;
        }
        if (parasiteAttachPoint.RemoveBodyPart)
            attachmentBone.transform.localScale *= 0.01f;

        // Create parasite
        var chosenParasite = parasiteAttachPoint.AttachableCreatures[Random.Range(0, parasiteAttachPoint.AttachableCreatures.Length)];
        var parasitePrefabTask = CraftData.GetPrefabForTechTypeAsync(chosenParasite.Type);
        yield return parasitePrefabTask;
        var parasite = Object.Instantiate(parasitePrefabTask.GetResult(), attachmentBone);
        Object.Destroy(parasite.GetComponent<LargeWorldEntity>());
        Object.Destroy(parasite.GetComponent<PrefabIdentifier>());
        parasite.SetActive(true);
        
        // Position the parasite properly
        var parasiteTransform = parasite.transform;
        parasiteTransform.localPosition = Vector3.zero;
        parasiteTransform.localEulerAngles = parasiteAttachPoint.LocalEulerAngles;
        var lossyBoneScale = attachmentBone.lossyScale;
        var parasiteScale = new Vector3(chosenParasite.Scale / lossyBoneScale.x,
            chosenParasite.Scale / lossyBoneScale.y, chosenParasite.Scale / lossyBoneScale.z);
        parasiteTransform.localScale = parasiteScale;
        var parasiteAverageScale = (parasiteScale.x + parasiteScale.y + parasiteScale.z) / 3f;
        
        // Disable the parasite's movement
        foreach (var collider in parasite.GetComponentsInChildren<Collider>(true))
        {
            if (collider.isTrigger) continue;
            collider.enabled = false;
        }

        var creature = parasite.GetComponent<Creature>();
        if (creature != null) creature.enabled = false;

        var parasiteRb = parasite.GetComponent<Rigidbody>();
        if (parasiteRb != null) parasiteRb.isKinematic = true;
        
        ZombieManager.Zombify(parasite);

        yield return null;
        
        foreach (var renderer in parasite.GetComponentsInChildren<Renderer>(true))
        {
            foreach (var material in renderer.materials)
            {
                if (!material) continue;
                material.SetFloat(InfectionHeightStrength, Mathf.Abs(3.5f * material.GetFloat(InfectionHeightStrength) / parasiteAverageScale));
            }
        }
    }
}