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

        Attach(menu, 0, 0, 1, 1);
    }

    #region App Menu
    internal async void OnMenuLogout(object? sender, EventArgs e)
    {
        App._Session?.Close();
        App._Session = null;
        App.Window.ShowLogin();
    }

    internal async void OnMenuAccount(object? sender, EventArgs e)
    {

    }

    internal async void OnMenuClose(object? sender, EventArgs e)
    {
        App._Session?.Close();
        App._Session = null;
        App.Window.Close();
    }
    #endregion

    #region Key Menu
    internal async void OnMenuShowAll(object? sender, EventArgs e)
    {

    }
    internal async void OnMenuShowKeys(object? sender, EventArgs e)
    {

    }
    internal async void OnMenuShowPasswords(object? sender, EventArgs e)
    {

    }
    internal async void OnMenuNewKey(object? sender, EventArgs e)
    {

    }
    internal async void OnMenuNewPassword(object? sender, EventArgs e)
    {

    }
    #endregion

    #region Info Menu

    #endregion
    
}