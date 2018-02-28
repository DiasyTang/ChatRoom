using SocketBusiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Server
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            App.ServerSocket = SocketFactory.CreateInkSocket(true, GetLocalIp(), "8080");
            App.ServerSocket.OnStartFailed += ServerSocket_OnStartFailed;
            await App.ServerSocket.Start();
        }

        private void ServerSocket_OnStartFailed(Exception e)
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
