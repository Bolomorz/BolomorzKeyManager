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
    internal void OnKey(Model.Key key)
    {

    }
    internal void OnPwd(Password pwd)
    {
        
    }
    #endregion
    
}