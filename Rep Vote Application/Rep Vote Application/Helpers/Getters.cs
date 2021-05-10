using Microsoft.AspNetCore.SignalR.Client;
using Projet_Pfe.Helpers;
using Rep_Vote_Application.Local_DB_Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAPI_SQL.Entities;
using Xamarin.Essentials;

namespace Rep_Vote_Application.Helpers
{
    static class Getters
    {
        public static Law TheVotedLaw;
        public static UserSession CurrentUser;
        public static VotingRoom CurrentVotingSession;
        public static string Vote="Didn't Vote";
        public static VoteSessionResult sessionResult;
        
       
        private async static void close()
        {
            await WebApiConnection.CloseVotingRoom();
        }
       
    }
}
