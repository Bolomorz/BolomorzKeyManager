using BolomorzKeyManager.Controller.Auth;
using BolomorzKeyManager.Controller.Data;
using BolomorzKeyManager.View.Windows;
using Gtk;

namespace BolomorzKeyManager.View.Widgets;

internal class LoginForm
{
    private readonly KeyManager App;
    private readonly MainWindow Window;

    private readonly Grid Grid;

    private readonly Entry UEntry;
    private readonly Button UHide;

    private readonly Entry PEntry;
    private readonly Button PHide;

    private readonly Button Register;
    private readonly Button Submit;

    internal LoginForm(KeyManager app, MainWindow window)
    {
        App = app;
        Window = window;

        Grid = [];
        Grid.RowSpacing = 10;
        Grid.RowHomogeneous = true;
        Grid.ColumnSpacing = 10;
        Grid.ColumnHomogeneous = false;
        Grid.Hexpand = true;
        Grid.Vexpand = true;
        Grid.Margin = 10;

        var icon = App.Theme.LoadIcon("view-conceal-symbolic", 24, IconLookupFlags.UseBuiltin | IconLookupFlags.GenericFallback);

        var heading = new Label("Login");
        
        var ulabel = new Label("Username:");
        UEntry = new Entry() { Visibility = true, InvisibleChar = '*', InvisibleCharSet = false, Hexpand = true, Vexpand = true };
        UHide = new Button() { Image = new Image(icon), Hexpand = true, Vexpand = true };

        var plabel = new Label("Password:");
        PEntry = new Entry() { Visibility = false, InvisibleChar = '*', InvisibleCharSet = true, Hexpand = true, Vexpand = true };
        PHide = new Button() { Image = new Image(icon), Hexpand = true, Vexpand = true };

        Submit = new Button() { Label = "Login", Hexpand = true, Vexpand = true };
        Register = new Button() { Label = "Register", Hexpand = true, Vexpand = true };

        Grid.Attach(heading, 0, 0, 2, 1);
        Grid.Attach(Register, 2, 0, 1, 1);

        Grid.Attach(ulabel, 0, 1, 1, 1);
        Grid.Attach(UEntry, 1, 1, 1, 1);
        Grid.Attach(UHide, 2, 1, 1, 1);

        Grid.Attach(plabel, 0, 2, 1, 1);
        Grid.Attach(PEntry, 1, 2, 1, 1);
        Grid.Attach(PHide, 2, 2, 1, 1);

        Grid.Attach(Submit, 0, 3, 3, 1);

        window.Add(Grid);

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
            Console.WriteLine("login successful");
        }
        else if (rd is not null)
        {
            Dialog.ErrorDialog(rd.Message, Window);
        }
        else
        {
            Dialog.ErrorDialog(Message.FailedToCreateReturn, Window);
        }

    }

    private async void OnRegister(object? sender, EventArgs e)
    {
        Window.ShowRegisterForm();
    }

    private async void OnUserHide(object? sender, EventArgs e)
    {
        UEntry.Visibility = !UEntry.Visibility;
        UEntry.InvisibleCharSet = !UEntry.InvisibleCharSet;
    }
    
    private async void OnPwdHide(object? sender, EventArgs e)
    {
        PEntry.Visibility = !PEntry.Visibility;
        PEntry.InvisibleCharSet = !PEntry.InvisibleCharSet;
    }
}