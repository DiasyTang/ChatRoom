using GalaSoft.MvvmLight;
using SocketBusiness;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model
{
    public class ContactDetailInfo : ViewModelBase
    {
        private ObservableCollection<MessageModel> chatRecordCollection;
        private string userName;
        public string UserName
        {
            get => userName;
            set
            {
                if (userName != value)
                {
                    userName = value;
                    RaisePropertyChanged(nameof(UserName));
                }
            }
        }
        public string Logo { get; set; }
        public string LastChatRecord { get; set; }

        public ObservableCollection<MessageModel> ChatRecordCollection
        {
            get => chatRecordCollection;
            set
            {
                if (chatRecordCollection != value)
                {
                    chatRecordCollection = value;
                    RaisePropertyChanged(nameof(ChatRecordCollection));
                }
            }
        }
    }
}
