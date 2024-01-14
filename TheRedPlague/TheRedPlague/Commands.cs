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
}