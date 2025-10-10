using BolomorzKeyManager.Controller.Data;

namespace BolomorzKeyManager.Controller.Auth;

enum MasterMode { Revealed, Concealed }
internal class Session
{
    internal AuthUser? AuthUser { get; private set; }
    internal KeyManagerContextOperations? Operations { get; private set; }

    private string? MasterPassword;
    internal MasterMode MasterMode = MasterMode.Concealed;

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

    internal ReturnDialog<byte[]> Encrypt(string data, string? master)
    {
        if (master is null && MasterPassword is not null)
        {
            var encrypted = Encryption.KeyEncryption.EncryptData(data, MasterPassword);
            return new(Message.Successful, encrypted);
        }
        else if (master is not null)
        {
            var encrypted = Encryption.KeyEncryption.EncryptData(data, master);
            return new(Message.Successful, encrypted);
        }
        else
        {
            return new(Message.MasterPassword, null);
        }
    }
    internal ReturnDialog<string> Decrypt(byte[] data, string? master)
    {
        if (master is null && MasterPassword is not null)
        {
            var decrypted = Encryption.KeyEncryption.DecryptData(data, MasterPassword);
            return new(Message.Successful, decrypted);
        }
        else if (master is not null)
        {
            var decrypted = Encryption.KeyEncryption.DecryptData(data, master);
            return new(Message.Successful, decrypted);
        }
        else
        {
            return new(Message.MasterPassword, null);
        }
    }

    internal void Reveal(string master)
    {
        MasterPassword = master;
        MasterMode = MasterMode.Revealed;
    }
    internal void Conceal()
    {
        MasterPassword = null;
        MasterMode = MasterMode.Concealed;
    }
    internal void Close()
    {
        AuthUser = null;
        Operations = null;
        Conceal();
    }

    internal static async Task<Session> OpenSession(string user, string password)
    {
        var session = new Session();
        await session.Authenticate(user, password);
        return session;
    }
}