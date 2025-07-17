using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace NoAutoPickUp2;

[Menu("No Auto Pickup")]
public class Config : ConfigFile
{
    [Toggle("Disable auto pickup for Fabricators")]
    public bool NoAutoPickupFabricator = true;

    [Toggle("Disable auto pickup for Modification Stations")]
    public bool NoAutoPickupModificationStation = true;

    [Toggle("Disable auto pickup for other crafting stations",
        Tooltip = "Cyclops Upgrade Fabricator, Scanner Room, and Vehicle Upgrade Console")]
    public bool NoAutoPickupOther = true;

    [Toggle("Disable auto pickup for modded stations",
        Tooltip = "WARNING: Enabling this option may lead to unexpected compatibility issues")]
    public bool NoAutoPickupModded = false;
}