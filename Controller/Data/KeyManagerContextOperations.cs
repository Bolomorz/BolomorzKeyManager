using BolomorzKeyManager.Controller.Security;
using BolomorzKeyManager.Controller.Auth;
using BolomorzKeyManager.Model;
using Microsoft.EntityFrameworkCore;

namespace BolomorzKeyManager.Controller.Data;

internal class KeyManagerContextOperations(Session session)
{
    private readonly Session Session = session;

    #region Authentication & Account
    internal static async Task<ReturnDialog<UserAccount>> Authenticate(string username, string password)
    {

        using var kmc = new KeyManagerContext();

        try
        {
            if (kmc.UserAccounts is null)
                return new(Message.FailedToCreateDatabase, null);

            var user = await kmc.UserAccounts
            .Include(x => x.Keys)
            .Include(x => x.Passwords)
            .FirstOrDefaultAsync(x => x.Username == username);

            if (user is null)
                return new(Message.WrongCredentials, null);

            var auth = Hashing.Authenticate(user, password);

            return auth ? new(Message.Successful, user) : new(Message.WrongCredentials, null);
        }
        catch (Exception ex)
        {
            return new(Message.ErrorThrown(ex.ToString(), "Authenticate"), null);
        }

    }
    internal static async Task<ReturnDialog> CreateUser(string username, string plainpassword)
    {
        using var kmc = new KeyManagerContext();
        using var transaction = await kmc.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead);

