using Nautilus.Assets;

namespace PodshellLeviathan.Prefabs;

public class PodshellLeviathanJuvenilePrefab : PodshellLeviathanPrefab
{
    public PodshellLeviathanJuvenilePrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
    }

    protected override float StandardSwimVelocity => 2.8f;
    protected override string ModelName => "PodshellLeviathanPrefab";
    protected override float MaxHealth => 5000;
}