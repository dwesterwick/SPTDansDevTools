using BepInEx;
using DansDevTools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DansDevTools
{
    [BepInPlugin("com.danw.dansdevtools", "DansDevTools", "1.1.0")]
    public class DansDevToolsPlugin : BaseUnityPlugin
    {
        protected void Awake()
        {
            Logger.LogInfo("Loading DansDevTools...");

            LoggingUtil.Logger = Logger;

            Logger.LogInfo("Loading DansDevTools...done.");
        }
    }
}
