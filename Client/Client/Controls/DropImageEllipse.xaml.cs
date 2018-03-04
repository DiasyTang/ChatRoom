using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Client.Controls
{
    public sealed partial class DropImageEllipse : UserControl
    {
        public DropImageEllipse()
        {
            this.InitializeComponent();
        }

        private async void Grid_Drop(object sender, DragEventArgs e)
        {
            var defer = e.GetDeferral();

            try
            {
                DataPackageView dataView = e.DataView;

                if (dataView.Contains(StandardDataFormats.StorageItems))
                {
                    var files = await dataView.GetStorageItemsAsync();
                    StorageFile file = files.OfType<StorageFile>().First();
                    if (file.FileType == ".png" || file.FileType == ".jpg")
                    {
                        //BitmapImage bitmag = new BitmapImage();
                        //await bitmag.SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));
                        //ximg.ImageSource = bitmag;
                        BitmapImage sourceImage = ximg.ImageSource as BitmapImage;
                        await sourceImage.SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));                        
                    }
                }
            }
            finally
            {
                defer.Complete();
            }
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "拖放打开";
            e.Handled = true;
        }
    }
}
