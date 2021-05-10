using Rep_Vote_Application.Local_DB_Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Rep_Vote_Application.Services
{
    static class LocalDBService
    {
        
        static SQLiteAsyncConnection db;

        #region Init
        static async Task Init()
        {
            if (db == null)
            {
                var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "VoteAppLocalDB.db");

                db = new SQLiteAsyncConnection(databasePath);
                await db.CreateTableAsync<UserSession>();
            }
        }
        #endregion
        #region Adding a new user session to local database (Login)
        public static async Task<int> AddSession(UserSession session)
        {
            await Init();
           var x=await db.InsertAsync(session);
            return x;
        }
        #endregion
        #region Delete the user session from local database (Logout)
        public static async Task<int> DeleteSession(string id)
        {
            await Init();
            var x=await db.DeleteAsync<UserSession>(id);
            return x;
        }
        #endregion
        #region Retrive the user session from the local database
        public static async Task<UserSession> GetSession()
        {
            await Init();
            try
            {
                var query = db.Table<UserSession>().ElementAtAsync(0);
                return await query;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        #endregion
        #region Verify if the table is empty 
        public static async Task<bool> IsTableEmpty()
        {
            await Init();
            var query =await db.Table<UserSession>().CountAsync();
            if (query == 0)
                return true;
            return false;
        }
        #endregion
    }
}
