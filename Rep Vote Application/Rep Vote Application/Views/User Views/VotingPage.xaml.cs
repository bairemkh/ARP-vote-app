using Microsoft.AspNetCore.SignalR.Client;
using Projet_Pfe;
using Projet_Pfe.Helpers;
using Projet_Pfe.Views;
using Rep_Vote_Application.Helpers;
using Rep_Vote_Application.Views.Common_Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI_SQL.Entities;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rep_Vote_Application.Views.Admin_Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VotingPage : ContentPage
    {
        public VotingPage()
        {
            InitializeComponent();
            #region other pages step 1: Timer Connection
            Connect();
            VoteTimer.hubConnection.On<int, int, bool>("Timer", (minutes, seconds, TimeState) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Timer.Text = $"{minutes}m : {seconds}s";
                });
                #region Action when the time is up
                if (TimeState)
                {
                    close();
                    Navigation.PushAsync(new ResultPageLoading());
                    VoteTimer.hubConnection.StopAsync();
                }
                #endregion


            });
            #endregion
        }
        protected override bool OnBackButtonPressed() => true;
        #region other pages Hub Connection
        private async void Connect()
        {
            if (VoteTimer.hubConnection.State == HubConnectionState.Disconnected)
                await VoteTimer.hubConnection.StartAsync();
        }
        #endregion
        async void close()
        {
            await WebApiConnection.CloseVotingRoom();
        }
        #region Yes
        private async void VotedYes_Clicked(object sender, EventArgs e)
        {
            UserVote vote = new UserVote(Getters.CurrentUser.UserId, "Yes");
            Getters.Vote = vote.Vote;
          await WebApiConnection.CreateVote(vote);
            var IsClosed = await WebApiConnection.IsVotingRoomClosed();
            if (IsClosed)
                await Navigation.PushAsync(new Result(), true);
            else
                await Navigation.PushAsync(new WaitingPage(), true);
        }
        #endregion

        #region No
        private async void VotedNo_Clicked(object sender, EventArgs e)
        {

            UserVote vote = new UserVote(Getters.CurrentUser.UserId, "No");
            Getters.Vote = vote.Vote;
            await WebApiConnection.CreateVote(vote);
            var IsClosed = await WebApiConnection.IsVotingRoomClosed();
            if (IsClosed)
                await Navigation.PushAsync(new Result(), true);
            else
                await Navigation.PushAsync(new WaitingPage(), true);
        }
        #endregion

        #region Retained
        private async void VotedRetained_Clicked(object sender, EventArgs e)
        {

            UserVote vote = new UserVote(Getters.CurrentUser.UserId, "Retained");
            Getters.Vote = vote.Vote;
            await WebApiConnection.CreateVote(vote);
            var IsClosed = await WebApiConnection.IsVotingRoomClosed();
            if (IsClosed)
                await Navigation.PushAsync(new Result(), true);
            else
                await Navigation.PushAsync(new WaitingPage(), true);
        }
        #endregion
       

    }
}