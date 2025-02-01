import { CommonUtils } from "./CommonUtils";
import modConfig from "../config/config.json";

import type { DependencyContainer } from "tsyringe";
import type { IPreSptLoadMod } from "@spt/models/external/IPreSptLoadMod";
import type { IPostDBLoadMod } from "@spt/models/external/IPostDBLoadMod";
import type { IPostSptLoadMod } from "@spt/models/external/IPostSptLoadMod";

import type { ILogger } from "@spt/models/spt/utils/ILogger";
import type { DatabaseServer } from "@spt/servers/DatabaseServer";
import type { IDatabaseTables } from "@spt/models/spt/server/IDatabaseTables";
import type { ConfigServer } from "@spt/servers/ConfigServer";
import type { LocaleService } from "@spt/services/LocaleService";
import type { ProfileHelper } from "@spt/helpers/ProfileHelper";

import { ConfigTypes } from "@spt/models/enums/ConfigTypes";
import type { ILocationConfig } from "@spt/models/spt/config/ILocationConfig";

import type { StaticRouterModService } from "@spt/services/mod/staticRouter/StaticRouterModService";
import type { DynamicRouterModService } from "@spt/services/mod/dynamicRouter/DynamicRouterModService";

const modName = "DansDevTools";
const invalidMaps : string[] = ["base", "develop"];

class DansDevTools implements IPreSptLoadMod, IPostSptLoadMod, IPostDBLoadMod
{
    private commonUtils: CommonUtils

    private logger: ILogger;
    private configServer: ConfigServer;
    private databaseServer: DatabaseServer;
    private databaseTables: IDatabaseTables;
    private localeService: LocaleService;
    private profileHelper: ProfileHelper;

    private iLocationConfig: ILocationConfig;
    
    public preSptLoad(container: DependencyContainer): void 
    {
        this.logger = container.resolve<ILogger>("WinstonLogger");
        const staticRouterModService = container.resolve<StaticRouterModService>("StaticRouterModService");
        const dynamicRouterModService = container.resolve<DynamicRouterModService>("DynamicRouterModService");
        
        if (!modConfig.enabled)
        {
            return;
        }

        // Game start
        staticRouterModService.registerStaticRouter(`StaticAkiGameStart${modName}`,
            [{
                url: "/client/game/start",
                // biome-ignore lint/suspicious/noExplicitAny: <explanation>
                action: async (url: string, info: any, sessionId: string, output: string) => 
                {
                    this.updateScavTimer(sessionId);

                    return output;
                }
            }], "aki"
        );

        // Can be used by other mods to check if the Questing Bots spawning system is active
        dynamicRouterModService.registerDynamicRouter(`DynamicQuestingBotsSpawnSystemCheck${modName}`,
            [{
                url: "/QuestingBots/AdjustPMCConversionChances/",
                action: async (output: string) => 
                {
                    this.commonUtils.logWarning("Questing Bots spawning system is active");
                    return output;
                }
            }], "QuestingBotsSpawnSystemCheck"
        );
    }
	
