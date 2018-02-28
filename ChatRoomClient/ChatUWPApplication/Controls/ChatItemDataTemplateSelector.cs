using ChatUWPApplication.Model;
using ChatUWPApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ChatUWPApplication.Controls
{
    public class ChatItemDataTemplateSelector:DataTemplateSelector
    {
        public DataTemplate LeftDataTemplate { get; set; }
        public DataTemplate RightDataTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if((item as ChatRecord).UserName.Equals(MainViewModel.MySelfUserName))
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
