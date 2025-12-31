using DansDevTools.Helpers;
using DansDevTools.Services.Internal;
using DansDevTools.Utils;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;

namespace DansDevTools.Services;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class MapRestrictionRemovalService : AbstractService
{
    private const string LABS_ID = "laboratory";
    private const string LABYRINTH_ID = "labyrinth";

    private DatabaseService _databaseService;
    private ConfigServer _configServer;
    private LocationConfig _locationConfig = null!;

    public MapRestrictionRemovalService(LoggingUtil logger, ConfigUtil config, DatabaseService databaseService, ConfigServer configServer) : base(logger, config)
    {
        _databaseService = databaseService;
        _configServer = configServer;
    }

    protected override void OnLoadIfModIsEnabled()
    {
        _locationConfig = _configServer.GetConfig<LocationConfig>();

        if (Config.CurrentConfig.FreeLabsAccess)
        {
            RemoveLabsRestrictions();
        }

        if (Config.CurrentConfig.FreeLabyrinthAccess)
        {
            RemoveLabyrinthRestrictions();
            AddLabyrinthToRaidTimeAdjustments();
        }
    }

    private void RemoveLabsRestrictions()
    {
        Location labsLocation = _databaseService.GetAndVerifyLocation(LABS_ID);
        labsLocation.Base.AccessKeys = [];
        labsLocation.Base.DisabledForScav = false;

        Logger.Info($"Removed restrictions for {LABS_ID}");
    }

    private void RemoveLabyrinthRestrictions()
    {
        Location labsLocation = _databaseService.GetAndVerifyLocation(LABYRINTH_ID);
        labsLocation.Base.Enabled = true;
        labsLocation.Base.AccessKeys = [];
        labsLocation.Base.DisabledForScav = false;

        Logger.Info($"Removed restrictions for {LABYRINTH_ID}");
    }

    private void AddLabyrinthToRaidTimeAdjustments()
    {
        ScavRaidTimeLocationSettings? labsSettings = _locationConfig.ScavRaidTimeSettings.Maps[LABS_ID];
        if (labsSettings == null)
        {
            throw new InvalidOperationException($"Could not retrieve scav raid-time adjustment settings for {LABS_ID}");
        }

        ScavRaidTimeLocationSettings labyrinthSettings = labsSettings.Clone();
        _locationConfig.ScavRaidTimeSettings.Maps.Add(LABYRINTH_ID, labyrinthSettings);

        Logger.Info($"Added Scav raid-time adjustment settings for {LABYRINTH_ID}");
    }
}
