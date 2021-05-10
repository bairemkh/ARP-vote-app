using Rep_Vote_Application.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rep_Vote_Application.Local_DB_Entities
{
    class VoteHistory
    {   [PrimaryKey,AutoIncrement]
        public int VoteId { get; set; }
        public DateTime VoteDateAndTime { get => DateTime.Now; set { } }
        public string UserVote { get => Getters.Vote; set { } }
        public string FinalVote { get => Getters.sessionResult.FinalVote; set { } }
        public string VoteSubject{ get => Getters.CurrentVotingSession.VoteSubject; set { } }
        public string VotedLaw{ get => Getters.TheVotedLaw.LawDesc; set { } }

    }
}
