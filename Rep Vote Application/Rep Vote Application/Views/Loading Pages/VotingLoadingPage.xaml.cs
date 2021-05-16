using Projet_Pfe;
using Projet_Pfe.Views;
using Rep_Vote_Application.Helpers;
using Rep_Vote_Application.Packages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI_SQL.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rep_Vote_Application.Views.Loading_Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VotingLoadingPage : ContentPage
    {
        public VotingLoadingPage(string vote)
        {
            InitializeComponent();
            

            generatevote(vote);

        }

        async void generatevote(string uservote)
        {
            var response=await InitFaceRecog();
            if (response > 0)
            {
                UserVote vote = new UserVote(Getters.CurrentUser.UserId, uservote);
                Getters.Vote = vote.Vote;
                await WebApiConnection.CreateVote(vote);
                var IsClosed = await WebApiConnection.IsVotingRoomClosed();
                if (IsClosed)
                    await Navigation.PushAsync(new Result(), true);
                else
                    await Navigation.PushAsync(new WaitingPage(), true);
            }
            await Navigation.PushAsync(new WaitingPage(), true);
        }
        async Task <double> InitFaceRecog()
        {
            var faceapi = new faceAPI();
            var response = await faceapi.TakeImageAsync();
            return response;
        }
    }
}