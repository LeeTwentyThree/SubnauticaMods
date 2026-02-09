using Nautilus.Assets;

namespace PodshellLeviathan.Prefabs;

public class PodshellLeviathanJuvenilePrefab : PodshellLeviathanPrefab
{
    public PodshellLeviathanJuvenilePrefab(PrefabInfo prefabInfo) : base(prefabInfo)
    {
    }

    protected override float StandardSwimVelocity => 2.8f;
    protected override string ModelName => "PodshellLeviathanJuvenilePrefab";
    protected override float MaxHealth => 4000;

    protected override float Mass => 1000;

    protected override ShellFragmentSettings FragmentSettings => new(true, 0.5f);
}