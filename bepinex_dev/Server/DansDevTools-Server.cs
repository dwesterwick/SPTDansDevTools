using DansDevTools.Helpers;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;

namespace DansDevTools;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = ModInfo.GUID;
    public override string Name { get; init; } = ModInfo.MODNAME;
    public override string Author { get; init; } = ModInfo.AUTHOR;
    public override List<string>? Contributors { get; init; }
    public override SemanticVersioning.Version Version { get; init; } = new(ModInfo.MODVERSION);
    public override SemanticVersioning.Range SptVersion { get; init; } = new(ModInfo.SPTVERSIONCOMPATIBILITY);
    public override List<string>? Incompatibilities { get; init; }
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public override string? Url { get; init; }
    public override bool? IsBundleMod { get; init; } = false;
    public override string License { get; init; } = "MIT";
}

[Injectable(TypePriority = OnLoadOrder.PreSptModLoader + 1)]
public class DansDevToolsServer : IOnLoad
{
    private readonly LoggingUtil _logger;
    private readonly ConfigUtil _config;

    public DansDevToolsServer(LoggingUtil logger, ConfigUtil config)
    {
        _logger = logger;
        _config = config;
    }

    public Task OnLoad()
    {
        _logger.Info("test");

        _logger.Info($"Mod enabled: {_config.Config.Enabled}");

        return Task.CompletedTask;
    }
}
