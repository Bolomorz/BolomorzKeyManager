using BolomorzKeyManager.Controller.Auth;
using BolomorzKeyManager.View.Windows;
using Gdk;
using Gtk;

namespace BolomorzKeyManager;

internal class KeyManager : Application
{
    internal Session? _Session { get; set; }
    internal MainWindow Window { get; set; }

    internal readonly Pixbuf ConcealSymbolic;
    internal readonly Pixbuf RevealSymbolic;
    internal readonly Pixbuf Active;
    internal readonly Pixbuf Inactive;

    internal KeyManager() : base("bolomorz.keymanager", GLib.ApplicationFlags.None)
    {
        Init();
        Register(GLib.Cancellable.Current);

        var theme = IconTheme.Default;
        ConcealSymbolic = theme.LoadIcon("view-conceal-symbolic", 25, IconLookupFlags.UseBuiltin | IconLookupFlags.GenericFallback);
        RevealSymbolic = theme.LoadIcon("view-reveal-symbolic", 25, IconLookupFlags.UseBuiltin | IconLookupFlags.GenericFallback);
        Inactive = theme.LoadIcon("process-stop-symbolic", 25, IconLookupFlags.UseBuiltin | IconLookupFlags.GenericFallback);
        Active = theme.LoadIcon("selection-mode-symbolic", 25, IconLookupFlags.UseBuiltin | IconLookupFlags.GenericFallback);

        string css = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "View", "Styles", "styles.css");
        var provider = new CssProvider();
        provider.LoadFromPath(css);
        StyleContext.AddProviderForScreen(Screen.Default, provider, StyleProviderPriority.User);

        Window = new MainWindow(this);
        Window.ShowLogin();

        Run();
    }

}