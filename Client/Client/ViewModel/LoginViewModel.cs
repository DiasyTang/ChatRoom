using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using SocketBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Client.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public static string MySelfUserName { get; set; }
        public RelayCommand<string> LoginCommand { get; set; }
        private bool IsLogin = false;

        public LoginViewModel()
        {      
            LoginCommand = new RelayCommand<string>(LoginCommand_Execute, (n) => { return !string.IsNullOrWhiteSpace(n); });
            StartServerSocket();
        }


        private async void StartServerSocket()
        {
            App.ServerSocket = SocketFactory.CreateInkSocket(true, GetLocalIp(), "24707");
            App.ServerSocket.OnStartFailed += ServerSocket_OnStartFailed;

            await App.ServerSocket.Start();
        }

        private void ServerSocket_OnStartFailed(Exception obj)
        {
            throw new NotImplementedException();
        }

        private async void LoginCommand_Execute(string name)
        {
            MySelfUserName = name;

            App.ClientSocket = SocketFactory.CreateInkSocket(false, GetLocalIp(), "24707") as ClientSocket;
            App.ClientSocket.OnStartFailed += ClientSocket_OnStartFailed;
            App.ClientSocket.OnStartSucess += ClientSocket_OnStartSucess;

            await App.ClientSocket.Start();

            if (IsLogin)
            {
                var navigationService = ServiceLocator.Current.GetInstance<NavigationService>();
                navigationService.NavigateTo("MainPage");
                (App.ClientSocket as ClientSocket).ReceiveData();
            }

        }

        private void ClientSocket_OnStartSucess()
        {
            IsLogin = true;
        }

        private void ClientSocket_OnStartFailed(Exception e)
        {
            throw e.InnerException;
        }

        private string GetLocalIp()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();
            var hostname = NetworkInformation.GetHostNames()
                .SingleOrDefault(
                h => h.IPInformation?.NetworkAdapter != null && h.IPInformation.NetworkAdapter.NetworkAdapterId
                == icp.NetworkAdapter.NetworkAdapterId);

            return hostname?.CanonicalName;
        }
    }
}
