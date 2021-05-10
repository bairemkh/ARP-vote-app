
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Projet_Pfe.Helpers
{
    static class  VoteTimer
    {

        public static HubConnection hubConnection = new HubConnectionBuilder().WithUrl("https://voteapplicationwebapi.azurewebsites.net/Timer").Build();
    }
}
