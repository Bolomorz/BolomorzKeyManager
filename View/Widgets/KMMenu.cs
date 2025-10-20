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
        miLogout.Activated += Main.OnLogout;
        mApp.Append(miLogout);

        var miAccount = new MenuItem("Account");
        miAccount.Activated += Main.OnAccount;
        mApp.Append(miAccount);

        var miSep1 = new SeparatorMenuItem();
        mApp.Append(miSep1);

        var miClose = new MenuItem("Quit");
        miClose.Activated += Main.OnClose;
        mApp.Append(miClose);

        Append(miApp);
        #endregion

        #region Key Menu
        var mKeys = new Menu();
        var miKeys = new MenuItem("Keys") { Submenu = mKeys };

        var miShowAll = new MenuItem("Show All");
        miShowAll.Activated += Main.OnShowAll;
        mKeys.Append(miShowAll);

        var miShowKeys = new MenuItem("Show Keys");
        miShowKeys.Activated += Main.OnShowKeys;
        mKeys.Append(miShowKeys);

        var miShowPwds = new MenuItem("Show Passwords");
        miShowPwds.Activated += Main.OnShowPasswords;
        mKeys.Append(miShowPwds);

        var miSep2 = new SeparatorMenuItem();
        mKeys.Append(miSep2);

        var miNewKey = new MenuItem("New Key");
        miNewKey.Activated += Main.OnNewKey;
        mKeys.Append(miNewKey);

        var miNewPwd = new MenuItem("New Password");
        miNewPwd.Activated += Main.OnNewPassword;
        mKeys.Append(miNewPwd);

        Append(miKeys);
        #endregion

        #region Info Menu
        
        #endregion
    }
}