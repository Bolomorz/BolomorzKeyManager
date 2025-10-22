using BolomorzKeyManager.Controller.Data;
using BolomorzKeyManager.View.Windows;
using Gtk;

namespace BolomorzKeyManager.View;

internal static class Dialog
{
    internal static void ErrorDialog(Message message, MainWindow window)
    {
        MessageDialog dialog = new(window, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, message.Error);
        dialog.Run();
        dialog.Destroy();
    }

    internal static void InfoDialog(Message message, MainWindow window)
    {
        MessageDialog dialog = new(window, DialogFlags.Modal, MessageType.Info, ButtonsType.Close, message.Error);
        dialog.Run();
        dialog.Destroy();
    }

    internal static string? InputDialog(string message, MainWindow window)
    {
        InputDialog dialog = new(message, window);
        var text = dialog.Run();
        dialog.Destroy();
        return text;
    }
    
}