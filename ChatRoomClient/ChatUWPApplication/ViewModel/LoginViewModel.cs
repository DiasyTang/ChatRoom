using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ChatUWPApplication.ViewModel
{
    public class LoginViewModel:ViewModelBase
    {
        public RelayCommand<string> LoginCommand { get; set; }
   
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<string>(LoginCommand_Execute, (n) => { return !string.IsNullOrWhiteSpace(n); });
        }

        private void LoginCommand_Execute(string name)
        {
            var navigationService = ServiceLocator.Current.GetInstance<NavigationService>();
            navigationService.NavigateTo("MainPage",name);
        }
        
    }
}
