using Gtk;

namespace BolomorzKeyManager.View.Widgets;

internal class KMMenu : MenuBar
{
    private readonly KMMain Main;

    internal KMMenu(KMMain main)
    {
        Main = main;

        #region App Menu
        var mApp = new Menu();
        var miApp = new MenuItem("App") { Submenu = mApp };

        var miLogout = new MenuItem("Logout");
        miLogout.Activated += Main.OnMenuLogout;
        mApp.Append(miLogout);

        var miAccount = new MenuItem("Account");
        miAccount.Activated += Main.OnMenuAccount;
        mApp.Append(miAccount);

        var miSep1 = new SeparatorMenuItem();
        mApp.Append(miSep1);

        var miClose = new MenuItem("Quit");
        miClose.Activated += Main.OnMenuClose;
        mApp.Append(miClose);

        Append(miApp);
        #endregion

        #region Key Menu
        var mKeys = new Menu();
        var miKeys = new MenuItem("Keys") { Submenu = mKeys };

        var miShowAll = new MenuItem("Show All");
        miShowAll.Activated += Main.OnMenuShowAll;
        mKeys.Append(miShowAll);

        var miShowKeys = new MenuItem("Show Keys");
        miShowKeys.Activated += Main.OnMenuShowKeys;
        mKeys.Append(miShowKeys);

        var miShowPwds = new MenuItem("Show Passwords");
        miShowPwds.Activated += Main.OnMenuShowPasswords;
        mKeys.Append(miShowPwds);

        var miSep2 = new SeparatorMenuItem();
        mKeys.Append(miSep2);

        var miNewKey = new MenuItem("New Key");
        miNewKey.Activated += Main.OnMenuNewKey;
        mKeys.Append(miNewKey);

        var miNewPwd = new MenuItem("New Password");
        miNewPwd.Activated += Main.OnMenuNewPassword;
        mKeys.Append(miNewPwd);

        Append(miKeys);
        #endregion

        #region Info Menu
        
        #endregion
    }
}