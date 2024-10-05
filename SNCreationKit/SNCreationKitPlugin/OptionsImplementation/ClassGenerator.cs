using System;
using SNCreationKitData.OptionGeneration;
using SNCreationKitData.OptionGeneration.Attributes;
using SNCreationKitData.OptionGeneration.OptionTypeInterfaces;
using UnityEngine;

namespace SNCreationKitPlugin.OptionsImplementation;

public class ClassGenerator : IClassGenerator<RectTransform>
{
    public void GenerateUserInterface(IUserInterfaceManager<RectTransform> ui)
    {
        ui.CreateHeader(DataAttribute.Name);
    }

    public Type OptionType { get; set; }
    public SubClassAttribute DataAttribute { get; set; }
}