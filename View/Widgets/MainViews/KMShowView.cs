using Gtk;
using BolomorzKeyManager.Controller.Data;

namespace BolomorzKeyManager.View.Widgets;

internal enum ShowMode { All, Pwd, Key }

internal class KMShowView : KMGrid
{
    private readonly KMMain Main;
    private readonly ShowMode ShowMode;

    private readonly Button Status;
    private readonly Entry MasterEntry;
    private readonly Button MasterHide;
    private readonly Button SwitchMasterMode;

    private readonly Entry QueryEntry;
    private readonly Button Query;

    private readonly ScrolledWindow Table;
    private Grid Grid;

    internal KMShowView(KeyManager app, KMMain main, ShowMode mode) : base(app)
    {
        Main = main;
        ShowMode = mode;

        RowSpacing = 10;
        RowHomogeneous = false;
        ColumnSpacing = 10;
        ColumnHomogeneous = false;
        Hexpand = true;
        Vexpand = true;
        Margin = 10;

        Label heading = ShowMode switch
        {
            ShowMode.All => new("Keys and Passwords") { Hexpand = true, Vexpand = false },
            ShowMode.Key => new("Keys") { Hexpand = true, Vexpand = false },
            ShowMode.Pwd => new("Passwords") { Hexpand = true, Vexpand = false },
            _ => throw new NotImplementedException()
        };

        var master = new Label("Mastermode:");
        Status = App._Session?.MasterMode switch
        {
            Controller.Auth.MasterMode.Concealed => new() { Label = "Inactive", Image = new Image(App.Inactive), Hexpand = true, Vexpand = false },
            Controller.Auth.MasterMode.Revealed => new() { Label = "Active", Image = new Image(App.Active), Hexpand = true, Vexpand = false },
            _ => throw new NotImplementedException()
        };
        SwitchMasterMode = App._Session?.MasterMode switch
        {
            Controller.Auth.MasterMode.Concealed => new() { Label = "Activate", Hexpand = true, Vexpand = false },
            Controller.Auth.MasterMode.Revealed => new() { Label = "Deactivate", Hexpand = true, Vexpand = false },
            _ => throw new NotImplementedException()
        };
        MasterHide = new() { Image = new Image(App.RevealSymbolic), Hexpand = true, Vexpand = false };
        MasterEntry = new() { Visibility = false, InvisibleChar = '*', InvisibleCharSet = true, Hexpand = true, Vexpand = false };

        QueryEntry = new() { Visibility = true, InvisibleChar = '*', InvisibleCharSet = false, Hexpand = true, Vexpand = false };
        Query = new() { Label = "Search", Hexpand = true, Vexpand = false };

        Table = [];
        Table.Hexpand = true;
        Table.Vexpand = true;
        Table.Margin = 10;

        Grid = [];
        Table.Add(Grid);


        Attach(heading, 0, 0, 4, 1);

        Attach(master, 0, 2, 3, 1);
        Attach(Status, 3, 2, 1, 1);
        Attach(SwitchMasterMode, 0, 3, 1, 1);
        Attach(MasterEntry, 1, 3, 2, 1);
        Attach(MasterHide, 3, 3, 1, 1);

        Attach(Query, 0, 5, 1, 1);
        Attach(QueryEntry, 1, 5, 3, 1);

        Attach(Table, 0, 6, 4, 1);

        FillItems();

        Query.Clicked += OnQuery;
        MasterHide.Clicked += OnMasterHide;
        SwitchMasterMode.Clicked += OnSwitchMasterMode;

    }

    private async void OnSwitchMasterMode(object? sender, EventArgs e)
    {
        switch (App._Session?.MasterMode)
        {
            case Controller.Auth.MasterMode.Concealed:
                if(MasterEntry.Text is not null && MasterEntry.Text.Trim() != "")
                {
                    App._Session.Reveal(MasterEntry.Text);
                    SwitchMasterMode.Label = "Deactivate";
                    Status.Label = "Active";
                    Status.Image = new Image(App.Active);
                }
            break;
            case Controller.Auth.MasterMode.Revealed:
                App._Session.Conceal(); 
                SwitchMasterMode.Label = "Activate";
                Status.Label = "Inactive";
                Status.Image = new Image(App.Inactive);
                MasterEntry.Text = "";
            break;
        }
    }

    private async void OnQuery(object? sender, EventArgs e)
    {
        FillItems();
        App.Window.ShowAll();
    }

    private void OnMasterHide(object? sender, EventArgs e)
    {
        MasterEntry.Visibility = !MasterEntry.Visibility;
        MasterEntry.InvisibleCharSet = !MasterEntry.InvisibleCharSet;
        MasterHide.Image = MasterEntry.Visibility ? new Image(App.ConcealSymbolic) : new Image(App.RevealSymbolic);
    }

    private async void FillItems()
    {
        Table.Remove(Grid);

        Grid = [];

        Grid.RowSpacing = 10;
        Grid.RowHomogeneous = true;
        Grid.ColumnSpacing = 10;
        Grid.ColumnHomogeneous = false;
        Grid.Hexpand = true;
        Grid.Vexpand = true;
        Grid.Margin = 10;

        if (App._Session is not null)
        {
            var query = QueryEntry.Text is not null && QueryEntry.Text.Trim().Length != 0 ? QueryEntry.Text : null;

            var row = 0;

            var rdk = ShowMode == ShowMode.All || ShowMode == ShowMode.Key ? await App._Session.Operations.GetKeys(new(query)) : null;
            if (rdk is not null && rdk.Message.Success && rdk.ReturnValue is not null)
            {
                var keys = rdk.ReturnValue;
                for (int i = 0; i < keys.Count; i++)
                {
                    var key = keys[i];
                    var lname = new Label(key.Name) { Hexpand = true, Vexpand = false };
                    var ldesc = new Label(key.Description) { Hexpand = true, Vexpand = false };
                    var breveal = new Button() { Image = new Image(App.RevealSymbolic), Hexpand = true, Vexpand = false };
                    breveal.Clicked += delegate { Main.OnKey(key); };

                    Grid.Attach(lname, 0, row, 1, 1);
                    Grid.Attach(ldesc, 1, row, 2, 1);
                    Grid.Attach(breveal, 3, row, 1, 1);
                    row++;
                }
            }

            var rdp = ShowMode == ShowMode.All || ShowMode == ShowMode.Pwd ? await App._Session.Operations.GetPasswords(new(query)) : null;
            if (rdp is not null && rdp.Message.Success && rdp.ReturnValue is not null)
            {
                var pwds = rdp.ReturnValue;
                for (int i = 0; i < pwds.Count; i++)
                {
                    var pwd = pwds[i];
                    var lname = new Label(pwd.Name) { Hexpand = true, Vexpand = false };
                    var ldesc = new Label(pwd.Description) { Hexpand = true, Vexpand = false };
                    var breveal = new Button() { Image = new Image(App.RevealSymbolic), Hexpand = true, Vexpand = false };
                    breveal.Clicked += delegate { Main.OnPwd(pwd); };

                    Grid.Attach(lname, 0, row, 1, 1);
                    Grid.Attach(ldesc, 1, row, 2, 1);
                    Grid.Attach(breveal, 3, row, 1, 1);
                    row++;
                }
            }
        }

        Table.Add(Grid);
    }
}