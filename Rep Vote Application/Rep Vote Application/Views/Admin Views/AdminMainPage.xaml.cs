using Projet_Pfe;
using Projet_Pfe.Views;
using Rep_Vote_Application.Helpers;
using Rep_Vote_Application.Packages;
using Rep_Vote_Application.Services;
using Rep_Vote_Application.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rep_Vote_Application.Views.Admin_Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminMainPage : ContentPage
    {
        public AdminMainPage()
        {
            InitializeComponent();
            BindingContext = new faceAPI();

        }
        
        private async void ToVoting(object sender, EventArgs e)
        {
            OnBackButtonPressed();
            CreateSession.IsEnabled = false;
            var IsClosed = await WebApiConnection.IsVotingRoomClosed();
            
            if(IsClosed)
               await PopupNavigation.Instance.PushAsync(new PopUp(), true);
           
            else
            {
                Getters.CurrentVotingSession = await WebApiConnection.GetLastVotingRoom();
                Getters.TheVotedLaw = await WebApiConnection.GetLaw(Getters.CurrentVotingSession.LawId);
                await Navigation.PushAsync(new WaitingPage(), true);
            }
            
            CreateSession.IsEnabled = true;
        }

        protected override bool OnBackButtonPressed() => true;

        private void HistoryBtn_Clicked(object sender, EventArgs e)
        {
            
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            await LocalDBService.DeleteSession(Getters.CurrentUser.UserId);
            await Navigation.PushAsync(new LoginPage(), true);
           
            
            Getters.CurrentUser = null;
        }
    }
}