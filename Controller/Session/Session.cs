using System.Threading.Tasks;
using BolomorzKeyManager.Controller.Data;

namespace BolomorzKeyManager.Controller.Auth;

internal class Session
{
    internal AuthUser? AuthUser { get; private set; }
    internal KeyManagerContextOperations? Operations { get; private set; }

    private async Task<ReturnDialog> Authenticate(string user, string password)
    {

        var rd = await KeyManagerContextOperations.Authenticate(user, password);
        if (rd.Message.Success && rd.ReturnValue is not null)
        {
            AuthUser = new(rd.ReturnValue, true);
            Operations = new(this);
        }

        return new(rd.Message);

    }

    internal static async Task<Session> OpenSession(string user, string password)
    {
        var session = new Session();
        await session.Authenticate(user, password);
        return session;
    }

    internal void Close()
    {
        AuthUser = null;
        Operations = null;
    }
    
}