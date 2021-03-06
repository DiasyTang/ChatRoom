﻿using Client.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SocketBusiness;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.ViewModel
{
    public class MessageViewModel : ViewModelBase
    {
        //private ContactDetailInfo selectedContact;

        public ICommand SendMessageCommand { get; set; }
        //public ContactDetailInfo SelectedContact
        //{
        //    get => selectedContact;
        //    set
        //    {
        //        if (selectedContact != value)
        //        {
        //            selectedContact = value;
        //            RaisePropertyChanged(nameof(SelectedContact));
        //        }
        //    }
        //}
        //public ObservableCollection<ContactDetailInfo> ContactCollection { get; set; }

        public ObservableCollection<MessageModel> ChatRecordCollection { get; set; } = new ObservableCollection<MessageModel>();
       
        public MessageViewModel()
        {
            SendMessageCommand = new RelayCommand<string>(SendMessageCommand_Execute);

            //ChatRecordCollection = new ObservableCollection<ChatRecord>()
            //{
            //    new ChatRecord(){ UserName="kdd",Comment="djkf"},
            //    new ChatRecord(){ UserName="df",Comment="djkf"},
            //    new ChatRecord(){ UserName="fff",Comment="djkf"},
            //    new ChatRecord(){ UserName="dafad",Comment="djkf"},
            //};
            //ContactCollection = new ObservableCollection<ContactDetailInfo>()
            //{
            //   new ContactDetailInfo(){UserName="kdd",ChatRecordCollection=ChatRecordCollection},
            //   new ContactDetailInfo(){UserName="df",ChatRecordCollection=ChatRecordCollection},
            //   new ContactDetailInfo(){UserName="fff",ChatRecordCollection=ChatRecordCollection},
            //   new ContactDetailInfo(){UserName="kasdfdd",ChatRecordCollection=ChatRecordCollection},
            //};
        }

        private async void SendMessageCommand_Execute(string text)
        {
            var msg = new SocketBusiness.MessageModel()
            {
                Published = DateTime.Now,
                Sender = LoginViewModel.MySelfUserName,
                Content = text
            };
            await App.ClientSocket.SendMsg(msg);
            ChatRecordCollection.Add(msg);
        }
    }
}
