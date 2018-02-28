using ChatUWPApplication.Model;
using ChatUWPApplication.View;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUWPApplication.ViewModel
{
    public  class ViewModelLocator
    {

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<MessageViewModel>();
            SimpleIoc.Default.Register<SettingViewModel>();


            SimpleIoc.Default.Register(()=> this.CreateSubNavigationService());
            SimpleIoc.Default.Register(() => this.CreateNavigationService());
        }

        private SubNavigationService CreateSubNavigationService()
        {
            var subnavigationService = new SubNavigationService();
            subnavigationService.Configure("Message", typeof(MessageView));
            subnavigationService.Configure("Setting", typeof(SettingView));

            return subnavigationService;
        }

        private NavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure("MainPage", typeof(MainView));
            navigationService.Configure("LoginPage", typeof(LoginView));

            return navigationService;
        }
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public LoginViewModel Login => ServiceLocator.Current.GetInstance<LoginViewModel>();
        public MessageViewModel Message => ServiceLocator.Current.GetInstance<MessageViewModel>();

        public SettingViewModel Setting => ServiceLocator.Current.GetInstance<SettingViewModel>();
    }
}
