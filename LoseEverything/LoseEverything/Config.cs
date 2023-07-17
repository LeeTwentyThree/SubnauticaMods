using Nautilus.Json;
using Nautilus.Options.Attributes;

namespace LoseEverything;

[Menu("Lose Everything")]
internal class Config : ConfigFile
{
    [Toggle("Lose all items on death")]
    public bool LoseItemsOnDeath = true;
    [Toggle("Lose all equipment on death")]
    public bool LoseEquipmentOnDeath;
}