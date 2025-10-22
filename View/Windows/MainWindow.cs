using BolomorzKeyManager.View.Widgets;
using Gtk;

namespace BolomorzKeyManager.View.Windows;

internal class MainWindow : Window
{
    private readonly KeyManager App;
    private KMGrid? Container;
    
    internal MainWindow(KeyManager app) : base("Bolomorz Key Manager")
    {
        App = app;
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit();  };
    }

    internal void ShowLogin()
    {
        Clear();
        Container = new KMLogin(App);
        Add(Container);
        ShowAll();
    }

    internal void ShowRegister()
    {
        Clear();
        Container = new KMRegister(App);
        Add(Container);
        ShowAll();
    }

    internal void ShowMain()
    {
        Clear();
        Container = new KMMain(App);
        Add(Container);
        ShowAll();
    }

    private void Clear()
    {
        if(Container is not null)
        {
            Remove(Container);
            Container.Destroy();
            Container = null;
        }
    }
    
}