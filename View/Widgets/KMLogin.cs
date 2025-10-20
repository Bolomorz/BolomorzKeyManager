using BolomorzKeyManager.Controller.Auth;
using BolomorzKeyManager.Controller.Data;
using Gtk;

namespace BolomorzKeyManager.View.Widgets;

internal class KMLogin : KMGrid
{
    private readonly Entry UEntry;
    private readonly Button UHide;

    private readonly Entry PEntry;
    private readonly Button PHide;

    private readonly Button Register;
    private readonly Button Submit;

    internal KMLogin(KeyManager app) : base(app)
    {
        RowSpacing = 10;
        RowHomogeneous = true;
        ColumnSpacing = 10;
        ColumnHomogeneous = false;
        Hexpand = true;
        Vexpand = true;
        Margin = 10;

        var heading = new Label("Login");

        var ulabel = new Label("Username:");
        UEntry = new Entry() { Visibility = true, InvisibleChar = '*', InvisibleCharSet = false, Hexpand = true, Vexpand = true };
        UHide = new Button() { Image = new Image(App.ConcealSymbolic), Hexpand = true, Vexpand = true };

        var plabel = new Label("Password:");
        PEntry = new Entry() { Visibility = false, InvisibleChar = '*', InvisibleCharSet = true, Hexpand = true, Vexpand = true };
        PHide = new Button() { Image = new Image(App.RevealSymbolic), Hexpand = true, Vexpand = true };

        Submit = new Button() { Label = "Login", Hexpand = true, Vexpand = true };
        Register = new Button() { Label = "Register", Hexpand = true, Vexpand = true };

        Attach(heading, 0, 0, 2, 1);
        Attach(Register, 2, 0, 1, 1);

        Attach(ulabel, 0, 1, 1, 1);
        Attach(UEntry, 1, 1, 1, 1);
        Attach(UHide, 2, 1, 1, 1);

        Attach(plabel, 0, 2, 1, 1);
        Attach(PEntry, 1, 2, 1, 1);
        Attach(PHide, 2, 2, 1, 1);

        Attach(Submit, 0, 3, 3, 1);

        UHide.Clicked += OnUserHide;
        PHide.Clicked += OnPwdHide;
        Submit.Clicked += OnSubmit;
        Register.Clicked += OnRegister;
    }

    private async void OnSubmit(object? sender, EventArgs e)
    {
        App._Session?.Close();

        var username = UEntry.Text;
        var password = PEntry.Text;

        var rd = await Session.OpenSession(username, password);
        if (rd is not null && rd.Message.Success && rd.ReturnValue is not null)
        {
            App._Session = rd.ReturnValue;
            App.Window.ShowMain();
        }
        else if (rd is not null)
        {
            Dialog.ErrorDialog(rd.Message, App.Window);
        }
        else
        {
            Dialog.ErrorDialog(Message.FailedToCreateReturn, App.Window);
        }

    }

    private void OnRegister(object? sender, EventArgs e)
    {
        App.Window.ShowRegister();
    }

    private void OnUserHide(object? sender, EventArgs e)
    {
        UEntry.Visibility = !UEntry.Visibility;
        UEntry.InvisibleCharSet = !UEntry.InvisibleCharSet;
        UHide.Image = UEntry.Visibility ? new Image(App.ConcealSymbolic) : new Image(App.RevealSymbolic);
    }

    private void OnPwdHide(object? sender, EventArgs e)
    {
        PEntry.Visibility = !PEntry.Visibility;
        PEntry.InvisibleCharSet = !PEntry.InvisibleCharSet;
        PHide.Image = PEntry.Visibility ? new Image(App.ConcealSymbolic) : new Image(App.RevealSymbolic);
    }
    
}