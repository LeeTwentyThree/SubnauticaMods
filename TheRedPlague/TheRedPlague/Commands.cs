using System;
using System.Collections;
using Nautilus.Commands;
using TheRedPlague.Mono;
using TheRedPlague.Mono.CinematicEvents;
using TheRedPlague.Mono.PlagueGarg;
using TheRedPlague.PrefabFiles;
using TheRedPlague.PrefabFiles.GargTeaser;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheRedPlague;

public static class Commands
{
    [ConsoleCommand("setinfectiondeathtimer")]
    public static void SetInfectionDeathTimer(float seconds)
    {
        PlayerInfectionDeath.SetDeathTimer(seconds);
    }
    
    [ConsoleCommand("jumpscare")]
    public static void JumpScare()
    {
        JumpScares.main.JumpScareNow();
    }
    
    [ConsoleCommand("spawndiver")]
    public static void SpawnDiver(string diverName)
    {
        var survivorManager = NpcSurvivorManager.main;
        if (survivorManager == null)
        {
            ErrorMessage.AddMessage("No NpcSurvivorManager found");
        }

        var survivors = survivorManager.gameObject.GetComponents<NpcSurvivor>();
        foreach (var survivor in survivors)
        {
            if (string.Equals(survivor.survivorName, diverName, StringComparison.OrdinalIgnoreCase))
            {
                survivor.ForceSpawnWithCommand();
                return;
            }
        }
    }
    
    [ConsoleCommand("prawnsuitcinematic")]
    public static void PrawnSuitCinematic()
    {
        Mono.PrawnSuitCinematic.PlayCinematic();
    }
    
    [ConsoleCommand("dropadminpod")]
    public static void SpawnAdminPod()
    {
        Mono.AdminDropPodFall.SpawnAdministratorDropPod();
    }
    
    [ConsoleCommand("thrusterevent")]
    public static void ThrusterEvent()
    {
        AuroraThrusterEvent.PlayCinematic();
    }
    
    [ConsoleCommand("gargteaserevent")]
    public static void PlayGargTeaserEvent()
    {
        GargTeaserEvent.PlayCinematic();
    }
    
        
    [ConsoleCommand("plaguegargbirth")]
    public static void PlagueGirthBirth()
    {
        GargCorpseBehavior.BirthPlagueGargs();
    }
    
    [ConsoleCommand("spawndome")]
    public static void SpawnDome()
    {
        UWE.CoroutineHost.StartCoroutine(SpawnDomeCoroutine());
    }

    private static IEnumerator SpawnDomeCoroutine()
    {
        var domeTask = CraftData.GetPrefabForTechTypeAsync(NewInfectionDome.Info.TechType);
        yield return domeTask;
        var dome = Object.Instantiate(domeTask.GetResult(), Vector3.up * 50, Quaternion.identity);
        StoryUtils.DomeConstructionEvent.Trigger();
        dome.SetActive(true);
        dome.transform.localScale = Vector3.one * 900;
    }
    
    [ConsoleCommand("shatterdome")]
    public static void ShatterDome()
    {
        DomeExplosion.ExplodeDome();
    }

    [ConsoleCommand("nuclearexplosion")]
    public static void NuclearExplosion()
    {
        Mono.CinematicEvents.NuclearExplosion.PlayCinematic();
    }
}