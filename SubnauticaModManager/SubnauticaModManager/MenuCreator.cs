using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubnauticaModManager;

public static class MenuCreator
{
    public static void ShowMenu()
    {
        SubnauticaModManagerPlugin.Logger.Log(LogLevel.Info, "Showing menu!");
    }

    public static void HideMenu()
    {
        SubnauticaModManagerPlugin.Logger.Log(LogLevel.Info, "Hiding menu!");
    }
}