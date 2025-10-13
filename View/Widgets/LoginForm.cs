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

    private string Username;
    private readonly Entry UEntry;
    private readonly Button UHide;

    private string Password;
    private readonly Entry PEntry;
    private readonly Button PHide;

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
        Grid.Margin = 10;

        var icon = App.Theme.LoadIcon("view-conceal-symbolic", 24, IconLookupFlags.UseBuiltin | IconLookupFlags.GenericFallback);

        var ulabel = new Label("Username:");
        UEntry = new Entry() { Visibility = true, InvisibleChar = '*', InvisibleCharSet = false };
        UHide = new Button() { Image = new Image(icon) };
        Username = "";

        var plabel = new Label("Password:");
        PEntry = new Entry() { Visibility = false, InvisibleChar = '*', InvisibleCharSet = true };
        PHide = new Button() { Image = new Image(icon) };
        Password = "";

        Submit = new Button() { Label = "Login" };

        Grid.Attach(ulabel, 0, 0, 1, 1);
        Grid.Attach(UEntry, 1, 0, 1, 1);
        Grid.Attach(UHide, 2, 0, 1, 1);

        Grid.Attach(plabel, 0, 1, 1, 1);
        Grid.Attach(PEntry, 1, 1, 1, 1);
        Grid.Attach(PHide, 2, 1, 1, 1);

        Grid.Attach(Submit, 0, 2, 3, 1);

        window.Add(Grid);

        UEntry.Changed += OnUserEntryChanged;
        UHide.Clicked += OnUserHide;
        PEntry.Changed += OnPwdEntryChanged;
        PHide.Clicked += OnPwdHide;
        Submit.Clicked += OnSubmit;
    }

    private async void OnSubmit(object? sender, EventArgs e)
    {
        App._Session?.Close();

        var rd = await Session.OpenSession(Username, Password);
        if (rd is not null && rd.Message.Success && rd.ReturnValue is not null)
        {
            App._Session = rd.ReturnValue;
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

    private async void OnUserEntryChanged(object? sender, EventArgs e)
    {
        Username = UEntry.Text;
    }
    
    private async void OnPwdEntryChanged(object? sender, EventArgs e)
    {
        Password = PEntry.Text;
    }
}