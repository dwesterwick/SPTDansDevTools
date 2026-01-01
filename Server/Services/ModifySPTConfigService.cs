using DansDevTools.Services.Internal;
using DansDevTools.Utils;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Servers;

namespace DansDevTools.Services;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + DansDevTools_Server.LOAD_ORDER_OFFSET)]
public class ModifySPTConfigService : AbstractService
{
    private ConfigServer _configServer;
    private LocationConfig _locationConfig = null!;
    private WeatherConfig _weatherConfig = null!;

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

        if (Config.CurrentConfig.SeasonAlwaysSummer)
        {
            ForceSummer();
        }
    }

    private void GetAllConfigs()
    {
        _locationConfig = _configServer.GetConfig<LocationConfig>();
        _weatherConfig = _configServer.GetConfig<WeatherConfig>();
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

    private void ForceSummer()
    {
        _weatherConfig.OverrideSeason = SPTarkov.Server.Core.Models.Enums.Season.SUMMER;
        Logger.Info("Forcing summer season");
    }
}
