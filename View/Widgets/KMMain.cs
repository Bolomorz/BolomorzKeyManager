using System.Threading.Tasks;
using BolomorzKeyManager.Controller.Data;
using BolomorzKeyManager.Model;
using Gtk;

namespace BolomorzKeyManager.View.Widgets;

internal class KMMain : KMGrid
{
    private KMGrid View;

    internal KMMain(KeyManager app) : base(app)
    {
        RowSpacing = 10;
        RowHomogeneous = false;
        ColumnSpacing = 10;
        ColumnHomogeneous = true;
        Hexpand = true;
        Vexpand = true;
        Margin = 10;

        var menu = new KMMenu(this);

        View = new KMWelcomeView(App, this);

        Attach(menu, 0, 0, 1, 1);
        Attach(View, 0, 1, 1, 1);
    }

    #region App Menu
    internal void OnLogout(object? sender, EventArgs e)
    {
        App._Session?.Close();
        App._Session = null;
        App.Window.ShowLogin();
    }

    internal void OnAccount(object? sender, EventArgs e)
    {
        Remove(View);
        View.Destroy();
        View = new KMAccountView(App, this);
        Attach(View, 0, 1, 1, 1);
        App.Window.ShowAll();
    }

    internal void OnClose(object? sender, EventArgs e)
    {
        App._Session?.Close();
        App._Session = null;
        App.Window.Close();
    }
    #endregion

    #region Key Menu
    internal void OnShowAll(object? sender, EventArgs e)
    {
        Remove(View);
        View.Destroy();
        View = new KMShowView(App, this, ShowMode.All);
        Attach(View, 0, 1, 1, 1);
        App.Window.ShowAll();
    }
    internal void OnShowKeys(object? sender, EventArgs e)
    {
        Remove(View);
        View.Destroy();
        View = new KMShowView(App, this, ShowMode.Key);
        Attach(View, 0, 1, 1, 1);
        App.Window.ShowAll();
    }
    internal void OnShowPasswords(object? sender, EventArgs e)
    {
        Remove(View);
        View.Destroy();
        View = new KMShowView(App, this, ShowMode.Pwd);
        Attach(View, 0, 1, 1, 1);
        App.Window.ShowAll();
    }
    internal void OnNewKey(object? sender, EventArgs e)
    {
        Remove(View);
        View.Destroy();
        View = new KMNewView(App, this, NewMode.Key);
        Attach(View, 0, 1, 1, 1);
        App.Window.ShowAll();
    }
    internal void OnNewPassword(object? sender, EventArgs e)
    {
        Remove(View);
        View.Destroy();
        View = new KMNewView(App, this, NewMode.Pwd);
        Attach(View, 0, 1, 1, 1);
        App.Window.ShowAll();
    }
    #endregion

    #region Info Menu

    #endregion

    #region Navigate Views
    internal void OnStart(object? sender, EventArgs e)
    {
        Remove(View);
        View.Destroy();
        View = new KMWelcomeView(App, this);
        Attach(View, 0, 1, 1, 1);
        App.Window.ShowAll();
    }
    internal void OnReveal(Model.Key key)
    {

        if (App._Session is null || key.EncryptedData is null) return;

        var master = App._Session.MasterMode == Controller.Auth.MasterMode.Revealed ? null : Dialog.InputDialog("insert master password", App);
        var rdd = App._Session.Decrypt(key.EncryptedData, master);

        if (rdd is not null && rdd.Message.Success && rdd.ReturnValue is not null)
        {
            var decrypt = rdd.ReturnValue;
            Dialog.OutputDialog(key.Name ?? "", decrypt, App);
        }
        else
        {
            Dialog.ErrorDialog(rdd is not null ? rdd.Message : Message.FailedToCreateReturn, App.Window);
        }
        
    }
    internal void OnReveal(Password pwd)
    {

        if (App._Session is null || pwd.EncryptedData is null) return;

        var master = App._Session.MasterMode == Controller.Auth.MasterMode.Revealed ? null : Dialog.InputDialog("insert master password", App);
        var rdd = App._Session.Decrypt(pwd.EncryptedData, master);

        if (rdd is not null && rdd.Message.Success && rdd.ReturnValue is not null)
        {
            var decrypt = rdd.ReturnValue;
            Dialog.OutputDialog(pwd.Name ?? "", decrypt, App);
        }
        else
        {
            Dialog.ErrorDialog(rdd is not null ? rdd.Message : Message.FailedToCreateReturn, App.Window);
        }

    }
    internal async Task OnDelete(Model.Key key)
    {
        var dialog = Dialog.ConfirmDialog($"do you really want to delete key: {key.Name}?", App.Window);
        if (dialog == ResponseType.Ok && App._Session is not null)
        {
            var rdd = await App._Session.Operations.DeleteKey(key.KID);
            if (rdd is not null && !rdd.Message.Success)
            {
                Dialog.ErrorDialog(rdd.Message, App.Window);
            }
            else if (rdd is null)
            {
                Dialog.ErrorDialog(Message.FailedToCreateReturn, App.Window);
            }
        }
    }
    internal async Task OnDelete(Password pwd)
    {
        var dialog = Dialog.ConfirmDialog($"do you really want to delete password: {pwd.Name}?", App.Window);
        if(dialog == ResponseType.Ok && App._Session is not null)
        {
            var rdd = await App._Session.Operations.DeletePassword(pwd.PID);
            if (rdd is not null && !rdd.Message.Success)
            {
                Dialog.ErrorDialog(rdd.Message, App.Window);
            }
            else if(rdd is null)
            {
                Dialog.ErrorDialog(Message.FailedToCreateReturn, App.Window);
            }
        }
    }
    #endregion
    
}