        try
        {
            if (kmc.UserAccounts is null)
            {
                await transaction.RollbackAsync();
                return new(Message.FailedToCreateDatabase);
            }

            var dupe = await kmc.UserAccounts.FirstOrDefaultAsync(x => x.Username == username);
            if (dupe is not null)
            {
                await transaction.RollbackAsync();
                return new(Message.Duplicate("username", username));
            }

            var data = Hashing.GenerateUserData(username, plainpassword);

            await kmc.UserAccounts.AddAsync(data);
            await kmc.SaveChangesAsync();
            await kmc.SaveChangesAsync();

            await transaction.CommitAsync();

            return new(Message.Successful);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new(Message.ErrorThrown(ex.ToString(), "CreateUser"));
        }
    }
    internal async Task<ReturnDialog> SetAccountData(string username, string plainpassword)
    {

        using var kmc = new KeyManagerContext();
        using var transaction = await kmc.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead);

        try
        {
            if (kmc.UserAccounts is null)
            {
                await transaction.RollbackAsync();
                return new(Message.FailedToCreateDatabase);
            }

            if (Session.AuthUser is null || !Session.AuthUser.Authenticated)
            {
                await transaction.RollbackAsync();
                return new(Message.UnauthorizedAccess);
            }

            var dupe = await kmc.UserAccounts.FirstOrDefaultAsync(x => x.Username == username && x.UAID != Session.AuthUser.UserAccount.UAID);
            if (dupe is not null)
            {
                await transaction.RollbackAsync();
                return new(Message.Duplicate("username", username));
            }

            var user = await kmc.UserAccounts.FirstOrDefaultAsync(x => x.UAID == Session.AuthUser.UserAccount.UAID);

            if (user is not null)
            {
                var newdata = Hashing.GenerateUserData(username, plainpassword);
                user.Username = newdata.Username;
                user.PasswordHash = newdata.PasswordHash;
                user.HashParameter = newdata.HashParameter;
            }

            await kmc.SaveChangesAsync();

            await transaction.CommitAsync();

            return user is not null ? new(Message.Successful) : new(Message.NotFound("user", "uaid", Session.AuthUser.UserAccount.UAID.ToString()));
        }
        catch (Exception ex)
        {
            return new(Message.ErrorThrown(ex.ToString(), "SetAccountData"));
        }

    }
    #endregion

    #region Key
    internal async Task<ReturnDialog<List<Key>>> GetKeys(Query query)
    {

        using var kcm = new KeyManagerContext();

        try
        {
            if (kcm.Keys is null)
                return new(Message.FailedToCreateDatabase, null);

            if (Session.AuthUser is null || !Session.AuthUser.Authenticated)
                return new(Message.UnauthorizedAccess, null);

            var keys = await kcm.Keys.Where(x => x.UAID == Session.AuthUser.UserAccount.UAID).ToListAsync();

            var result = new List<Key>();
            foreach (var key in keys)
                if (query.Result(key))
                    result.Add(key);

            return new(Message.Successful, result);
        }
        catch (Exception ex)
        {
            return new(Message.ErrorThrown(ex.ToString(), "GetKeys"), null);
        }

    }
    internal async Task<ReturnDialog<Key>> GetKey(int kid)
    {

        using var kcm = new KeyManagerContext();

        try
        {
            if (kcm.Keys is null)
                return new(Message.FailedToCreateDatabase, null);

            if (Session.AuthUser is null || !Session.AuthUser.Authenticated)
                return new(Message.UnauthorizedAccess, null);

            var key = await kcm.Keys.FirstOrDefaultAsync(x => x.UAID == Session.AuthUser.UserAccount.UAID && x.KID == kid);

            return key is not null ? new(Message.Successful, key) : new(Message.NotFound("key", "kid", kid.ToString()), null);
        }
        catch (Exception ex)
        {
            return new(Message.ErrorThrown(ex.ToString(), "GetKey"), null);
        }

    }
    internal async Task<ReturnDialog> SaveKey(string name, string description, byte[] data, int? kid)
    {
        using var kmc = new KeyManagerContext();
        using var transaction = await kmc.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead);

        try
        {
            if (kmc.Keys is null)
            {
                await transaction.RollbackAsync();
                return new(Message.FailedToCreateDatabase);
            }

            if (Session.AuthUser is null || !Session.AuthUser.Authenticated)
            {
                await transaction.RollbackAsync();
                return new(Message.UnauthorizedAccess);
            }

            var key = await kmc.Keys.FirstOrDefaultAsync(x => x.KID == kid);

            if (kid is null || key is null)
            {
                await kmc.Keys.AddAsync(new()
                {
                    Name = name,
                    Description = description,
                    EncryptedData = data,
                    UAID = Session.AuthUser.UserAccount.UAID
                });
            }
            else
            {
                if (key.UAID == Session.AuthUser.UserAccount.UAID)
                {
                    key.Name = name;
                    key.Description = description;
                    key.EncryptedData = data;
                }
                else
                {
                    await transaction.RollbackAsync();
                    return new(Message.UnauthorizedAccess);
                }
            }

            await kmc.SaveChangesAsync();

            await transaction.CommitAsync();

            return new(Message.Successful);
        }
        catch (Exception ex)
        {
            return new(Message.ErrorThrown(ex.ToString(), "SaveKey"));
        }

    }
    internal async Task<ReturnDialog> DeleteKey(int kid)
    {
        using var kmc = new KeyManagerContext();
        using var transaction = await kmc.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead);

        try
        {
            if (kmc.Keys is null)
            {
                await transaction.RollbackAsync();
                return new(Message.FailedToCreateDatabase);
            }

            if (Session.AuthUser is null || !Session.AuthUser.Authenticated)
            {
                await transaction.RollbackAsync();
                return new(Message.UnauthorizedAccess);
            }

            var key = await kmc.Keys.FirstOrDefaultAsync(x => x.KID == kid);

            if (key is not null && key.UAID == Session.AuthUser.UserAccount.UAID)
            {
                kmc.Keys.Remove(key);
            }
            else
            {
                await transaction.RollbackAsync();
                return new(Message.UnauthorizedAccess);
            }

            await kmc.SaveChangesAsync();

            await transaction.CommitAsync();

            return new(Message.Successful);
        }
        catch (Exception ex)
        {
            return new(Message.ErrorThrown(ex.ToString(), "DeleteKey"));
        }

    }
    #endregion

    #region Password
    internal async Task<ReturnDialog<List<Password>>> GetPasswords(Query query)
    {

        using var kcm = new KeyManagerContext();

        try
        {
            if (kcm.Passwords is null)
                return new(Message.FailedToCreateDatabase, null);

            if (Session.AuthUser is null || !Session.AuthUser.Authenticated)
                return new(Message.UnauthorizedAccess, null);

            var passwords = await kcm.Passwords.Where(x => x.UAID == Session.AuthUser.UserAccount.UAID).ToListAsync();

            var result = new List<Password>();
            foreach (var password in passwords)
                if (query.Result(password))
                    result.Add(password);

            return new(Message.Successful, result);
        }
        catch (Exception ex)
        {
            return new(Message.ErrorThrown(ex.ToString(), "GetPasswords"), null);
        }

    }
    internal async Task<ReturnDialog<Password>> GetPassword(int pid)
    {

        using var kcm = new KeyManagerContext();

        try
        {
            if (kcm.Passwords is null)
                return new(Message.FailedToCreateDatabase, null);

            if (Session.AuthUser is null || !Session.AuthUser.Authenticated)
                return new(Message.UnauthorizedAccess, null);

            var password = await kcm.Passwords.FirstOrDefaultAsync(x => x.UAID == Session.AuthUser.UserAccount.UAID && x.PID == pid);

            return password is not null ? new(Message.Successful, password) : new(Message.NotFound("password", "pid", pid.ToString()), null);
        }
        catch (Exception ex)
        {
            return new(Message.ErrorThrown(ex.ToString(), "GetPassword"), null);
        }

    }
    internal async Task<ReturnDialog> SavePassword(string name, string description, byte[] data, int? pid)
    {
        using var kmc = new KeyManagerContext();
        using var transaction = await kmc.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead);

        try
        {
            if (kmc.Passwords is null)
            {
                await transaction.RollbackAsync();
                return new(Message.FailedToCreateDatabase);
            }

            if (Session.AuthUser is null || !Session.AuthUser.Authenticated)
            {
                await transaction.RollbackAsync();
                return new(Message.UnauthorizedAccess);
            }

            var password = await kmc.Passwords.FirstOrDefaultAsync(x => x.PID == pid);

            if (pid is null || password is null)
            {
                await kmc.Passwords.AddAsync(new()
                {
                    Name = name,
                    Description = description,
                    EncryptedData = data,
                    UAID = Session.AuthUser.UserAccount.UAID
                });
            }
            else
            {
                if (password.UAID == Session.AuthUser.UserAccount.UAID)
                {
                    password.Name = name;
                    password.Description = description;
                    password.EncryptedData = data;
                }
                else
                {
                    await transaction.RollbackAsync();
                    return new(Message.UnauthorizedAccess);
                }
            }

            await kmc.SaveChangesAsync();

            await transaction.CommitAsync();

            return new(Message.Successful);
        }
        catch (Exception ex)
        {
            return new(Message.ErrorThrown(ex.ToString(), "SavePassword"));
        }

    }
    internal async Task<ReturnDialog> DeletePassword(int pid)
    {
        using var kmc = new KeyManagerContext();
        using var transaction = await kmc.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead);

        try
        {
            if (kmc.Passwords is null)
            {
                await transaction.RollbackAsync();
                return new(Message.FailedToCreateDatabase);
            }

            if (Session.AuthUser is null || !Session.AuthUser.Authenticated)
            {
                await transaction.RollbackAsync();
                return new(Message.UnauthorizedAccess);
            }

            var password = await kmc.Passwords.FirstOrDefaultAsync(x => x.PID == pid);

            if (password is not null && password.UAID == Session.AuthUser.UserAccount.UAID)
            {
                kmc.Passwords.Remove(password);
            }
            else
            {
                await transaction.RollbackAsync();
                return new(Message.UnauthorizedAccess);
            }

            await kmc.SaveChangesAsync();

            await transaction.CommitAsync();

            return new(Message.Successful);
        }
        catch (Exception ex)
        {
            return new(Message.ErrorThrown(ex.ToString(), "DeletePassword"));
        }

    }
    #endregion

}