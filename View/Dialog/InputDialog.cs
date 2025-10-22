using BolomorzKeyManager.Controller.Data;
using BolomorzKeyManager.View.Windows;
using Gtk;

namespace BolomorzKeyManager.View;

internal class InputDialog : Gtk.Dialog
{
    private readonly Entry Input;

    internal InputDialog(string message, MainWindow window) : base(message, window, DialogFlags.Modal)
    {
        Input = new() { Visibility = true, PlaceholderText = message };
        ContentArea.Add(Input);
        ShowAll();
        AddButton("Cancel", ResponseType.Cancel);
        AddButton("Submit", ResponseType.Apply);
    }

    internal new string? Run()
        => (ResponseType)base.Run() == ResponseType.Apply ? Input.Text : null;
    
}