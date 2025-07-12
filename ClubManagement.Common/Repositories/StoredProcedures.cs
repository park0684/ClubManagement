using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Common.Repositories
{
    public static class StoredProcedures
    {
        public const string InsertGames = "usp_InsertGames";
        public const string InsertGamePlayer = "usp_InsertGamePlayer";
        public const string UpdateConfig= "usp_UpdateConfig";
        public const string UpdatePlayerOption= "usp_UpdatePlayerOption";
        public const string UpdatePlayerScore= "usp_UpdatePlayerScore";
        public const string SetIndividualOption= "usp_SetIndividualOption";
        public const string InsertIndividaulRank= "usp_InsertIndividaulRank";
        public const string InsertMember= "usp_InsertMember";
        public const string UpdateMatchPlayer= "usp_UpdateMatchPlayer";
        public const string UpdateMatch= "usp_UpdateMatch";
        public const string UpdateMember= "usp_UpdateMember";
        public const string InsertStatement= "usp_InsertStatment";
        public const string UpdateStatement= "usp_UpdateeStatement";
        public const string InsertMatch= "usp_InsertMatch";
        public const string DeleteStatement = "usp_DeletStatment";
        public const string UpdateMemberGrade = "usp_UpdateMemberGarde";
    }
}
