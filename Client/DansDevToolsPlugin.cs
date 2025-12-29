using BepInEx;
using Comfort.Common;
using DansDevTools.Helpers;
using DansDevTools.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DansDevTools
{
    [BepInPlugin(ModInfo.GUID, ModInfo.MODNAME, ModInfo.MODVERSION)]
    public class DansDevToolsPlugin : BaseUnityPlugin
    {
        protected void Awake()
        {
            Logger.LogInfo("Loading DansDevTools...");

            Singleton<LoggingUtil>.Create(new LoggingUtil(Logger));

            if (ConfigUtil.CurrentConfig.Enabled)
            {
                Singleton<LoggingUtil>.Instance.LogInfo("Loading DansDevTools...enabled");

                if (ConfigUtil.CurrentConfig.FreeLabyrinthAccess)
                {
                    new LabyrinthScavExfilPatch().Enable();
                    Singleton<LoggingUtil>.Instance.LogInfo("Loading DansDevTools...created Scav exfils for Labyrinth...");
                }
            }

            Singleton<LoggingUtil>.Instance.LogInfo("Loading DansDevTools...done.");
        }
    }
}
