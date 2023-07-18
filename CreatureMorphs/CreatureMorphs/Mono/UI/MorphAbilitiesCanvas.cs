using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatureMorphs.Mono.UI;
internal class MorphAbilitiesCanvas : MonoBehaviour
{
    public static MorphAbilitiesCanvas Main { get; private set; }

    public static MorphAbilitiesCanvas Show()
    {
        var canvas = Instantiate(Plugin.bundle.LoadAsset<GameObject>("MorphAbilitiesCanvas"));
    }
}
