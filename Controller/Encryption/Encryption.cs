using System.Security.Cryptography;
using System.Text;

namespace BolomorzKeyManager.Controller.Encryption;

internal static class KeyEncryption
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iter = 25000;
    internal static byte[] EncryptData(string data, string master)
    {
        var salt = GenerateIV();
        var key = DeriveKey(salt, master);
        var iv = GenerateIV();

        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;
            using (var ms = new MemoryStream())
            {
                ms.Write(salt, 0, SaltSize);
                ms.Write(iv, 0, SaltSize);
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(data);
                    }
                }
                return ms.ToArray();
            }
        }
    }

    internal static string? DecryptData(byte[] encrypted, string master)
    {
        var salt = new byte[SaltSize];
        var iv = new byte[SaltSize];
        var cipher = new byte[SaltSize];
        using (var ms = new MemoryStream(encrypted))
        {
            ms.Read(salt, 0, SaltSize);
            ms.Read(iv, 0, SaltSize);
            ms.Read(cipher, 0, SaltSize); //Saltzie works becaus encrypted.Length = 3*Saltsize (48)
        }

        var key = DeriveKey(salt, master);

        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;
            
            using (var ms = new MemoryStream(cipher))
            {
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs))
                    {
                        var data = sr.ReadToEnd();
                        return data;
                    }
                }
            }
        }

    }

    private static byte[] DeriveKey(byte[] salt, string master)
    {
        var rfc = new Rfc2898DeriveBytes(master, salt, Iter, HashAlgorithmName.SHA512);
        return rfc.GetBytes(KeySize);
    }
    
    private static byte[] GenerateIV()
    {
        var rand = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];

        rand.GetBytes(salt);

        return salt;
    }
}