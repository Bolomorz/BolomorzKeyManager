using BolomorzKeyManager.Model;

namespace BolomorzKeyManager.Controller.Auth;

internal class AuthUser(UserAccount account, bool authenticated)
{
    internal UserAccount UserAccount { get; init; } = account;
    internal bool Authenticated { get; init; } = authenticated;
}