using SMLHelper.Json;
using SMLHelper.Options;
using SMLHelper.Options.Attributes;
using System;
using System.Collections.Generic;

namespace InventoryColorCustomization
{
    // worst class i've written since the pre-ECC gargantuan leviathan
    public class Options : ModOptions
    {
        private SaveOptions savedOptions;

        public Options() : base("Inventory Color Customization")
        {
            OnChanged += OnAnyOptionChanged;

            savedOptions = new SaveOptions();
            savedOptions.Load(true);
            EnsureSettingsAreValid();
        }

        public void InitOptionItems()
        {
            AddBackgroundColorOption(new BackgroundType(CraftData.BackgroundType.Normal), "Normal Item Color");
            AddBackgroundColorOption(new BackgroundType(CraftData.BackgroundType.ExosuitArm), "Exosuit Arms Color");
            AddBackgroundColorOption(new BackgroundType(CraftData.BackgroundType.PlantWater), "Water Flora Color");
            AddBackgroundColorOption(new BackgroundType(CraftData.BackgroundType.PlantAir), "Air Flora Color");
            AddBackgroundColorOption(new BackgroundType(BackgroundTypeManager.Category_Creatures), "Creatures Color");
            AddBackgroundColorOption(new BackgroundType(BackgroundTypeManager.Category_Precursor), "Precursor Items Color");
            AddBackgroundColorOption(new BackgroundType(BackgroundTypeManager.Category_Tools), "Tools Color");
            AddBackgroundColorOption(new BackgroundType(BackgroundTypeManager.Category_Deployables), "Deployables Colors");
            AddBackgroundColorOption(new BackgroundType(BackgroundTypeManager.Category_FoodDrinks), "Food & Drink Colors");
            AddBackgroundColorOption(new BackgroundType(CraftData.BackgroundType.PlantWaterSeed), "Water Seeds Color");
            AddBackgroundColorOption(new BackgroundType(CraftData.BackgroundType.PlantAirSeed), "Air Seeds Color");
            AddItem(ModToggleOption.Create("SquareIcons", "Use Square Icons", savedOptions.SquareIcons));

            // AddBackgroundColorOption(CraftData.BackgroundType.Blueprint, "Blueprint Color (Unused)");
        }

        private void EnsureSettingsAreValid()
        {
            if (ColorChoiceManager.ColorChoices == null || savedOptions == null || savedOptions.BackgroundColorChoices == null) return;
            int maxChoices = ColorChoiceManager.ColorChoices.Count;
            string[] keys = new string[savedOptions.BackgroundColorChoices.Count];
            savedOptions.BackgroundColorChoices.Keys.CopyTo(keys, 0);
            foreach (var key in keys)
            {
                if (savedOptions.BackgroundColorChoices[key] >= maxChoices)
                {
                    savedOptions.BackgroundColorChoices[key] = 0;
                }
            }
        }

        public override void BuildModOptions(uGUI_TabbedControlsPanel panel, int modsTabIndex, IReadOnlyCollection<OptionItem> options)
        {
            base.BuildModOptions(panel, modsTabIndex, options);
        }

        public void OnAnyOptionChanged(object sender, EventArgs e)
        {
            switch (e)
            {
                case ToggleChangedEventArgs args:
                    OnToggleChanged(args);
                    break;
                case ChoiceChangedEventArgs<string> args:
                    OnChoiceChanged(args);
                    break;
            }
        }

        private void OnToggleChanged(ToggleChangedEventArgs eventArgs)
        {
            bool refreshRequired = false;
            switch (eventArgs.Id)
            {
                case "SquareIcons":
                    savedOptions.SquareIcons = eventArgs.Value;
                    refreshRequired = true;
                    break;
            }
            if (refreshRequired)
            {
                ColorChoiceManager.RefreshAll();
                BackgroundDataManager.RefreshAll();
            }
            savedOptions.Save();
        }

        private void OnChoiceChanged(ChoiceChangedEventArgs<string> eventArgs)
        {
            var key = eventArgs.Id;
            int value = ColorChoiceManager.ChoiceNameToIndex(key, eventArgs.Value);
            if (savedOptions.BackgroundColorChoices == null)
            {
                savedOptions.BackgroundColorChoices = new Dictionary<string, int>();
            }
            var dict = savedOptions.BackgroundColorChoices;
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
            savedOptions.Save();
        }

        private void AddBackgroundColorOption(BackgroundType backgroundType, string label)
        {
            var data = backgroundType.GetData();
            if (data == null)
            {
                Main.logger.LogError($"BackgroundData '{label}' for '{backgroundType}' is null!");
                return;
            }
            string id = data.ID;
            string[] choices = ColorChoiceManager.GetColorChoiceNames(backgroundType);
            AddItem(ModChoiceOption<string>.Create(id, label, choices, savedOptions.GetBackgroundColorChoice(BackgroundDataManager.GetBackgroundData(backgroundType).ID)));
        }

        public int GetSelectedIndexForBackground(string backgroundType)
        {
            return savedOptions.GetBackgroundColorChoice(backgroundType);
        }

        public bool SquareIcons { get { return savedOptions.SquareIcons; } }

    }
}