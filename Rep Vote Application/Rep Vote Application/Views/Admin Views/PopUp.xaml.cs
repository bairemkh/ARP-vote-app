using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Rep_Vote_Application.Helpers;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebAPI_SQL.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Projet_Pfe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    
    public partial class PopUp : Rg.Plugins.Popup.Pages.PopupPage
    {
    
        public VotingRoom voteroom;
        public Law LawVoted;
        public User user;
       

        #region Popup methode
        public PopUp()
        {
            InitializeComponent();
            IfPlannedSession.IsVisible = false;
            
            
            ConfirmBtn.IsEnabled = false;
            DisplayDuration.Text = "Vote Duration: " + DurationStepper.Value + " Minutes";
            AreEnabled(false);
            #region step 1: main page elements 
            Connect();
            #endregion

        }
        #endregion

        #region form events
        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            //voteroom.IsPlanned = e.Value;
            IfPlannedSession.IsVisible = e.Value;

        }
        #region Law Getter
        private async void Assign(string lawChapterS, string lawNumberS)
        {

           
            try
            {
                var law = await WebApiConnection.GetLaw(lawChapterS, lawNumberS);
                #region law found
                Getters.TheVotedLaw = law;
                DisplayLaw.Text = law.LawDesc;
                LawVoted = law;
                AreEnabled(true);
                #endregion

            }
            #region law not found
            catch (Exception)
            {
                DisplayLaw.Text = "Law Not Found";

                AreEnabled(false);
            }
            #endregion
        }
        #endregion

        #region ennable or disable elements methode
        private void AreEnabled(bool enable)
        {
            VoteSubject.IsEnabled = enable;
            PlannedCheckBox.IsEnabled = enable;
            DurationStepper.IsEnabled = enable;
        }

        #endregion

        #region Btn Enabled Conditions
        public bool EnableBtn()
        {
            return ((VoteSubject.Text.Length > 5) && (LawChapterVal.Text.Length > 0) && (LawNumberVal.Text.Length > 0));

        }

        #endregion

        private void CancelBtn_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAllAsync(true);
        }

        private async void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            ConfirmBtn.IsEnabled = false;


            VotingRoom votingroom;
            if (PlannedCheckBox.IsChecked)
            {
                CreateSession(Math.Round(DurationStepper.Value), DatePlanned.Date, TimePlanned.Time, VoteSubject.Text, LawVoted.LawId);
                votingroom = new VotingRoom(Convert.ToInt32(DurationStepper.Value), DatePlanned.Date.ToString(), TimePlanned.Time.ToString(), VoteSubject.Text, LawVoted.LawId, "opened");


            }
            else
            { CreateSession(Math.Round(DurationStepper.Value), VoteSubject.Text, LawVoted.LawId);
            votingroom = new VotingRoom(Convert.ToInt32(DurationStepper.Value),"", "", VoteSubject.Text, LawVoted.LawId, "opened");
            }

            try
            {

                if (Helpers.VoteTimer.hubConnection.State == HubConnectionState.Connected)
                {
                    await Helpers.VoteTimer.hubConnection.SendAsync("RaiseTimer", Convert.ToInt32(DurationStepper.Value));
                    
                }
                

            }
            catch (Exception ex)
            {
               await DisplayAlert("error", ex.ToString(), "okay");
            }

            Getters.CurrentVotingSession = votingroom;
            Getters.TheVotedLaw = await WebApiConnection.GetLaw(votingroom.LawId);
            await Navigation.PushAsync(new WaitingPage());

            await  PopupNavigation.Instance.PopAllAsync(true);

            ConfirmBtn.IsEnabled = true;



        }
        #region step 2: first page Hub Connection
        private async void Connect()
        {

            await Helpers.VoteTimer.hubConnection.StartAsync();
        }
        #endregion


        private async void CreateSession(double duration, string voteSubject, string lawId)
        {
            var request = new HttpClient();
            VotingRoom votingRoom;
            string ApiRoutURL = "https://voteapplicationwebapi.azurewebsites.net/";
            try
            {

               
                votingRoom = new VotingRoom(Convert.ToInt32(duration.ToString()), string.Empty, string.Empty, voteSubject, lawId,"opened");
                

               
                var jsonContent = JsonConvert.SerializeObject(votingRoom);

               
                var content = new StringContent(jsonContent);

                
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await request.PostAsync(ApiRoutURL + $"api/VotingRoom/CreateVotingSession", content);
               
                var resContent = await response.Content.ReadAsStringAsync();

               
                var resUser = JsonConvert.DeserializeObject<string>(resContent);



                Getters.CurrentVotingSession = votingRoom;
                Getters.TheVotedLaw = await WebApiConnection.GetLaw(lawId);
            }
            catch (Exception )
            {
                
            }
        }

        private async void CreateSession(double duration, DateTime date, TimeSpan time, string voteSubject, string lawId)
        {
            var request = new HttpClient();
            VotingRoom votingRoom;
            string ApiRoutURL = "https://voteapplicationwebapi.azurewebsites.net/";
            try
            {

                
               votingRoom = new VotingRoom(Convert.ToInt32(duration.ToString()), date.ToString("D"), time.ToString().Substring(0,5), voteSubject,lawId,"opened"); 
               

               
                var jsonContent = JsonConvert.SerializeObject(votingRoom);
                 
                
                var content = new StringContent(jsonContent);

              
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await request.PostAsync(ApiRoutURL + $"api/VotingRoom/CreateVotingSession", content);
               
                var resContent = await response.Content.ReadAsStringAsync();

               
                var resUser = JsonConvert.DeserializeObject<string>(resContent);

                Getters.CurrentVotingSession= votingRoom;
                Getters.TheVotedLaw = await WebApiConnection.GetLaw(lawId);

            }
            catch (Exception )
            {
              
            }


        }

        protected override bool OnBackButtonPressed() => true;
        private void LawChapterVal_TextChanged(object sender, TextChangedEventArgs e)
        {
            Assign(e.NewTextValue, LawNumberVal.Text);
        }

        private void LawNumberVal_TextChanged(object sender, TextChangedEventArgs e)
        {
            Assign(LawChapterVal.Text, e.NewTextValue);
        }
        private void VoteSubject_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConfirmBtn.IsEnabled = EnableBtn();
        }
        private void DurationStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            DisplayDuration.Text = "Vote Duration: "+e.NewValue.ToString()+" Minutes";
        }

        #endregion

      

       
    }
}