using DansDevTools.Helpers;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Utils;

namespace DansDevTools.Routers.Template;

public abstract class AbstractStaticRouter<TResult> : StaticRouter
{
    protected static LoggingUtil Logger { get; private set; } = null!;
    protected static ConfigUtil Config { get; private set; } = null!;

    private static readonly Dictionary<string, AbstractStaticRouter<TResult>> _registeredRoutes = new();

    private readonly Dictionary<string, RouteAction> _routeActions = new();

    public AbstractStaticRouter(IEnumerable<string> _routeNames, LoggingUtil logger, ConfigUtil config, JsonUtil jsonUtil) : base(jsonUtil, GetRoutes(_routeNames))
    {
        Logger = logger;
        Config = config;

        RegisterRoutes(_routeNames, this);
    }

    private static void RegisterRoutes(IEnumerable<string> routeNames, AbstractStaticRouter<TResult> instance)
    {
        foreach (string routeName in routeNames)
        {
            if (_registeredRoutes.ContainsKey(routeName))
            {
                throw new InvalidOperationException($"Route \"{routeNames}\" is already registered");
            }

            _registeredRoutes.Add(routeName, instance);
        }
    }

    private static IEnumerable<RouteAction> GetRoutes(IEnumerable<string> routeNames)
    {
        if (!Config.CurrentConfig.Enabled)
        {
            yield break;
        }

        foreach (string _routeName in routeNames)
        {
            if (!_registeredRoutes.TryGetValue(_routeName, out AbstractStaticRouter<TResult>? instance) || instance == null)
            {
                throw new InvalidOperationException($"Cannot retrieve route for \"{_routeName}\"");
            }

            yield return instance.GetRouteAction(_routeName);
        }
    }

    protected abstract ValueTask<TResult> HandleRoute(string routeName, RouterData routerData);

    private RouteAction GetRouteAction(string routeName)
    {
        if (_routeActions.TryGetValue(routeName, out RouteAction? routeAction))
        {
            if (routeAction == null)
            {
                throw new InvalidOperationException($"RouteAction for \"{routeName}\" is null");
            }

            return routeAction;
        }

        routeAction = CreateRouteAction(routeName);
        _routeActions.Add(routeName, routeAction);

        return routeAction;
    }

    private RouteAction CreateRouteAction(string routeName)
    {
        string routePath = RouterHelpers.GetRoutePath(routeName);
        Logger.Info("Creating route: " + routePath);

        RouteAction routeAction = new RouteAction(routePath, async (url, info, sessionId, output) =>
                    await HandleRoute(routeName, new RouterData(url, info, sessionId, output)) ?? throw new InvalidOperationException("HandleRoute returned null"));

        return routeAction;
    }
}