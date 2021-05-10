using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebAPI_SQL.Entities;

namespace Rep_Vote_Application.Helpers
{
    public static class WebApiConnection

    {
        private static string ApiRoutURL = "https://voteapplicationwebapi.azurewebsites.net/";

        #region Login
        public static async Task<User> Login(string UserLogin,string UserPassword)
        {

            HttpClient client = new HttpClient();
            try
            {
                var response = await client.GetStringAsync("https://voteapplicationwebapi.azurewebsites.net/api/Users/Login?id=" + UserLogin + "&password=" + UserPassword);
                var user = JsonConvert.DeserializeObject<User>(response);
                return user;
            }
            catch (Exception)
            { return null; }
        }
        #endregion
        #region Creating Voting Room
        public static async void CreateVotingRoom(double duration, DateTime date, TimeSpan time, string voteSubject, string lawRef)
        {
            var request = new HttpClient();
            VotingRoom votingRoom;
            
            try
            {


                votingRoom = new VotingRoom(Convert.ToInt32(duration.ToString()), date.ToString("D"), time.ToString().Substring(0, 5), voteSubject, lawRef, "opened");



                var jsonContent = JsonConvert.SerializeObject(votingRoom);


                var content = new StringContent(jsonContent);


                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await request.PostAsync(ApiRoutURL + $"api/VotingRoom/CreateVotingSession", content);

                var resContent = await response.Content.ReadAsStringAsync();


                var resUser = JsonConvert.DeserializeObject<string>(resContent);

                

            }
            catch (Exception)
            {

            }


        }
        #endregion
        #region Room Getter
        public static async Task<VotingRoom> GetLastVotingRoom()
        {

            try
            {

                var request = new HttpClient();

                var response = await request.GetStringAsync("https://voteapplicationwebapi.azurewebsites.net/api/VotingRoom/GetVotingSession");


                var votingRoom = JsonConvert.DeserializeObject<VotingRoom>(response);
                return votingRoom;

            }
            catch (Exception ex)
            {
                var votingRoom = new VotingRoom(0, "", "", ex.Message, "", "Mrigel");

                return votingRoom;
            }
        }
        #endregion
        #region Law Getter
        public static async Task<Law> GetLaw(string lawChapterS, string lawNumberS)
        {

            HttpClient client = new HttpClient();
            try
            {
                int lawChapter = int.Parse(lawChapterS);
                int lawNumber = int.Parse(lawNumberS);

                var response = await client.GetStringAsync("https://voteapplicationwebapi.azurewebsites.net/api/Laws/GetLaw?Chapter=" + lawChapter + "&Number=" + lawNumber);
                var law = JsonConvert.DeserializeObject<Law>(response);
                return law;


            }
            #region law not found
            catch (Exception)
            {
                return null;
            }


            #endregion
        }
        public static async Task<Law> GetLaw(string lawRef)
        {

            HttpClient client = new HttpClient();
            try
            {
                int lawChapter = int.Parse(lawRef.Substring(2, 1));
                int lawNumber = int.Parse(lawRef.Substring(lawRef.Length - 1, 1));

                var response = await client.GetStringAsync("https://voteapplicationwebapi.azurewebsites.net/api/Laws/GetLaw?Chapter=" + lawChapter + "&Number=" + lawNumber);
                var law = JsonConvert.DeserializeObject<Law>(response);
                return law;


            }
            #region law not found
            catch (Exception)
            {
                return null;
            }


            #endregion
        }
        #endregion
        #region Close Room 
        public static async Task CloseVotingRoom()
        {

            try
            {

                var request = new HttpClient();

                var response = await request.GetStringAsync("https://voteapplicationwebapi.azurewebsites.net/api/VotingRoom/CloseRoom");


                
               

            }
            catch (Exception )
            {
                

               
            }
        }
        #endregion
        #region Verify if the room is closed or not 
        public static async Task<bool> IsVotingRoomClosed()
        {

            

               var votingroom=await GetLastVotingRoom();
                if (votingroom.SessionState.ToLower().Equals("opened"))
                    return false;
                return true;

            
            
        }
        #endregion
        #region User Voting
        public static async Task CreateVote(UserVote vote)
        {
            var request = new HttpClient();
        try
            {
                var jsonContent = JsonConvert.SerializeObject(vote);


                var content = new StringContent(jsonContent);


                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await request.PostAsync(ApiRoutURL + $"api/Votes/Vote", content);

                var resContent = await response.Content.ReadAsStringAsync();


                var resUser = JsonConvert.DeserializeObject<string>(resContent);



            }
            catch (Exception)
            {

            }


        }
        #endregion
        #region Get User's vote
        public static async Task<UserVote> GetVote(string UserId)
        {
            var request = new HttpClient();
            try
            {

                var response = await request.GetStringAsync("https://voteapplicationwebapi.azurewebsites.net/api/Votes/RetriveUserVote?UserId=" + UserId);
                
                var resUser = JsonConvert.DeserializeObject<UserVote>(response);
                return resUser;
            }
            catch (Exception ex)
            {
                UserVote vote = new UserVote(UserId, ex.Message);
                return vote;
            }
        }
        #endregion

        #region Votes
        public static async Task<int> TotalVotes()
        {
            var request = new HttpClient();
            try
            {

                var response = await request.GetStringAsync("https://voteapplicationwebapi.azurewebsites.net/api/Votes/VotersNumber");

                var Number = JsonConvert.DeserializeObject<int>(response);


                return Number;

            }
            catch (Exception)
            {
                return -1;
            }


        }
        public static async Task<int> YesVotes()
        {
            var request = new HttpClient();
            try
            {

                var response = await request.GetStringAsync("https://voteapplicationwebapi.azurewebsites.net/api/Votes/Get YesVote");

                var Number = JsonConvert.DeserializeObject<int>(response);


                return Number;

            }
            catch (Exception)
            {
                return -1;
            }


        }
        public static async Task<int> NoVotes()
        {
            var request = new HttpClient();
            try
            {

                var response = await request.GetStringAsync("https://voteapplicationwebapi.azurewebsites.net/api/Votes/Get NoVote");

                var Number = JsonConvert.DeserializeObject<int>(response);


                return Number;

            }
            catch (Exception)
            {
                return -1;
            }


        }
        public static async Task<int> RetainedVotes()
        {
            var request = new HttpClient();
            try
            {

                var response = await request.GetStringAsync("https://voteapplicationwebapi.azurewebsites.net/api/Votes/Get ReservedVote");

                var Number = JsonConvert.DeserializeObject<int>(response);


                return Number;

            }
            catch (Exception)
            {
                return -1;
            }


        }
        public static async Task<string> FinalVote()
        {
            var request = new HttpClient();
            try
            {

                var response = await request.GetStringAsync("https://voteapplicationwebapi.azurewebsites.net/api/Votes/MustVoted");

              


                return response;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }
        public static async Task<VoteSessionResult> GetVoteResult()
        {
            var request = new HttpClient();
            try
            {

                var response = await request.GetStringAsync("https://voteapplicationwebapi.azurewebsites.net/api/VotingRoom/GetResult");

                var Result = JsonConvert.DeserializeObject<VoteSessionResult>(response);
                return Result;
            }
            catch (Exception ex)
            {
                VoteSessionResult vote = new VoteSessionResult(0, 0, 0, 0, ex.Message);
                return vote;
            }
        }

        #endregion
    }
}
