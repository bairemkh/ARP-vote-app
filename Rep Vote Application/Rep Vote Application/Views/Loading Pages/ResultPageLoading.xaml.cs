using Projet_Pfe.Views;
using Rep_Vote_Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI_SQL.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rep_Vote_Application.Views.Common_Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPageLoading : ContentPage
    {
        public ResultPageLoading()
        {
            InitializeComponent();
            Loading();
            
        }
         async void Loading()
        {
            await GetResult();
            await Navigation.PushAsync(new Result());
        }

        async Task GetResult()
        {
            Getters.sessionResult = await WebApiConnection.GetVoteResult();
            var uservote = await WebApiConnection.GetVote(Getters.CurrentUser.UserId);
            Getters.Vote = uservote.Vote;
            Getters.CurrentVotingSession = await WebApiConnection.GetLastVotingRoom();
            Getters.TheVotedLaw = await WebApiConnection.GetLaw(Getters.CurrentVotingSession.LawId);
            
        }
        protected override bool OnBackButtonPressed() => true;
    }
}