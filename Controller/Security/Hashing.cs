using BolomorzKeyManager.Model;
using System.Security.Cryptography;
using System.Text;

namespace BolomorzKeyManager.Controller.Security;

internal static class Hashing
{
    internal static UserAccount GenerateUserData(string username, string password)
    {
        var param = HashParameter.Generate();
        var hash = CalculateHash(param, password, username);

        return new()
        {
            Username = username,
            PasswordHash = hash,
            HashParameter = param.ToString()
        };
    }

    internal static bool Authenticate(UserAccount account, string password)
    {
        if (account.HashParameter is null || account.Username is null || account.PasswordHash is null)
            return false;
        if (password is null)
            return false;

        var param = HashParameter.FromString(account.HashParameter);
        if (param is null)
            return false;

        var hash = CalculateHash(param, password, account.Username);

        var auth = Compare(hash, account.PasswordHash);

        return auth;
    }

    private static string CalculateHash(HashParameter param, string plain, string username)
    {
        byte[] salt = Encoding.UTF8.GetBytes(param.ToString() + username);

        var rfc = new Rfc2898DeriveBytes(plain, salt, param.Iterations, HashAlgorithmName.SHA512);
        var key = rfc.GetBytes(param.Length);

        return Convert.ToBase64String(key);
    }

    private static bool Compare(string hash1, string hash2)
    {
        if(hash1 is null || hash2 is null) return false;

        int min = Math.Min(hash1.Length, hash2.Length);
        int result = 0;

        for(int i = 0; i < min; i++) result |= hash1[i] ^ hash2[i];

        return result == 0;
    }
}