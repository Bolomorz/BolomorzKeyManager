using BolomorzKeyManager.View.Widgets;
using Gtk;

namespace BolomorzKeyManager.View.Windows;

internal class MainWindow : Window
{
    private readonly KeyManager App;
    private FormBase? Form;
    
    internal MainWindow(KeyManager app) : base("Bolomorz Key Manager")
    {
        App = app;
        SetPosition(WindowPosition.Center);
        DeleteEvent += WindowDeleteEvent;
    }

    internal void ShowLoginForm()
    {
        Clear();
        Form = new LoginForm(App);
        Add(Form.Form);
        ShowAll();
    }

    internal void ShowRegisterForm()
    {
        Clear();
        Form = new RegisterForm(App);
        Add(Form.Form);
        ShowAll();
    }

    private void Clear()
    {
        Form = null;
        foreach (var child in Children)
        {
            Remove(child);
        }
    }
    private void WindowDeleteEvent(object sender, DeleteEventArgs args) { Application.Quit(); }
}