using DansDevTools.Helpers;
using DansDevTools.Routers.Template;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Services;
using SPTarkov.Server.Core.Utils;

namespace DansDevTools.Routers
{
    [Injectable]
    public class ScavCooldownTimerRouter : AbstractStaticRouter<string?>
    {
        private static readonly string[] _routeNames = ["/client/game/start"];

        private DatabaseService _databaseService;
        private ProfileHelper _profileHelper;

        public ScavCooldownTimerRouter
        (
            LoggingUtil logger,
            ConfigUtil config,
            JsonUtil jsonUtil,
            DatabaseService databaseService,
            ProfileHelper profileHelper
        ) : base(_routeNames, logger, config, jsonUtil)
        {
            _databaseService = databaseService;
            _profileHelper = profileHelper;
        }

        protected override ValueTask<string?> HandleRoute(string routeName, RouterData routerData)
        {
            UpdateScavTimer(routerData.SessionId);

            return new ValueTask<string?>(routerData.Output);
        }

        private void UpdateScavTimer(MongoId sessionId)
        {
            PmcData? pmcData = _profileHelper.GetPmcProfile(sessionId);
            PmcData? scavData = _profileHelper.GetScavProfile(sessionId);

            if (pmcData?.Info == null || scavData?.Info == null)
            {
                Logger.Info("Cannot update Scav timer; Scav profile has not been created yet");
                return;
            }

            double? cooldownTimeRemaining = scavData.Info.SavageLockTime - pmcData.Info.LastTimePlayedAsSavage;

            double worstCaseCooldownModifier = GetWorstCaseCooldownModifier();
            double? maxCooldownTime = _databaseService.GetGlobals().Configuration.SavagePlayCooldown * worstCaseCooldownModifier * 1.1;

            if (cooldownTimeRemaining > maxCooldownTime)
            {
                Logger.Info($"Resetting Scav cooldown timer for {sessionId}");
                scavData.Info.SavageLockTime = 0;
            }
        }

        private double GetWorstCaseCooldownModifier()
        {
            double worstModifier = 0.01;

            Dictionary<double, FenceLevel> fenceLevels = _databaseService.GetGlobals().Configuration.FenceSettings.Levels;
            foreach (double level in fenceLevels.Keys)
            {
                if (fenceLevels[level].SavageCooldownModifier > worstModifier)
                {
                    worstModifier = fenceLevels[level].SavageCooldownModifier;
                }
            }

            return worstModifier;
        }
    }
}
