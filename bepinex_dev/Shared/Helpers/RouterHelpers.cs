using System;
using System.Collections.Generic;
using System.Text;

namespace DansDevTools.Helpers
{
    public static class RouterHelpers
    {
        public static string GetRoutePath(string routeName)
        {
            return "/" + ModInfo.MODNAME + "/" + routeName;
        }
    }
}
