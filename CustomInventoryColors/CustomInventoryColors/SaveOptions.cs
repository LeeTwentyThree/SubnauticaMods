using SMLHelper.V2.Json;
using System.Collections.Generic;

namespace InventoryColorCustomization
{
    internal class SaveOptions : ConfigFile
    {
        public bool SquareIcons = false;
        public Dictionary<string, int> BackgroundColorChoices = new Dictionary<string, int>(); // id - value

        public int GetBackgroundColorChoice(string backgroundTypeID)
        {
            if (BackgroundColorChoices.TryGetValue(backgroundTypeID, out int value))
            {
                return value;
            }
            return 0;
        }
    }
}
