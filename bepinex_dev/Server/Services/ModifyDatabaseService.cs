using DansDevTools.Helpers;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Services;

namespace DansDevTools.Services;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class ModifyDatabaseService : AbstractService
{
    private DatabaseService _databaseService;

    public ModifyDatabaseService(LoggingUtil logger, ConfigUtil config, DatabaseService databaseService) : base(logger, config)
    {
        _databaseService = databaseService;
    }

    protected override void OnLoadIfEnabled()
    {
        if (Config.CurrentConfig.FreeLabsAccess)
        {
            RemoveLabsRestrictions();
        }
    }

    private void RemoveLabsRestrictions()
    {
        Location labsLocation = _databaseService.GetAndVerifyLocation("laboratory");
        labsLocation.Base.AccessKeys = [];
        labsLocation.Base.DisabledForScav = false;

        Logger.Info("Removed restrictions for Labs");
    }
}