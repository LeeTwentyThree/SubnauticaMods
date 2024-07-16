using System;
using Nautilus.Commands;
using TheRedPlague.Mono;

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
}