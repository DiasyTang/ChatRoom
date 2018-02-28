using System;
using ChatUWPApplication.Model;
using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Windows.UI.Xaml.Controls;

namespace ChatUWPApplication.ViewModel
{
    public class MainViewModel:ViewModelBase
    {
        public static string MySelfUserName { get; set; }
        public RelayCommand GoToSettingsPageCommand { get; set; }
        public RelayCommand GoToMessagePageCommand { get; set; }

        private ViewModelBase currenViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => currenViewModel;
            set
            {
                if (currenViewModel != value)
                {
                   currenViewModel = value;
                   RaisePropertyChanged("CurrentViewModel");
                }
            }
        }

        public MainViewModel()
        {
            GoToMessagePageCommand = new RelayCommand(GoToMessagePageCommand_Execute);
            GoToSettingsPageCommand = new RelayCommand(GoToSettingsPageCommand_Execute);
        }

        private void GoToSettingsPageCommand_Execute()
        {
            var subnavigationservice = ServiceLocator.Current.GetInstance(typeof(SubNavigationService)) as SubNavigationService;
            subnavigationservice.NavigateTo("Setting");
        }

        private void GoToMessagePageCommand_Execute()
        {
            var subnavigationservice = ServiceLocator.Current.GetInstance(typeof(SubNavigationService)) as SubNavigationService;
            subnavigationservice.NavigateTo("Message");
        }
    }
}