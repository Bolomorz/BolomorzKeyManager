using BolomorzKeyManager.Controller.Auth;
using BolomorzKeyManager.Controller.Data;
using BolomorzKeyManager.Model;
using Gtk;

namespace BolomorzKeyManager.View.Widgets;

internal class RegisterForm : FormBase
{
    private readonly KeyManager App;

    private readonly Entry UEntry;
    private readonly Button UHide;

    private readonly Entry PEntry1;
    private readonly Button PHide1;

    private readonly Entry PEntry2;
    private readonly Button PHide2;


    private readonly Button Submit;

    internal RegisterForm(KeyManager app) : base([])
    {
        App = app;

        Form.RowSpacing = 10;
        Form.RowHomogeneous = true;
        Form.ColumnSpacing = 10;
        Form.ColumnHomogeneous = false;
        Form.Hexpand = true;
        Form.Vexpand = true;
        Form.Margin = 10;

        var icon = App.Theme.LoadIcon("view-conceal-symbolic", 24, IconLookupFlags.UseBuiltin | IconLookupFlags.GenericFallback);

        var heading = new Label("Register new User");

        var ulabel = new Label("Username:");
        UEntry = new Entry() { Visibility = true, InvisibleChar = '*', InvisibleCharSet = false, Hexpand = true, Vexpand = true, PlaceholderText = "insert username" };
        UHide = new Button() { Image = new Image(icon), Hexpand = true, Vexpand = true };

        var plabel1 = new Label("Password:");
        PEntry1 = new Entry() { Visibility = false, InvisibleChar = '*', InvisibleCharSet = true, Hexpand = true, Vexpand = true, PlaceholderText = "insert password" };
        PHide1 = new Button() { Image = new Image(icon), Hexpand = true, Vexpand = true };

        var plabel2 = new Label("Password:");
        PEntry2 = new Entry() { Visibility = false, InvisibleChar = '*', InvisibleCharSet = true, Hexpand = true, Vexpand = true, PlaceholderText = "insert password again" };
        PHide2 = new Button() { Image = new Image(icon), Hexpand = true, Vexpand = true };

        Submit = new Button() { Label = "Register", Hexpand = true, Vexpand = true };

        Form.Attach(heading, 0, 0, 3, 1);

        Form.Attach(ulabel, 0, 1, 1, 1);
        Form.Attach(UEntry, 1, 1, 1, 1);
        Form.Attach(UHide, 2, 1, 1, 1);

        Form.Attach(plabel1, 0, 2, 1, 1);
        Form.Attach(PEntry1, 1, 2, 1, 1);
        Form.Attach(PHide1, 2, 2, 1, 1);

        Form.Attach(plabel2, 0, 3, 1, 1);
        Form.Attach(PEntry2, 1, 3, 1, 1);
        Form.Attach(PHide2, 2, 3, 1, 1);

        Form.Attach(Submit, 0, 4, 3, 1);

        UHide.Clicked += OnUserHide;
        PHide1.Clicked += OnPwd1Hide;
        PHide2.Clicked += OnPwd2Hide;
        Submit.Clicked += OnSubmit;
    }

    private async void OnSubmit(object? sender, EventArgs e)
    {
        App._Session?.Close();

        var usr = UEntry.Text;
        var pwd1 = PEntry1.Text;
        var pwd2 = PEntry2.Text;
        if
        (
            pwd1 is not null && pwd1 != "" && pwd1 == pwd2 &&
            usr is not null && usr != ""
        )
        {
            var rd = await Session.CreateUser(usr, pwd1);
            if (rd is not null)
            {
                Dialog.ErrorDialog(rd.Message, App.Window);
                if (rd.Message.Success) App.Window.ShowLoginForm();
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

    private async void OnUserHide(object? sender, EventArgs e)
    {
        UEntry.Visibility = !UEntry.Visibility;
        UEntry.InvisibleCharSet = !UEntry.InvisibleCharSet;
    }

    private async void OnPwd1Hide(object? sender, EventArgs e)
    {
        PEntry1.Visibility = !PEntry1.Visibility;
        PEntry1.InvisibleCharSet = !PEntry1.InvisibleCharSet;
    }

    private async void OnPwd2Hide(object? sender, EventArgs e)
    {
        PEntry2.Visibility = !PEntry2.Visibility;
        PEntry2.InvisibleCharSet = !PEntry2.InvisibleCharSet;
    }

}