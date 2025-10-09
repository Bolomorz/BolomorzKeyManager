using System.Security.Cryptography;

namespace BolomorzKeyManager.Controller.Security;

internal class HashParameter
{
    internal required int Length { get; set; }
    internal required int Iterations { get; set; }
    internal required string Salt { get; set; }

    public override string ToString()
        => $"{Length}.{Iterations}.{Salt}";

    internal static HashParameter? FromString(string input)
    {
        var param = input.Split('.');
        if (param.Length != 3) return null;

        return new()
        {
            Length = Convert.ToInt32(param[0]),
            Iterations = Convert.ToInt32(param[1]),
            Salt = param[2]
        };
    }

    internal static HashParameter Generate()
    {
        var rand = RandomNumberGenerator.Create();
        var length = RandomNumberGenerator.GetInt32(32, 64);
        var iter = RandomNumberGenerator.GetInt32(10000, 50000);
        var salt = new byte[length];

        rand.GetBytes(salt);

        return new()
        {
            Length = length,
            Iterations = iter,
            Salt = Convert.ToBase64String(salt)
        };
    }
}