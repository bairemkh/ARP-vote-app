using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rep_Vote_Application.Local_DB_Entities
{
    class UserSession
    {   [PrimaryKey]
        public string UserId { get; set; }
        public string UserPassword { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string UserType { get; set; }
        public string UserPoliticalParty { get; set; }
        public byte[] UserImage { get; set; }
        



    }
}
