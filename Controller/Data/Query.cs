using BolomorzKeyManager.Model;

namespace BolomorzKeyManager.Controller.Data;

internal class Query(string? query)
{
    private readonly string? Q = query;

    internal bool Result(Key key)
        => Q is null
        || (key.Name is not null && key.Name.Contains(Q, StringComparison.CurrentCultureIgnoreCase))
        || (key.Description is not null && key.Description.Contains(Q, StringComparison.CurrentCultureIgnoreCase));

    internal bool Result(Password password)
        => Q is null
        || (password.Name is not null && password.Name.Contains(Q, StringComparison.CurrentCultureIgnoreCase))
        || (password.Description is not null && password.Description.Contains(Q, StringComparison.CurrentCultureIgnoreCase));
}