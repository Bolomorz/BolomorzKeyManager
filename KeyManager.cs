using BolomorzKeyManager.Controller.Auth;
using BolomorzKeyManager.View.Windows;
using Gdk;
using Gtk;

namespace BolomorzKeyManager;

internal class KeyManager : Application
{
    internal Session? _Session { get; set; }
    internal MainWindow Window { get; set; }
    internal readonly IconTheme Theme;
    internal KeyManager() : base("bolomorz.keymanager", GLib.ApplicationFlags.None)
    {
        Init();
        Register(GLib.Cancellable.Current);

        Theme = IconTheme.Default;

        string css = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "View", "Styles", "styles.css");
        var provider = new CssProvider();
        provider.LoadFromPath(css);
        StyleContext.AddProviderForScreen(Screen.Default, provider, StyleProviderPriority.User);

        Window = new MainWindow(this);
        Window.ShowLogin();

        Run();
    }

}