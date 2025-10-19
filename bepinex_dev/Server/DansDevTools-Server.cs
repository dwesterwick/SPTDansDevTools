using DansDevTools.Helpers;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;

namespace DansDevTools;

public record ModMetadata : AbstractModMetadata
{
    public override string ModGuid { get; init; } = "com.danw.dansdevtools";
    public override string Name { get; init; } = "DansDevTools";
    public override string Author { get; init; } = "DanW";
    public override List<string>? Contributors { get; init; }
    public override SemanticVersioning.Version Version { get; init; } = new("1.1.0");
    public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
    public override List<string>? Incompatibilities { get; init; }
    public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public override string? Url { get; init; }
    public override bool? IsBundleMod { get; init; } = false;
    public override string? License { get; init; } = "MIT";
}

[Injectable(TypePriority = OnLoadOrder.PreSptModLoader + 1)]
public class DansDevToolsServer(
    ISptLogger<DansDevToolsServer> logger)
    : IOnLoad
{
    public Task OnLoad()
    {
        LoggingUtil.Init(logger);
        LoggingUtil.LogInfo("test");

        return Task.CompletedTask;
    }
}
