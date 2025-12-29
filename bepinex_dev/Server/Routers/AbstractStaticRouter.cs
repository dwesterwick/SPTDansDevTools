using DansDevTools.Helpers;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Utils;

namespace DansDevTools.Routers;

public abstract class AbstractStaticRouter<TResult> : StaticRouter
{
    protected static LoggingUtil Logger { get; private set; } = null!;
    protected static ConfigUtil Config { get; private set; } = null!;

    private static string _routePath = null!;
    private static RouteAction _routeAction = null!;
    private static AbstractStaticRouter<TResult> _instace = null!;

    public AbstractStaticRouter(string _routeName, LoggingUtil logger, ConfigUtil config, JsonUtil jsonUtil) : base(jsonUtil, GetCustomRoutes())
    {
        _instace = this;
        Logger = logger;
        Config = config;

        _routePath = RouterHelpers.GetRoutePath(_routeName);
    }

    private static IEnumerable<RouteAction> GetCustomRoutes()
    {
        if (!Config.CurrentConfig.Enabled)
        {
            yield break;
        }

        yield return GetRouteAction();
    }

    protected abstract ValueTask<TResult> HandleRoute(string url, IRequestData info, MongoId sessionId, string? output);

    private static RouteAction GetRouteAction()
    {
        if (_routeAction != null)
        {
            return _routeAction;
        }

        Logger.Info("Creating route: " + _routePath);

        _routeAction = new RouteAction(_routePath, async (url, info, sessionId, output) =>
                    await _instace.HandleRoute(url, info, sessionId, output) ?? throw new InvalidOperationException("HandleRoute returned null"));

        return _routeAction;
    }
}