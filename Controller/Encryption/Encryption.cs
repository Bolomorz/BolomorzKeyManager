using System.Security.Cryptography;
using BolomorzKeyManager.Model;
using System.Text;

namespace BolomorzKeyManager.Controller.Security;

internal static class Encryption
{
    internal static byte[] EncryptData(string data, UserAccount account, string master)
    {
        return [];
    }

    private static byte[] DeriveKey(HashParameter param, string plain, string username)
    {
        byte[] salt = Encoding.UTF8.GetBytes(param.ToString() + username);

        var rfc = new Rfc2898DeriveBytes(plain, salt, param.Iterations, HashAlgorithmName.SHA512);
        return rfc.GetBytes(param.Length);
    }
    
}