using Gtk;
using BolomorzKeyManager.Controller.Data;

namespace BolomorzKeyManager.View.Widgets;

internal class KMWelcomeView : KMGrid
{
    private readonly KMMain Main;

    internal KMWelcomeView(KeyManager app, KMMain main) : base(app)
    {
        Main = main;

        var heading = new Label("KeyManager by @Bolomorz");
        Attach(heading, 0, 0, 3, 1);
    }
}