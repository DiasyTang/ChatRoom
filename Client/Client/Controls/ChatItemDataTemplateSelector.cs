using Client.Model;
using Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Client.Controls
{
    public class ChatItemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LeftDataTemplate { get; set; }
        public DataTemplate RightDataTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if ((item as ChatRecord).UserName.Equals(LoginViewModel.MySelfUserName))
            {
                return LeftDataTemplate;
            }
            else
            {
                return RightDataTemplate;
            }
        }
    }
}
