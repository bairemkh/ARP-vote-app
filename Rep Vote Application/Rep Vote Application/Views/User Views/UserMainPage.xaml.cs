using Microsoft.AspNetCore.SignalR.Client;
using Projet_Pfe;
using Projet_Pfe.Helpers;
using Projet_Pfe.Views;
using Rep_Vote_Application.Helpers;
using Rep_Vote_Application.Services;
using Rep_Vote_Application.Views.Common_Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rep_Vote_Application.Views.Admin_Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserMainPage : ContentPage
    {
        public UserMainPage()
        {
            InitializeComponent();
            init();
        }
       

        private async void LogoutBtn_Clicked(object sender, EventArgs e)
        {
            await LocalDBService.DeleteSession(Getters.CurrentUser.UserId);
            await Navigation.PushAsync(new LoginPage(), true);


            Getters.CurrentUser = null;
        }
        #region To Voting
        private async void JoinRoomBtn_Clicked(object sender, EventArgs e)
        {
            JoinRoomBtn.IsVisible = false;
            LoadingJoinBtn.IsVisible = true;
            var IsClosed = await WebApiConnection.IsVotingRoomClosed();
            if (!IsClosed)
            {
                var vote = await WebApiConnection.GetVote(Getters.CurrentUser.UserId);
                if (vote.Vote.ToLower().Equals("didn't vote")) {
                    Getters.CurrentVotingSession=await WebApiConnection.GetLastVotingRoom();
                    Getters.TheVotedLaw = await WebApiConnection.GetLaw(Getters.CurrentVotingSession.LawId);
                    await Navigation.PushAsync(new VotingPage());
                }
                else
                {
                    Getters.CurrentVotingSession = await WebApiConnection.GetLastVotingRoom();
                    Getters.TheVotedLaw = await WebApiConnection.GetLaw(Getters.CurrentVotingSession.LawId);
                    var uservote = await WebApiConnection.GetVote(Getters.CurrentUser.UserId);
                    Getters.Vote = uservote.Vote;
                    Getters.sessionResult = await WebApiConnection.GetVoteResult();
                    await Navigation.PushAsync(new WaitingPage());
                }
                    

            }
            else
            {
                
                
                await Navigation.PushAsync(new ResultPageLoading());
                JoinRoomBtn.IsVisible = true;
                LoadingJoinBtn.IsVisible = false;
            }

        }
        #endregion
        async void init()
        {
          var IsClosed=await WebApiConnection.IsVotingRoomClosed();
            if(IsClosed)
            {
                IsActif.Text = "There Is No Voting Session Around";
                IsActif.TextColor = Color.Red;
                JoinRoomBtn.IsEnabled = true;
                IsActif.IsVisible = true;
                JoinRoomBtn.Text = "See Last Vote Result";
               
                AppTimer.Text = "You Can See the last vote session result";
            }
            else
            {
                IsActif.Text = "Session Actif";
                JoinRoomBtn.Text = "Join Room";
                IsActif.IsVisible = true;
                JoinRoomBtn.IsEnabled = true;
                IsActif.TextColor = Color.Green;
                #region other pages step 1: Timer Connection
                Connect();
                VoteTimer.hubConnection.On<int, int, bool>("Timer", (minutes, seconds, TimeState) =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        AppTimer.Text = $"{minutes}m : {seconds}s";
                    });
                    #region Action when the time is up
                    if (TimeState)
                    {
                        close();
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            IsActif.Text = "There Is No Voting Session Around";
                            IsActif.TextColor = Color.Red;
                            JoinRoomBtn.IsEnabled = true;
                            IsActif.IsVisible = true;
                            JoinRoomBtn.Text = "See Last Vote Result";
                            
                            AppTimer.Text = "You Can See the last vote session result";
                        });                        
                        VoteTimer.hubConnection.StopAsync();
                    }
                    #endregion


                });
                #endregion
            }
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
    }
}