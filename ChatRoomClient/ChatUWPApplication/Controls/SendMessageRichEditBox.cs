using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ChatUWPApplication.Controls
{
     public class SendMessageRichEditBox:RichEditBox
    {
        public static readonly DependencyProperty SendMessageCommandProperty = DependencyProperty.Register(
            "SendMessageCommand", typeof(ICommand), typeof(SendMessageRichEditBox), new PropertyMetadata(null));
        public ICommand SendMessageCommand
        {
            get => GetValue(SendMessageCommandProperty) as ICommand;
            set => SetValue(SendMessageCommandProperty, value);
        }
        public SendMessageRichEditBox()
        {
            this.KeyUp += SendMessageRichEditBox_KeyUp;
        }

        private void SendMessageRichEditBox_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                string text;
                var box = sender as RichEditBox;
                box.Document.GetText(Windows.UI.Text.TextGetOptions.UseObjectText, out text);
                SendMessageCommand?.Execute(text);
                box.Document.SetText(Windows.UI.Text.TextSetOptions.None, String.Empty);
            }
        }
    }
}
