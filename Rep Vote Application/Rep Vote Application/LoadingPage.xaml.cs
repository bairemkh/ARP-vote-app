using Rep_Vote_Application.Helpers;
using Rep_Vote_Application.Local_DB_Entities;
using Rep_Vote_Application.Services;
using Rep_Vote_Application.Views;
using Rep_Vote_Application.Views.Admin_Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rep_Vote_Application
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {   bool IsSessionClosed;
        UserSession user;
        public LoadingPage()
        {
            InitializeComponent();
            LogIntoApp();
        }
         async void LogIntoApp()
        {   
            await RetriveIfSessionOpened();
            if (IsSessionClosed)
            {
                
                await Navigation.PushAsync(new LoginPage());
                Navigation.RemovePage(this);
            }
            else
            {
                await RetriveUserIfSessionOpened();
                if (user.UserType.ToLower().Equals("admin"))
                {
                    
                    await Navigation.PushAsync(new AdminMainPage());
                    Navigation.RemovePage(this);
                }
                else
                {
                    
                    await Navigation.PushAsync(new UserMainPage());
                    Navigation.RemovePage(this);
                }
            }
        }

        async Task RetriveUserIfSessionOpened()
        {
            user = await LocalDBService.GetSession();
            Getters.CurrentUser = user;
            
            

        }

        async Task RetriveIfSessionOpened()
        {
            IsSessionClosed = await LocalDBService.IsTableEmpty();
            
            
        }
    }
}