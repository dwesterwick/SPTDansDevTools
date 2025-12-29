using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;

namespace DansDevTools;

[Injectable(TypePriority = OnLoadOrder.PreSptModLoader + 1)]
public class DansDevTools_Server
{
    public DansDevTools_Server()
    {

    }

    public Task OnLoad()
    {
        return Task.CompletedTask;
    }
}
