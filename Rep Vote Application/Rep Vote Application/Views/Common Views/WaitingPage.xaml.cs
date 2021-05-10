using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Projet_Pfe.Views;
using Rep_Vote_Application.Helpers;
using Rep_Vote_Application.Services;
using Rep_Vote_Application.Views.Admin_Views;
using Rep_Vote_Application.Views.Common_Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAPI_SQL.Entities;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projet_Pfe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WaitingPage : ContentPage
    {

       
        public WaitingPage()
        {
            InitializeComponent();

            var user = Getters.CurrentUser;
            #region User Avatar
            /*  if (user.UserImage != null)
            {

                UserAvatar.Source = ImageSource.FromStream(() => new MemoryStream(user.UserImage));


            }*/
            #endregion
            if (user.UserType.ToLower().Equals("admin"))
                Vote.IsVisible = false;

            #region other pages step 1: Timer Connection
            Connect();
            Helpers.VoteTimer.hubConnection.On<int, int, bool>("Timer", (minutes, seconds, TimeState) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    TimeRemaining.Text = $"{minutes}m : {seconds}s";

                });
                #region Action when the time is up
                if (TimeState)
                {
                    
                    close();

                    Navigation.PushAsync(new ResultPageLoading());

                    Helpers.VoteTimer.hubConnection.StopAsync();
                    
                }
                #endregion


            });
            #endregion



        }
        protected override bool OnBackButtonPressed()
        {
            if (Getters.CurrentUser.UserType.ToLower().Equals("user"))
                Navigation.PushAsync(new UserMainPage());
            else
                Navigation.PushAsync(new AdminMainPage());
            return true;
        }

        #region other pages Hub Connection
        private async void Connect()
        {
            if (Helpers.VoteTimer.hubConnection.State == HubConnectionState.Disconnected)
                await Helpers.VoteTimer.hubConnection.StartAsync();
        }
        #endregion
        async void close()
        {
            await WebApiConnection.CloseVotingRoom();
        }
       
        
    }

}