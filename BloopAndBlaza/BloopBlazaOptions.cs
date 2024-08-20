using System.Collections.Generic;
using Nautilus.Options;

namespace BloopAndBlaza;

public class BloopBlazaOptions : ModOptions
{
    public BloopBlazaOptions() : base("Bloop and Blaza")
    {
        AddItem(Plugin.registerBlazaSpawns.ToModToggleOption());
        AddItem(Plugin.registerShallowBloopSpawns.ToModToggleOption());
        AddItem(Plugin.registerDeepBloopSpawns.ToModToggleOption());
    }

    public override void BuildModOptions(uGUI_TabbedControlsPanel panel, int modsTabIndex, IReadOnlyCollection<OptionItem> options)
    {
        panel.AddHeading(modsTabIndex, Name + "\n" +
                                       "<size=60%><color=#FFFFFF>\u26a0 These options do not affect creatures that have already spawned." +
                                       " A game restart is required for changes to take place!</color></size>");
        
        options.ForEach(option => option.AddToPanel(panel, modsTabIndex));
        options.ForEach(option => ((ModToggleOption)option).OnChanged += NotifyChangeRecommended);
    }

    private void NotifyChangeRecommended(object value, ToggleChangedEventArgs args)
    {
        ErrorMessage.AddMessage("<color=#FF0000>Bloop and Blaza mod: You must restart the game in order for configuration changes to apply!</color>");
    }
}