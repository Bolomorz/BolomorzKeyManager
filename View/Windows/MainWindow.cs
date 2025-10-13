using BolomorzKeyManager.View.Widgets;
using Gtk;

namespace BolomorzKeyManager.View.Windows;

internal class MainWindow : Window
{
    private readonly KeyManager App;
    
    internal MainWindow(KeyManager app) : base("Bolomorz Key Manager")
    {
        App = app;
        DeleteEvent += WindowDeleteEvent;
    }

    internal void ShowLoginForm()
    {
        Clear();
        var loginForm = new LoginForm(App, this);
        ShowAll();
    }

    private void Clear()
    {
        foreach (var child in Children)
        {
            Remove(child);
        }
    }
    private void WindowDeleteEvent(object sender, DeleteEventArgs args) { Application.Quit(); }
}