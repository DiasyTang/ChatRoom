using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace ChatUWPApplication.Model
{
    public class ContactDetailInfo:ViewModelBase
    {
        private ObservableCollection<ChatRecord> chatRecordCollection;
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

        public ObservableCollection<ChatRecord> ChatRecordCollection
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