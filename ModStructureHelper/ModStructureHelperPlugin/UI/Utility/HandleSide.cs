using System;

namespace ModStructureHelperPlugin.UI.Utility;

[Flags]
public enum HandleSide
{
    None = 0,
    Left = 1,
    Right = 2,
    Top = 4,
    TopLeft = 5,
    TopRight = 6,
    Bottom = 8,
    BottomLeft = 9,
    BottomRight = 10
}