using DansDevTools.Services.Internal;
using DansDevTools.Utils;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Servers;

namespace DansDevTools.Services;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class ModifySPTConfigService : AbstractService
{
    private ConfigServer _configServer;
    private LocationConfig _locationConfig = null!;

    public ModifySPTConfigService(LoggingUtil logger, ConfigUtil config, ConfigServer configServer) : base(logger, config)
    {
        _configServer = configServer;
    }

    protected override void OnLoadIfModIsEnabled()
    {
        GetAllConfigs();

        if (Config.CurrentConfig.FullLengthScavRaids)
        {
            ForceFullLengthScavRaids();
        }
    }

    private void GetAllConfigs()
    {
        _locationConfig = _configServer.GetConfig<LocationConfig>();
    }

    private void ForceFullLengthScavRaids()
    {
        foreach (ScavRaidTimeLocationSettings? settings in _locationConfig.ScavRaidTimeSettings.Maps.Values)
        {
            if (settings == null)
            {
                continue;
            }

            settings.ReducedChancePercent = 0;
        }

        Logger.Info("Forced full-length Scav raids for all locations");
    }
}
