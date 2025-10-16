using Gtk;

namespace BolomorzKeyManager.View.Widgets;

internal abstract class KMGrid(KeyManager app) : Grid()
{
    protected readonly KeyManager App = app;
}