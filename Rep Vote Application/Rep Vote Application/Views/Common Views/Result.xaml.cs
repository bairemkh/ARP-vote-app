
using Projet_Pfe.Helpers;
using Rep_Vote_Application.Helpers;
using Rep_Vote_Application.Views.Admin_Views;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Projet_Pfe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Result : ContentPage
    {
       

        public Result()
        {
        InitializeComponent();
            Results();
        }
        async void Results()
        {
            
            var total = Getters.sessionResult.TotalVotes;
            var yes = Getters.sessionResult.YesVotes;
            var no = Getters.sessionResult.NolVotes;
            var retained = Getters.sessionResult.RetainedVotes;

            await YesProgBar.ProgressTo(((double)yes /total), 500, Easing.SinIn);
            await NoProgBar.ProgressTo(((double)no / total), 500, Easing.SinIn);
            await RetainedProgBar.ProgressTo(((double)retained / total), 500, Easing.SinIn);
            
        }
        protected override bool OnBackButtonPressed() => true;

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (Getters.CurrentUser.UserType.ToLower().Equals("user"))
            Navigation.PushAsync(new UserMainPage());
            else
            Navigation.PushAsync(new AdminMainPage());

        }
    }
} 
