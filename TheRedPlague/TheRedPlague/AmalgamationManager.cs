using System.Collections;
using System.Collections.Generic;
using TheRedPlague.Mono;
using UnityEngine;

namespace TheRedPlague;

public static class AmalgamationManager
{
    private static readonly int InfectionHeightStrength = Shader.PropertyToID("_InfectionHeightStrength");

    private static readonly List<TechType> LeviathanTechTypes = new()
    {
        TechType.ReaperLeviathan,
        TechType.GhostLeviathan,
        TechType.GhostLeviathanJuvenile,
        TechType.SeaDragon
    };

    private const float LeviathanProbabilityScale = 8;
    
    public static void AmalgamateCreature(GameObject host)
    {
        UWE.CoroutineHost.StartCoroutine(AmalgamateCreatureInternal(host));
    }

    private static IEnumerator AmalgamateCreatureInternal(GameObject host)
    {
        var techType = CraftData.GetTechType(host);

        if (AmalgamationSettingsDatabase.CustomModificationsList.TryGetValue(techType, out var customModification))
        {
            yield return customModification(host);
        }
        
        if (!AmalgamationSettingsDatabase.SettingsList.TryGetValue(techType, out var settings))
            yield break;

        if (host.transform.parent != null && host.transform.parent.gameObject.GetComponentInParent<Creature>() != null)
            yield break;

        var probabilityScale = Mathf.Clamp(ZombieManager.GetInfectionStrengthAtPosition(host.transform.position), 0.05f, 1f);
        if (LeviathanTechTypes.Contains(techType))
        {
            probabilityScale = Mathf.Clamp01(probabilityScale * LeviathanProbabilityScale);
            if (WaterBiomeManager.main.GetBiome(host.transform.position) == "dunes")
            {
                probabilityScale = 1f;
            }
        }
        
        foreach (var attachPoint in settings.AttachPoints)
        {
            foreach (var bone in attachPoint.PathToAffectedBone)
            {
                if (Random.value <= attachPoint.Probability * probabilityScale)
                {
                    yield return AttachCreatureToHost(host, attachPoint, bone);
                }
            }
        }

        host.EnsureComponent<DropAmalgamatedBoneOnDeath>();
    }

    private static IEnumerator AttachCreatureToHost(GameObject host, ParasiteAttachPoint parasiteAttachPoint, string chosenBone)
    {
        // Shrink the bone
        var attachmentBone = host.transform.Find(chosenBone);
        if (attachmentBone == null)
        {
            Plugin.Logger.LogWarning($"Could not find attachment bone of path '{chosenBone}' on host {host}!");
            yield break;
        }
        foreach (var unaffectedChild in parasiteAttachPoint.UnaffectedChildObjects)
        {
            var childObj = attachmentBone.Find(unaffectedChild);
            if (childObj != null) childObj.parent = attachmentBone.parent;
        }
        if (parasiteAttachPoint.RemoveBodyPart)
            attachmentBone.transform.localScale *= 0.01f;

        // Create parasite
        var chosenParasite = parasiteAttachPoint.AttachableCreatures[Random.Range(0, parasiteAttachPoint.AttachableCreatures.Length)];
        var parasitePrefabTask = CraftData.GetPrefabForTechTypeAsync(chosenParasite.Type);
        yield return parasitePrefabTask;
        if (attachmentBone == null)
        {
            Plugin.Logger.LogWarning($"Attachment bone became null! The host might've died.");
            yield break;
        }
        var parasitePrefab = parasitePrefabTask.GetResult();
        if (parasitePrefab == null)
        {
            Plugin.Logger.LogWarning($"Parasite prefab for TechType {chosenParasite.Type} not found!");
            yield break;
        }
        var parasite = Object.Instantiate(parasitePrefab, attachmentBone);
        Object.Destroy(parasite.GetComponent<LargeWorldEntity>());
        Object.Destroy(parasite.GetComponent<PrefabIdentifier>());
        parasite.SetActive(true);
        
        // Position the parasite properly
        var parasiteTransform = parasite.transform;
        parasiteTransform.localPosition = Vector3.zero;
        parasiteTransform.localEulerAngles = parasiteAttachPoint.LocalEulerAngles;
        
        // Scale the parasite properly
        var lossyBoneScale = attachmentBone.lossyScale;
        var parasiteScale = new Vector3(chosenParasite.Scale / lossyBoneScale.x,
            chosenParasite.Scale / lossyBoneScale.y, chosenParasite.Scale / lossyBoneScale.z);
        parasiteTransform.localScale = parasiteScale;
        var parasiteAverageScale = (parasiteScale.x + parasiteScale.y + parasiteScale.z) / 3f;
        parasite.AddComponent<FixMassiveCreatures>().desiredLossyScale = chosenParasite.Scale;

        if (!string.IsNullOrEmpty(chosenParasite.DecapitationPoint))
        {
            var decapitationPoint = parasiteTransform.Find(chosenParasite.DecapitationPoint);
            if (decapitationPoint) decapitationPoint.localScale = Vector3.one * 0.001f;
            else Plugin.Logger.LogWarning($"Failed to find point at path '{chosenParasite.DecapitationPoint}'");
        }
        
        // Disable the parasite's movement
        foreach (var collider in parasite.GetComponentsInChildren<Collider>(true))
        {
            if (collider.isTrigger) continue;
            collider.enabled = false;
        }

        var creature = parasite.GetComponent<Creature>();
        if (creature != null)
        {
            creature.enabled = false;
            creature.SetSize(chosenParasite.Scale);
        }

        var parasiteRb = parasite.GetComponent<Rigidbody>();
        if (parasiteRb != null) parasiteRb.isKinematic = true;
        
        ZombieManager.Zombify(parasite);

        yield return null;

        if (parasite == null)
        {
            Plugin.Logger.LogWarning($"Parasite was destroyed!");
            yield break;
        }
        
        foreach (var renderer in parasite.GetComponentsInChildren<Renderer>(true))
        {
            foreach (var material in renderer.materials)
            {
                if (!material) continue;
                if (material.HasProperty(InfectionHeightStrength))
                    material.SetFloat(InfectionHeightStrength, Mathf.Abs(material.GetFloat(InfectionHeightStrength) / parasiteAverageScale));
            }
        }
    }
}