    public postDBLoad(container: DependencyContainer): void
    {
        this.databaseServer = container.resolve<DatabaseServer>("DatabaseServer");
        this.configServer = container.resolve<ConfigServer>("ConfigServer");
        this.localeService = container.resolve<LocaleService>("LocaleService");
        this.profileHelper = container.resolve<ProfileHelper>("ProfileHelper");
		
        this.databaseTables = this.databaseServer.getTables();
        this.iLocationConfig = this.configServer.getConfig(ConfigTypes.LOCATION);
		
        this.commonUtils = new CommonUtils(this.logger, this.databaseTables, this.localeService);
		
        if (!modConfig.enabled)
        {
            this.commonUtils.logInfo("Mod disabled in config.json", true);
            return;
        }

        if (modConfig.free_labs_access)
        {
            this.databaseTables.locations.laboratory.base.AccessKeys = [];
            this.databaseTables.locations.laboratory.base.DisabledForScav = false;
        }

        this.databaseTables.globals.config.RagFair.minUserLevel = modConfig.min_level_for_flea;

        if (modConfig.scav_cooldown_time < this.databaseTables.globals.config.SavagePlayCooldown)
        {
            this.databaseTables.globals.config.SavagePlayCooldown = modConfig.scav_cooldown_time;
        }

        if (modConfig.full_length_scav_raids)
        {
            this.forceFullLengthScavRaids();
        }

        if (modConfig.always_have_airdrops)
        {
            this.commonUtils.logInfo("Forcing airdrops to occur at the beginning of every raid...");

            for (const map in this.databaseTables.locations)
            {
                if (invalidMaps.includes(map))
                {
                    continue;
                }

                if (this.databaseTables.locations[map].base.AirdropParameters === undefined)
                {
                    continue;
                }
                
                this.databaseTables.locations[map].base.AirdropParameters[0].MinPlayersCountToSpawnAirdrop = 1;
                this.databaseTables.locations[map].base.AirdropParameters[0].PlaneAirdropChance = 1;
                this.databaseTables.locations[map].base.AirdropParameters[0].PlaneAirdropStartMin = 5;
                this.databaseTables.locations[map].base.AirdropParameters[0].PlaneAirdropStartMax = 10;
            }
        }

        if (modConfig.bosses_always_spawn)
        {
            this.makeBossesAlwaysSpawn();
        }

        if (modConfig.friendly_pmcs)
        {
            this.commonUtils.logWarning("Friendly PMC's is not working!");
        }
    }
	
    public postSptLoad(): void
    {
        if (!modConfig.enabled)
        {
            return;
        }
    }

    private updateScavTimer(sessionId: string): void
    {
        const pmcData = this.profileHelper.getPmcProfile(sessionId);
        const scavData = this.profileHelper.getScavProfile(sessionId);
		
        if ((scavData.Info === null) || (scavData.Info === undefined))
        {
            this.commonUtils.logInfo("Scav profile hasn't been created yet.");
            return;
        }
		
        // In case somebody disables scav runs and later wants to enable them, we need to reset their Scav timer unless it's plausible
        const worstCooldownFactor = this.getWorstSavageCooldownModifier();
        if (scavData.Info.SavageLockTime - pmcData.Info.LastTimePlayedAsSavage > this.databaseTables.globals.config.SavagePlayCooldown * worstCooldownFactor * 1.1)
        {
            this.commonUtils.logInfo(`Resetting scav timer for sessionId=${sessionId}...`);
            scavData.Info.SavageLockTime = 0;
        }
    }
	
    // Return the highest Scav cooldown factor from Fence's rep levels
    private getWorstSavageCooldownModifier(): number
    {
        // Initialize the return value at something very low
        let worstCooldownFactor = 0.01;

        for (const level in this.databaseTables.globals.config.FenceSettings.Levels)
        {
            if (this.databaseTables.globals.config.FenceSettings.Levels[level].SavageCooldownModifier > worstCooldownFactor)
                worstCooldownFactor = this.databaseTables.globals.config.FenceSettings.Levels[level].SavageCooldownModifier;
        }
        return worstCooldownFactor;
    }

    private forceFullLengthScavRaids(): void
    {
        this.commonUtils.logInfo("Forcing full-length Scav raids...");

        for (const map in this.iLocationConfig.scavRaidTimeSettings.maps)
        {
            this.iLocationConfig.scavRaidTimeSettings.maps[map].reducedChancePercent = 0;
        }
    }

    private makeBossesAlwaysSpawn(): void
    {
        this.commonUtils.logInfo("Making bosses always spawn...");

        for (const map in this.databaseTables.locations)
        {
            if (invalidMaps.includes(map))
            {
                continue;
            }

            if (this.databaseTables.locations[map].base.BossLocationSpawn === undefined)
            {
                continue;
            }

            for (const bossLocationSpawn in this.databaseTables.locations[map].base.BossLocationSpawn)
            {
                this.databaseTables.locations[map].base.BossLocationSpawn[bossLocationSpawn].BossChance = 100;
            }
        }
    }
}

module.exports = { mod: new DansDevTools() }