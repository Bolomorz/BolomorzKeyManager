using Gtk;
using BolomorzKeyManager.Controller.Data;
using BolomorzKeyManager.Controller.Encryption;

namespace BolomorzKeyManager.View.Widgets;

internal enum NewMode { Pwd, Key }

internal class KMNewView : KMGrid
{
    private readonly KMMain Main;
    private readonly NewMode NewMode;

    private readonly Button Status;
    private readonly Entry MasterEntry;
    private readonly Button MasterHide;
    private readonly Button SwitchMasterMode;

    private readonly Entry NameEntry;
    private readonly Entry DescriptionEntry;
    private readonly Entry DataEntry;
    private readonly Button DataHide;

    private readonly Button Submit;

    internal KMNewView(KeyManager app, KMMain main, NewMode mode) : base(app)
    {
        Main = main;
        NewMode = mode;

        RowSpacing = 10;
        RowHomogeneous = true;
        ColumnSpacing = 10;
        ColumnHomogeneous = false;
        Hexpand = true;
        Vexpand = true;
        Margin = 10;

        Label heading = NewMode switch
        {
            NewMode.Key => new("New Key"),
            NewMode.Pwd => new("New Password"),
            _ => throw new NotImplementedException()
        };

        var master = new Label("Mastermode:");
        Status = App._Session?.MasterMode switch
        {
            Controller.Auth.MasterMode.Concealed => new() { Label = "Inactive", Image = new Image(App.InactiveSymbolic), Hexpand = true, Vexpand = true },
            Controller.Auth.MasterMode.Revealed => new() { Label = "Active", Image = new Image(App.ActiveSymbolic), Hexpand = true, Vexpand = true },
            _ => throw new NotImplementedException()
        };
        SwitchMasterMode = App._Session?.MasterMode switch
        {
            Controller.Auth.MasterMode.Concealed => new() { Label = "Activate" },
            Controller.Auth.MasterMode.Revealed => new() { Label = "Deactivate" },
            _ => throw new NotImplementedException()
        };
        MasterHide = new() { Image = new Image(App.RevealSymbolic), Hexpand = true, Vexpand = true };
        MasterEntry = new() { Visibility = false, InvisibleChar = '*', InvisibleCharSet = true, Hexpand = true, Vexpand = true };

        var namelabel = new Label("Name:");
        NameEntry = new Entry() { Visibility = true, InvisibleChar = '*', InvisibleCharSet = false, Hexpand = true, Vexpand = true, PlaceholderText = "insert name" };

        var desclabel = new Label("Description:");
        DescriptionEntry = new Entry() { Visibility = true, InvisibleChar = '*', InvisibleCharSet = false, Hexpand = true, Vexpand = true, PlaceholderText = "insert description" };

        Label datalabel = NewMode switch
        {
            NewMode.Key => new("Key:"),
            NewMode.Pwd => new("Password:"),
            _ => throw new NotImplementedException()
        };
        DataEntry = new Entry() { Visibility = false, InvisibleChar = '*', InvisibleCharSet = true, Hexpand = true, Vexpand = true, PlaceholderText = "insert password again" };
        DataHide = new Button() { Image = new Image(App.RevealSymbolic), Hexpand = true, Vexpand = true };

        Submit = new Button() { Label = "Submit", Hexpand = true, Vexpand = true };

        Attach(heading, 0, 0, 4, 1);

        Attach(master, 0, 2, 3, 1);
        Attach(Status, 3, 2, 1, 1);
        Attach(SwitchMasterMode, 0, 3, 1, 1);
        Attach(MasterEntry, 1, 3, 2, 1);
        Attach(MasterHide, 3, 3, 1, 1);

        Attach(namelabel, 0, 5, 1, 1);
        Attach(NameEntry, 1, 5, 3, 1);
        Attach(desclabel, 0, 6, 1, 1);
        Attach(DescriptionEntry, 1, 6, 3, 1);
        Attach(datalabel, 0, 7, 1, 1);
        Attach(DataEntry, 1, 7, 2, 1);
        Attach(DataHide, 3, 7, 1, 1);

        Attach(Submit, 0, 8, 4, 1);

        DataHide.Clicked += OnDataHide;
        MasterHide.Clicked += OnMasterHide;
        SwitchMasterMode.Clicked += OnSwitchMasterMode;
        Submit.Clicked += OnSubmit;
    }

    private async void OnSubmit(object? sender, EventArgs e)
    {

        var name = NameEntry.Text;
        var desc = DescriptionEntry.Text;
        var data = DataEntry.Text;

        if (App._Session is null) return;

        var master = App._Session.MasterMode == Controller.Auth.MasterMode.Revealed ? null : Dialog.InputDialog("insert master password", App);
        var rde = App._Session.Encrypt(data, master);

        if(rde is not null && rde.Message.Success && rde.ReturnValue is not null)
        {
            var encrypt = rde.ReturnValue;
            var rds = NewMode == NewMode.Key ? await App._Session.Operations.SaveKey(name, desc, encrypt, null) : await App._Session.Operations.SavePassword(name, desc, encrypt, null);

            if (rds is not null && rds.Message.Success)
            {
                Dialog.InfoDialog(Message.Successful, App.Window);
                switch (NewMode)
                {
                    case NewMode.Key: Main.OnNewKey(sender, e); break;
                    case NewMode.Pwd: Main.OnNewPassword(sender, e); break;
                }
            }
            else
            {
                Dialog.ErrorDialog(rds is not null ? rds.Message : Message.FailedToCreateReturn, App.Window);
            }
        }
        else
        {
            Dialog.ErrorDialog(rde is not null ? rde.Message : Message.FailedToCreateReturn, App.Window);
        }
        
    }

    private void OnSwitchMasterMode(object? sender, EventArgs e)
    {
        switch (App._Session?.MasterMode)
        {
            case Controller.Auth.MasterMode.Concealed:
                if(MasterEntry.Text is not null && MasterEntry.Text.Trim() != "")
                {
                    App._Session.Reveal(MasterEntry.Text);
                    SwitchMasterMode.Label = "Deactivate";
                    Status.Label = "Active";
                    Status.Image = new Image(App.ActiveSymbolic);
                }
            break;
            case Controller.Auth.MasterMode.Revealed:
                App._Session.Conceal(); 
                SwitchMasterMode.Label = "Activate";
                Status.Label = "Inactive";
                Status.Image = new Image(App.InactiveSymbolic);
                MasterEntry.Text = "";
            break;
        }
    }

    private void OnMasterHide(object? sender, EventArgs e)
    {
        MasterEntry.Visibility = !MasterEntry.Visibility;
        MasterEntry.InvisibleCharSet = !MasterEntry.InvisibleCharSet;
        MasterHide.Image = MasterEntry.Visibility ? new Image(App.ConcealSymbolic) : new Image(App.RevealSymbolic);
    }

    private void OnDataHide(object? sender, EventArgs e)
    {
        DataEntry.Visibility = !DataEntry.Visibility;
        DataEntry.InvisibleCharSet = !DataEntry.InvisibleCharSet;
        DataHide.Image = DataEntry.Visibility ? new Image(App.ConcealSymbolic) : new Image(App.RevealSymbolic);
    }

}