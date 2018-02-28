using ChatUWPApplication.View;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatUWPApplication.Model
{
    public class SubNavigationService : INavigationService
    {
        public const string RootPageKey = "-- Root --";

        public const string UnknownPageKey = "-- UNKNOWN --";

        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        public string CurrentPageKey
        {
            get
            {
                lock (_pagesByKey)
                {
                    var frame = MainView.MView.MainFrame;

                    if (frame.BackStackDepth == 0)
                    {
                        return RootPageKey;
                    }

                    if (frame.Content == null)
                    {
                        return UnknownPageKey;
                    }

                    var currentType = frame.Content.GetType();

                    if (_pagesByKey.All(p => p.Value != currentType))
                    {
                        return UnknownPageKey;
                    }

                    var item = _pagesByKey.FirstOrDefault(i => i.Value == currentType);

                    return item.Key;
                }
            }
        }

        public void GoBack()
        {
            var frame = MainView.MView.MainFrame;

            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            lock (_pagesByKey)
            {
                if (!_pagesByKey.ContainsKey(pageKey))
                {
                    throw new ArgumentException(
                        string.Format("No such page: {0}. Did you forget to call NavigationService.Configure?", 
                        pageKey),"pageKey");
                }

                var frame = MainView.MView.MainFrame;
                frame.Navigate(_pagesByKey[pageKey], parameter);
            }
        }

        public void Configure(string key,Type pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(key))
                {
                    throw new ArgumentException("This key is already used: " + key);
                }

                if (_pagesByKey.Any(p => p.Value == pageType))
                {
                    throw new ArgumentException(
                        "This type is already configured with key " + _pagesByKey.FirstOrDefault(p => p.Value == pageType).Key);
                }

                _pagesByKey.Add(key, pageType);
            }
        }
    }
}
