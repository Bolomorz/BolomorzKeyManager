using Gtk;
using BolomorzKeyManager.Controller.Data;
using System.Text;

namespace BolomorzKeyManager.View.Widgets;

internal class KMWelcomeView : KMGrid
{
    private readonly KMMain Main;

    internal KMWelcomeView(KeyManager app, KMMain main) : base(app)
    {
        Main = main;

        var heading = new Label("KeyManager by @Bolomorz");
        Attach(heading, 0, 0, 3, 1);

        if(app._Session is not null)
        {
            var rde = app._Session.Encrypt("test", "mpwd");
            if(rde.ReturnValue is not null)
            {
                var rdd = app._Session.Decrypt(rde.ReturnValue, "mpwd");
                Attach(new Label($"Decrypt: {rdd.ReturnValue}"), 0, 2, 3, 1);
            }
        }
    }
}