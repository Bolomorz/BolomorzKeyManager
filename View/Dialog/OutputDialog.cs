using Gtk;

namespace BolomorzKeyManager.View;

internal class OutputDialog : Gtk.Dialog
{
    private readonly KeyManager App;
    private readonly Entry Output;
    private readonly Button BtHide;

    internal OutputDialog(string name, string decrypt, KeyManager app) : base($"Passkey {name}", app.Window, DialogFlags.Modal)
    {
        App = app;

        Grid grid = [];
        grid.RowSpacing = 10;
        grid.RowHomogeneous = true;
        grid.ColumnSpacing = 10;
        grid.ColumnHomogeneous = false;
        grid.Hexpand = true;
        grid.Vexpand = true;
        grid.Margin = 10;

        Output = new() { Visibility = false, InvisibleChar = '*', InvisibleCharSet = true, Text = decrypt, IsEditable = false, CanFocus = false };
        BtHide = new() { Image = new Image(App.RevealSymbolic) };

        BtHide.Clicked += OnHide;
        grid.Attach(Output, 0, 0, 3, 1);
        grid.Attach(BtHide, 3, 0, 1, 1);
        ContentArea.Add(grid);

        ShowAll();
        AddButton("Close", ResponseType.Close);
    }


    private void OnHide(object? sender, EventArgs e)
    {
        Output.Visibility = !Output.Visibility;
        Output.InvisibleCharSet = !Output.InvisibleCharSet;
        BtHide.Image = Output.Visibility ? new Image(App.ConcealSymbolic) : new Image(App.RevealSymbolic);
    }

}