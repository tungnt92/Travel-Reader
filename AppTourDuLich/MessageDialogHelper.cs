using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace AppTourDuLich
{
    class MessageDialogHelper
    {
        public async void Show(string content, string title)
        {
            MessageDialog messageDialog = new MessageDialog(content, title);
            messageDialog.Commands.Add(new UICommand("reload", new UICommandInvokedHandler(CommandHandlers)));
            messageDialog.Commands.Add(new UICommand("Quit", new UICommandInvokedHandler(CommandHandlers)));
            await messageDialog.ShowAsync();
        }
        public async void CommandHandlers(IUICommand commandLabel)
        {
            var Actions = commandLabel.Label;
            switch (Actions)
            {
                case "Reload":
                    //Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-notifications://"));
                    break;
                case "Quit":
                    Application.Current.Exit();
                    break;
                //end.
            }
        }
    }
}
