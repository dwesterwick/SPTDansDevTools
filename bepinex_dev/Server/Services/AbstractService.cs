using DansDevTools.Helpers;
using SPTarkov.Server.Core.DI;

namespace DansDevTools.Services;

public abstract class AbstractService : IOnLoad
{
    protected static LoggingUtil Logger { get; private set; } = null!;
    protected static ConfigUtil Config { get; private set; } = null!;

    private static bool _disabledMessageLogged = false;

    public AbstractService(LoggingUtil logger, ConfigUtil config)
    {
        Logger = logger;
        Config = config;
    }

    public Task OnLoad()
    {
        if (Config.CurrentConfig.Enabled)
        {
            OnLoadIfEnabled();
        }
        else
        {
            LogDisabledMessage();
        }

        return Task.CompletedTask;
    }

    protected abstract void OnLoadIfEnabled();

    private void LogDisabledMessage()
    {
        if (_disabledMessageLogged)
        {
            return;
        }

        Logger.Info(ModInfo.MODNAME + " is disabled.");

        _disabledMessageLogged = true;
    }
}
