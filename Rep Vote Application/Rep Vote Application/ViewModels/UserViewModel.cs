using Rep_Vote_Application.Helpers;
using Rep_Vote_Application.Local_DB_Entities;
using Rep_Vote_Application.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using WebAPI_SQL.Entities;
using Xamarin.Forms;

namespace Rep_Vote_Application.ViewModels
{
    class UserViewModel:INotifyPropertyChanged
    {
        string userid= Getters.CurrentUser.UserId;
       
        string username=Getters.CurrentUser.UserName;
        string userlastname= Getters.CurrentUser.UserLastName;
        string usertype = Getters.CurrentUser.UserType;
        string userpoliticalparty = Getters.CurrentUser.UserPoliticalParty;
        ImageSource userimage=ImageSource.FromStream(() => new MemoryStream(Getters.CurrentUser.UserImage));

        public string UserId {
            get { return userid; }
            set {
                if (value != null)
                {
                    userid = value;
                    OnPropertyChange(nameof(UserId));
                }
            } }
        
        public string UserName
        {
            get { return username; }
            set
            {
                if (value != null)
                {
                    username = value;
                    OnPropertyChange(nameof(UserName));
                }
            }
        }
        public string UserLastName
        {
            get { return userlastname; }
            set
            {
                if (value != null)
                {
                    userlastname = value;
                    OnPropertyChange(nameof(UserLastName));
                }
            }
        }
        public string UserType
        {
            get { return usertype; }
            set
            {
                if (value != null)
                {
                    usertype = value;
                    OnPropertyChange(nameof(UserType));
                }
            }
        }
        public string UserPoliticalParty
        {
            get { return userpoliticalparty; }
            set
            {
                if (value != null)
                {
                    userpoliticalparty = value;
                    OnPropertyChange(nameof(UserPoliticalParty));
                }
            }
        }

        public ImageSource UserImage
        {
            get { return userimage; }
            set
            {
                if (value != null)
                {
                    userimage = value;
                    OnPropertyChange(nameof(UserImage));
                }
            }
        }
        public string NameAndLastName
        {
            get => $"{UserName} {UserLastName}";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChange(String name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
