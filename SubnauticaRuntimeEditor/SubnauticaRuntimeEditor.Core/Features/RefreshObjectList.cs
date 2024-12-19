using SubnauticaRuntimeEditor.Core.Utils.Abstractions;

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
