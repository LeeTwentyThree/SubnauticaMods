using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace PdaUpgradeCards;

[Menu("PDA Upgrade Cards")]
public class ModConfig : ConfigFile
{
    [Toggle("Look for Red Hydra mod music")]
    public bool LoadHydraModMusic { get; set; } = true;

    [Toggle("Look for Red Plague Act 1 music")]
    public bool LoadRedPlagueMusic { get; set; } = true;
    
    [Toggle("Look for Return of the Ancients music")]
    public bool LoadReturnOfTheAncientsMusic { get; set; } = true;
}