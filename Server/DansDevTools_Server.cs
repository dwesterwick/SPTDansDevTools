using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;

namespace DansDevTools;

[Injectable(TypePriority = OnLoadOrder.PreSptModLoader + DansDevTools_Server.LOAD_ORDER_OFFSET)]
public class DansDevTools_Server
{
    public const int LOAD_ORDER_OFFSET = 1;

    public DansDevTools_Server()
    {

    }

    public Task OnLoad()
    {
        return Task.CompletedTask;
    }
}
