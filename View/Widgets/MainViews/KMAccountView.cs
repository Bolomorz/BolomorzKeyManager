using Gtk;
using BolomorzKeyManager.Controller.Data;

namespace BolomorzKeyManager.View.Widgets;

internal class KMAccountView : KMGrid
{
    private readonly KMMain Main;

    private readonly Entry UEntry;
    private readonly Button UHide;

    private readonly Entry PEntry1;
    private readonly Button PHide1;

    private readonly Entry PEntry2;
    private readonly Button PHide2;

    private readonly Button Submit;


    internal KMAccountView(KeyManager app, KMMain main) : base(app)
    {
        Main = main;

        RowSpacing = 10;
        RowHomogeneous = true;
        ColumnSpacing = 10;
        ColumnHomogeneous = false;
        Hexpand = true;
        Vexpand = true;
        Margin = 10;

        var heading = new Label("Account Settings");

        var ulabel = new Label("Username:");
        UEntry = new Entry() { Text = App._Session?.AuthUser?.UserAccount.Username, Visibility = true, InvisibleChar = '*', InvisibleCharSet = false, Hexpand = true, Vexpand = true, PlaceholderText = "insert username" };
        UHide = new Button() { Image = new Image(App.ConcealSymbolic), Hexpand = true, Vexpand = true };

        var plabel1 = new Label("New Password:");
        PEntry1 = new Entry() { Visibility = false, InvisibleChar = '*', InvisibleCharSet = true, Hexpand = true, Vexpand = true, PlaceholderText = "insert password" };
        PHide1 = new Button() { Image = new Image(App.RevealSymbolic), Hexpand = true, Vexpand = true };

        var plabel2 = new Label("Repeat Password:");
        PEntry2 = new Entry() { Visibility = false, InvisibleChar = '*', InvisibleCharSet = true, Hexpand = true, Vexpand = true, PlaceholderText = "insert password again" };
        PHide2 = new Button() { Image = new Image(App.RevealSymbolic), Hexpand = true, Vexpand = true };

        Submit = new Button() { Label = "Save", Hexpand = true, Vexpand = true };

        Attach(heading, 0, 0, 3, 1);

        Attach(ulabel, 0, 1, 1, 1);
        Attach(UEntry, 1, 1, 1, 1);
        Attach(UHide, 2, 1, 1, 1);

        Attach(plabel1, 0, 2, 1, 1);
        Attach(PEntry1, 1, 2, 1, 1);
        Attach(PHide1, 2, 2, 1, 1);

        Attach(plabel2, 0, 3, 1, 1);
        Attach(PEntry2, 1, 3, 1, 1);
        Attach(PHide2, 2, 3, 1, 1);

        Attach(Submit, 0, 4, 3, 1);

        UHide.Clicked += OnUserHide;
        PHide1.Clicked += OnPwd1Hide;
        PHide2.Clicked += OnPwd2Hide;
        Submit.Clicked += OnSubmit;
    }

    private async void OnSubmit(object? sender, EventArgs e)
    {
        var usr = UEntry.Text;
        var pwd1 = PEntry1.Text;
        var pwd2 = PEntry2.Text;

        if (App._Session is not null)
        {
            if
            (
                pwd1 is not null && pwd1 != "" && pwd1 == pwd2 &&
                usr is not null && usr != ""
            )
            {
                var rd = await App._Session.Operations.SetAccountData(usr, pwd1);
                if (rd is not null)
                {
                    Dialog.ErrorDialog(rd.Message, App.Window);
                    if (rd.Message.Success) Main.OnStart(sender, e);
                }
                else
                {
                    Dialog.ErrorDialog(Message.FailedToCreateReturn, App.Window);
                }
            }
            else
            {
                Dialog.ErrorDialog(Message.PasswordDontMatch, App.Window);
            }
        }
        else
        {
            Dialog.ErrorDialog(Message.UnauthorizedAccess, App.Window);
        }

    }

    private void OnUserHide(object? sender, EventArgs e)
    {
        UEntry.Visibility = !UEntry.Visibility;
        UEntry.InvisibleCharSet = !UEntry.InvisibleCharSet;
        UHide.Image = UEntry.Visibility ? new Image(App.ConcealSymbolic) : new Image(App.RevealSymbolic);
    }

    private void OnPwd1Hide(object? sender, EventArgs e)
    {
        PEntry1.Visibility = !PEntry1.Visibility;
        PEntry1.InvisibleCharSet = !PEntry1.InvisibleCharSet;
        PHide1.Image = PEntry1.Visibility ? new Image(App.ConcealSymbolic) : new Image(App.RevealSymbolic);
    }

    private void OnPwd2Hide(object? sender, EventArgs e)
    {
        PEntry2.Visibility = !PEntry2.Visibility;
        PEntry2.InvisibleCharSet = !PEntry2.InvisibleCharSet;
        PHide2.Image = PEntry2.Visibility ? new Image(App.ConcealSymbolic) : new Image(App.RevealSymbolic);
    }
    
}