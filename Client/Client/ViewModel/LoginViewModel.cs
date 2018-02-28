using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using SocketBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public static string MySelfUserName { get; set; }
        public RelayCommand<string> LoginCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<string>(LoginCommand_Execute, (n) => { return !string.IsNullOrWhiteSpace(n); });
        }

        private async void LoginCommand_Execute(string name)
        {
            MySelfUserName = name;

            App.ClientSocket = SocketFactory.CreateInkSocket(false, "127.0.0.1", "8080") as ClientSocket;
            App.ClientSocket.OnStartFailed += ClientSocket_OnStartFailed;
            App.ClientSocket.OnStartSucess += ClientSocket_OnStartSucess;

            await App.ClientSocket.Start();

        }

        private void ClientSocket_OnStartSucess()
        {
            var navigationService = ServiceLocator.Current.GetInstance<NavigationService>();
            navigationService.NavigateTo("MainPage");
        }

        private void ClientSocket_OnStartFailed(Exception e)
        {
            throw new NotImplementedException();
        }
    }
}
