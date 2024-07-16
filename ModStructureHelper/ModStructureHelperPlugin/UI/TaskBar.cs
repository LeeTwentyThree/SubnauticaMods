﻿using System.Runtime.Remoting.Messaging;
using ModStructureHelperPlugin.StructureHandling;
using UnityEngine;

namespace ModStructureHelperPlugin.UI;

public class TaskBar : MonoBehaviour
{
    public StructureHelperUI ui;

    public void OnButtonClose() => StructureHelperUI.SetUIEnabled(false);
    public void OnButtonSave() => StructureInstance.TrySave();
    public void OnButtonHome() => ui.SetMenuActive(MenuType.Main);
    public void OnButtonPrintUnloaded() => StructureInstance.Main.PrintUnloadedObjects();
}