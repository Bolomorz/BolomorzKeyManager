using BolomorzKeyManager.Controller.Data;
using BolomorzKeyManager.View.Windows;
using Gtk;

namespace BolomorzKeyManager.View;

internal class InputDialog : Gtk.Dialog
{
    private KeyManager App;
    private readonly Entry Input;
    private readonly Button BtHide;

    internal InputDialog(string message, KeyManager app) : base(message, app.Window, DialogFlags.Modal)
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

        Input = new() { Visibility = false, InvisibleChar = '*', InvisibleCharSet = true, PlaceholderText = message };
        BtHide = new() { Image = new Image(App.RevealSymbolic) };

        BtHide.Clicked += OnHide;
        grid.Attach(Input, 0, 0, 3, 1);
        grid.Attach(BtHide, 3, 0, 1, 1);
        ContentArea.Add(grid);

        ShowAll();
        AddButton("Cancel", ResponseType.Cancel);
        AddButton("Submit", ResponseType.Apply);
    }

    private void OnHide(object? sender, EventArgs e)
    {
        Input.Visibility = !Input.Visibility;
        Input.InvisibleCharSet = !Input.InvisibleCharSet;
        BtHide.Image = Input.Visibility ? new Image(App.ConcealSymbolic) : new Image(App.RevealSymbolic);
    }

    internal new string? Run()
        => (ResponseType)base.Run() == ResponseType.Apply ? Input.Text : null;
    
}