using DansDevTools.Helpers;
using DansDevTools.Services.Template;
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

        if (Config.CurrentConfig.FreeLabyrinthAccess)
        {
            RemoveLabyrinthRestrictions();
        }

        SetMinLevelForFlea(Config.CurrentConfig.MinLevelForFlea);
        ResetScavCooldownTime(Config.CurrentConfig.ScavCooldownTime);

        if (Config.CurrentConfig.AlwaysHaveAirdrops)
        {
            ForceAirdropsWhenRaidsStart();
        }

        if (Config.CurrentConfig.BossesAlwaysSpawn)
        {
            ForceBossesToAlwaysSpawn();
        }
    }

    private void RemoveLabsRestrictions()
    {
        Location labsLocation = _databaseService.GetAndVerifyLocation("laboratory");
        labsLocation.Base.AccessKeys = [];
        labsLocation.Base.DisabledForScav = false;

        Logger.Info("Removed restrictions for Labs");
    }

    private void RemoveLabyrinthRestrictions()
    {
        Location labsLocation = _databaseService.GetAndVerifyLocation("labyrinth");
        labsLocation.Base.Enabled = true;
        labsLocation.Base.AccessKeys = [];
        labsLocation.Base.DisabledForScav = false;

        Logger.Info("Removed restrictions for Labyrinth");
    }

    private void SetMinLevelForFlea(int level)
    {
        _databaseService.GetGlobals().Configuration.RagFair.MinUserLevel = level;
        Logger.Info($"Set min level for flea market to {level}");
    }

    private void ResetScavCooldownTime(int maxTime)
    {
        int currentCooldownTime = _databaseService.GetGlobals().Configuration.SavagePlayCooldown;
        if (currentCooldownTime > maxTime)
        {
            _databaseService.GetGlobals().Configuration.SavagePlayCooldown = maxTime;
            Logger.Info($"Reset Scav cooldown time to {maxTime}");
        }
    }

    private void ForceAirdropsWhenRaidsStart()
    {
        foreach (Location location in _databaseService.EnumerateLocations())
        {
            if ((location.Base?.AirdropParameters == null) || (location.Base.AirdropParameters.Count == 0))
            {
                continue;
            }

            AirdropParameter? firstAirdrop = location.Base?.AirdropParameters.First();
            if (firstAirdrop == null)
            {
                continue;
            }

            firstAirdrop.MinimumPlayersCountToSpawnAirdrop = 1;
            firstAirdrop.PlaneAirdropChance = 1;
            firstAirdrop.PlaneAirdropStartMin = 5;
            firstAirdrop.PlaneAirdropStartMax = 10;
        }

        Logger.Info("Forced airdrops to spawn at the start of every raid on all valid locations");
    }

    private void ForceBossesToAlwaysSpawn()
    {
        foreach (Location location in _databaseService.EnumerateLocations())
        {
            if ((location.Base == null) || (location.Base.BossLocationSpawn.Count == 0))
            {
                continue;
            }

            foreach (BossLocationSpawn bossSpawn in location.Base.BossLocationSpawn)
            {
                bossSpawn.BossChance = 100;
            }
        }

        Logger.Info("Forced bosses to always spawn on all locations");
    }
}