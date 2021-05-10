using Microsoft.AspNetCore.SignalR.Client;
using Projet_Pfe.Helpers;
using Rep_Vote_Application.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Rep_Vote_Application.ViewModels
{
    class VotingRoomViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string votesubject = Getters.CurrentVotingSession.VoteSubject;
        public string VoteSubject {
        get {return votesubject; }
        set {
                if (value != null)
                {
                    votesubject = value;
                    OnPropertyChange(nameof(VoteSubject));
                }
        } }

        int votedlawnumber = Getters.TheVotedLaw.LawNumber;
        public int VotedLawNumber
        {
            get { return votedlawnumber; }
            set
            {
               
                    votedlawnumber = value;
                    OnPropertyChange(nameof(VotedLawNumber));
                
            }
        }

        int votedchapternumber = Getters.TheVotedLaw.LawChapter;
        public int VotedChapterNumber
        {
            get { return votedchapternumber; }
            set
            {

                votedchapternumber = value;
                OnPropertyChange(nameof(VotedChapterNumber));

            }
        }

        string lawdesc = Getters.TheVotedLaw.LawDesc;
        public string LawDesc
        {
            get { return lawdesc; }
            set
            {
                if (value != null)
                {
                    lawdesc = value;
                    OnPropertyChange(nameof(LawDesc));
                }
            }
        }
        string timeleft;
        public string TimeLeft
        {
            get => timeleft;
            set
            {
                Connect();
                VoteTimer.hubConnection.On<int, int, bool>("Timer", (minutes, seconds, TimeState) =>
                {
                    timeleft = $"{minutes}:{seconds}";
                    OnPropertyChange(nameof(TimeLeft));
                });
            }
        }

        #region other pages Hub Connection
        private async void Connect()
        {
            if (VoteTimer.hubConnection.State == HubConnectionState.Disconnected)
                await VoteTimer.hubConnection.StartAsync();
        }
        #endregion
        void OnPropertyChange(String name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
