using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Options.Attributes;
using System.Collections.Generic;

namespace InventoryColorCustomization
{
    // worst class i've written since the pre-ECC gargantuan leviathan
    public class Options : ModOptions
    {
        private SaveOptions savedOptions;

        public Options() : base("Inventory Color Customization")
        {
            ToggleChanged += OnToggleChanged;
            ChoiceChanged += OnChoiceChanged;
            savedOptions = new SaveOptions();
            savedOptions.Load(true);
            EnsureSettingsAreValid();
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

        public override void BuildModOptions()
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
            AddToggleOption("SquareIcons", "Use Square Icons", savedOptions.SquareIcons);

            // AddBackgroundColorOption(CraftData.BackgroundType.Blueprint, "Blueprint Color (Unused)");
        }

        public void OnToggleChanged(object sender, ToggleChangedEventArgs eventArgs)
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

        public void OnChoiceChanged(object sender, ChoiceChangedEventArgs eventArgs)
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
            string id = backgroundType.GetData().ID;
            string[] choices = ColorChoiceManager.GetColorChoiceNames(backgroundType);
            AddChoiceOption(id, label, choices, savedOptions.GetBackgroundColorChoice(BackgroundDataManager.GetBackgroundData(backgroundType).ID));
        }

        public int GetSelectedIndexForBackground(string backgroundType)
        {
            return savedOptions.GetBackgroundColorChoice(backgroundType);
        }

        public bool SquareIcons { get { return savedOptions.SquareIcons; } }

    }
}