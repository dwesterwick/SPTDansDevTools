using DansDevTools.Routers.Internal;
using DansDevTools.Utils;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Utils;

namespace DansDevTools.Routers
{
    [Injectable]
    public class ScavCooldownTimerRouter : AbstractStaticRouter
    {
        private static readonly string[] _routeNames = ["/client/game/start"];

        private ProfileUtil _profileUtil;
        
        public ScavCooldownTimerRouter(LoggingUtil logger, ConfigUtil config, JsonUtil jsonUtil, ProfileUtil profileUtil) : base(_routeNames, logger, config, jsonUtil)
        {
            _profileUtil = profileUtil;
        }

        public override ValueTask<string?> HandleRoute(string routeName, RequestData routerData)
        {
            CheckAndUpdateScavCooldownTime(routerData.SessionId);

            return new ValueTask<string?>(routerData.Output);
        }

        private void CheckAndUpdateScavCooldownTime(MongoId sessionId)
        {
            PmcData? pmcData = _profileUtil.GetPmcProfile(sessionId);
            PmcData? scavData = _profileUtil.GetScavProfile(sessionId);

            if ((pmcData?.Info == null) || (scavData?.Info == null))
            {
                Logger.Info("Cannot update Scav timer; Scav profile has not been created yet");
                return;
            }

            if (!IsCooldownTimeRemainingTooHigh(pmcData, scavData))
            {
                return;
            }

            double newCooldownTime = _profileUtil.GetMaxScavCooldownTime(pmcData);
            ChangeScavLockTime(pmcData, scavData, newCooldownTime);
        }

        private bool IsCooldownTimeRemainingTooHigh(PmcData pmcData, PmcData scavData)
        {
            double maxCooldownTime = _profileUtil.GetMaxScavCooldownTime(pmcData);
            double? cooldownTimeRemaining = _profileUtil.GetScavCooldownTimeRemaining(scavData);

            if (cooldownTimeRemaining < maxCooldownTime)
            {
                Logger.Info($"Current Scav cooldown time ({cooldownTimeRemaining} seconds) is below max cooldown time ({maxCooldownTime} seconds)");
                return false;
            }

            return true;
        }

        private void ChangeScavLockTime(PmcData pmcData, PmcData scavData, double newCooldownTime)
        {
            scavData.Info!.SavageLockTime = pmcData.Info!.LastTimePlayedAsSavage + newCooldownTime;

            double? cooldownTimeRemaining = _profileUtil.GetScavCooldownTimeRemaining(scavData);
            Logger.Info($"Reduced Scav cooldown timer to {newCooldownTime} seconds ({cooldownTimeRemaining} remaining)");
        }
    }
}
