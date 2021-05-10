using Rep_Vote_Application.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WebAPI_SQL.Entities;

namespace Rep_Vote_Application.ViewModels
{
    class VoteViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string vote = Getters.Vote;
        VoteSessionResult sessionResult = Getters.sessionResult;
        public string Vote { get => vote;
            set
            {
                if (value == null)
                    return;
                vote = value;
                OnPropertyChange(nameof(Vote));
            }
        }
        public VoteSessionResult SessionResult
        {
            get => sessionResult;
            set
            {
                if (value == null)
                    return;
                sessionResult = value;
                OnPropertyChange(nameof(SessionResult));
            }
        }
        
        
        void OnPropertyChange(String name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
