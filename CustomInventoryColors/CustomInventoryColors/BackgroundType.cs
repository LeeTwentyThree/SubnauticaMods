using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryColorCustomization
{
    // alternative to CraftData.BackgroundType that allows custom ones to be defined outside of the enum
    internal struct BackgroundType
    {
        public CraftData.BackgroundType enumValue;
        public string stringValue;

        public Mode Type { get; private set; }

        public enum Mode
        {
            Enum,
            String
        }

        public BackgroundType(string stringValue)
        {
            this.stringValue = stringValue;

            enumValue = CraftData.BackgroundType.Normal;

            Type = Mode.String;
        }

        public BackgroundType(CraftData.BackgroundType enumValue)
        {
            stringValue = null;

            this.enumValue = enumValue;

            Type = Mode.Enum;
        }

        public bool UseEnum { get { return Type == Mode.Enum; } }

        public bool UseString { get { return Type == Mode.String; } }

        public bool VanillaBackground { get { return UseEnum && (int)enumValue <= 6; } }

        public BackgroundData GetData()
        {
            return BackgroundDataManager.GetBackgroundData(this);
        }

        public bool Equals(BackgroundType other)
        {
            if (Type == Mode.String && other.Type == Mode.String)
            {
                return stringValue.Equals(other.stringValue);
            }
            else if (Type == Mode.Enum && other.Type == Mode.Enum)
            {
                return enumValue == other.enumValue;
            }
            else
            {
                return false;
            }
        }
    }
}
