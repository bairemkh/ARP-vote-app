using Projet_Pfe.Views;
using Rep_Vote_Application.Helpers;
using Rep_Vote_Application.Local_DB_Entities;
using Rep_Vote_Application.Services;
using Rep_Vote_Application.Views;
using Rep_Vote_Application.Views.Admin_Views;
using System;
using System.Threading.Tasks;
using WebAPI_SQL.Entities;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Rep_Vote_Application
{
    public partial class App : Application
    {
       
        public App()
        {
            InitializeComponent();
            
           MainPage = new NavigationPage(new LoadingPage());
           

        }
       
        
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
