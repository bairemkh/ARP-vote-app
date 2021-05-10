using Rep_Vote_Application.Helpers;
using Rep_Vote_Application.Local_DB_Entities;
using Rep_Vote_Application.Services;
using Rep_Vote_Application.Views.Admin_Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rep_Vote_Application.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            

        }

        private async void Loginbtn_Clicked(object sender, EventArgs e)
        {
            try {
                IsLoading(true);
                var user= await WebApiConnection.Login(UserCardNumber.Text, UserPassword.Text);
                if (user != null) {
                    var userSession = new UserSession
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        UserLastName = user.UserLastName,
                        UserPoliticalParty = user.UserPoliticalParty,
                        UserType = user.UserType,
                        UserImage = user.UserImage
                    };
                    await AddUser(userSession);
                    Getters.CurrentUser = userSession;
                    if (user.UserType.ToLower().Equals("user"))
                    {
                     await Navigation.PushAsync(new UserMainPage());
                     IsLoading(false);
                    }
                    else
                    {
                        await Navigation.PushAsync(new AdminMainPage());
                        IsLoading(false);
                    }
                }
                
                else
                {
                    await DisplayAlert("Error", "user not found", "Retry");
                    IsLoading(false);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Object reference"))
                    await DisplayAlert("Error", ex.Message, "Retry");
                else if (ex.Message.Contains("Unable to resolve host"))
                    await DisplayAlert("Error", "Check your internet connection", "Retry");
                else
                    await DisplayAlert("Error", ex.Message, "Retry");
                IsLoading(false);

            }
        }
        void IsLoading(bool Isloading)
        {
            Loginbtn.IsVisible = !Isloading;
            Loginbtn.IsEnabled = !Isloading;
            Loading.IsVisible = Isloading;
        }
        async Task AddUser(UserSession userSession)
        {
        var x = await LocalDBService.AddSession(userSession);

        }
        protected override bool OnBackButtonPressed() => true;
    }
}