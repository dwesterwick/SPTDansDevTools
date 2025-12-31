using System.Runtime.Serialization;

namespace DansDevTools.Configuration
{
    [DataContract]
    public class ModConfig
    {
        [DataMember(Name = "enabled")]
        public bool Enabled { get; set; } = false;

        [DataMember(Name = "scav_cooldown_time")]
        public int ScavCooldownTime { get; set; } = 1500;

        [DataMember(Name = "free_labs_access")]
        public bool FreeLabsAccess { get; set; } = false;

        [DataMember(Name = "free_labyrinth_access")]
        public bool FreeLabyrinthAccess { get; set; } = false;

        [DataMember(Name = "min_level_for_flea")]
        public int MinLevelForFlea { get; set; } = 15;

        [DataMember(Name = "full_length_scav_raids")]
        public bool FullLengthScavRaids { get; set; } = false;

        [DataMember(Name = "always_have_airdrops")]
        public bool AlwaysHaveAirdrops { get; set; } = false;

        [DataMember(Name = "bosses_always_spawn")]
        public bool BossesAlwaysSpawn { get; set; } = false;
    }
}
