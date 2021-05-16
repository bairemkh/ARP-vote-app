using Microsoft.AspNetCore.SignalR.Client;
using Projet_Pfe;
using Projet_Pfe.Helpers;
using Projet_Pfe.Views;
using Rep_Vote_Application.Helpers;
using Rep_Vote_Application.Views.Common_Views;
using Rep_Vote_Application.Views.Loading_Pages;
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
            var Action=await DisplayAlert("Confirmation", "Do you confirm your vote ? \n You Voted : Yes", "Yes i confirm my vote", "cancel");
            if (Action) {
                await Navigation.PushAsync(new VotingLoadingPage("Yes"), true);
            }
            
        }
        #endregion

        #region No
        private async void VotedNo_Clicked(object sender, EventArgs e)
        {
            var Action = await DisplayAlert("Confirmation", "Do you confirm your vote ? \n You Voted : No", "Yes i confirm my vote", "cancel");
            if (Action)
            {
                await Navigation.PushAsync(new VotingLoadingPage("No"), true);
            }
        }
        #endregion

        #region Retained
        private async void VotedRetained_Clicked(object sender, EventArgs e)
        {
            var Action = await DisplayAlert("Confirmation", "Do you confirm your vote ? \n You Voted : Retained", "Yes i confirm my vote", "cancel");
            if (Action)
            {
                await Navigation.PushAsync(new VotingLoadingPage("Retained"), true);
            }
        }
        #endregion
       

    }
}