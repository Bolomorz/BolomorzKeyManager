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
}