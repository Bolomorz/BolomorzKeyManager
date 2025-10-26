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

    internal static string? InputDialog(string message, KeyManager app)
    {
        InputDialog dialog = new(message, app);
        var text = dialog.Run();
        dialog.Destroy();
        return text;
    }

    internal static void OutputDialog(string name, string decrypt, KeyManager app)
    {
        OutputDialog dialog = new(name, decrypt, app);
        dialog.Run();
        dialog.Destroy();
    }

    internal static ResponseType ConfirmDialog(string message, MainWindow window)
    {
        MessageDialog dialog = new(window, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, message);
        ResponseType response = (ResponseType)dialog.Run();
        dialog.Destroy();
        return response;
    }
    
}