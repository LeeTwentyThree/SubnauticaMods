namespace NoAutoPickUp2;

public static class Logic
{
    public static CraftTree.Type GetCraftTreeForGhostCrafter(GhostCrafter ghostCrafter)
    {
        return ghostCrafter.craftTree;
    }

    public static bool CraftTreeIsModded(CraftTree.Type type)
    {
        var integer = (int)type;
        return integer < 0 || integer > (int)CraftTree.Type.Rocket;
    }

    public static bool ShouldDisableAutoPickupForCraftTree(CraftTree.Type type)
    {
        if (CraftTreeIsModded(type))
        {
            return Plugin.ModConfig.NoAutoPickupModded;
        }
        switch (type)
        {
            case CraftTree.Type.Fabricator:
                return Plugin.ModConfig.NoAutoPickupFabricator;
            case CraftTree.Type.Workbench:
                return Plugin.ModConfig.NoAutoPickupModificationStation;
            default:
                return Plugin.ModConfig.NoAutoPickupOther;
        }
    }
}