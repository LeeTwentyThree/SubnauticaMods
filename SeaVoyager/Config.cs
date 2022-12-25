using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipMod
{
    [Menu("Sea Voyager")]
    public class Config : ConfigFile
    {
        [Slider("Max power", 100, 2000, DefaultValue = 1250)]
        public float MaxPower;
        [Slider("Power production scale", 0f, 2f, Step = 0.05f, DefaultValue = 1f)]
        public float PowerProductionScale;
        [Slider("Power depletion rate", 0f, 2f, Step = 0.05f, DefaultValue = 1f)]
        public float PowerDepletionRate;
        [Slider("Sound volume", 0f, 1f, Step = 0.02f, DefaultValue = 1f, Tooltip = "Separate to the Master sound setting.")]
        public float AudioVolume;
    }
}
