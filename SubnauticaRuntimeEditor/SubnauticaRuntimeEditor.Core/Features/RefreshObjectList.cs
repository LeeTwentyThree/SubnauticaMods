using SubnauticaRuntimeEditor.Core.Utils.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubnauticaRuntimeEditor.Core
{
    public class RefreshObjectList : FeatureBase<RefreshObjectList>
    {
        public static RefreshObjectList main;

        protected override void Initialize(InitSettings initSettings)
        {
            DisplayName = "Auto refresh (slow!)";
            Enabled = false;
            main = this;
        }
    }
}
