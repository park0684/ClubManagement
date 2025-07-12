using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ClubManagement.Common.Repositories;
using ClubManagement.Members.DTOs;

namespace ClubManagement.Members.Repositories
{
    public class MemberScoreRepository : BaseRepository, IMemberScoreRepository
    {
        public DataTable GetMatchScore(int matchCode)
        {
            string query = $"SELECT m.mem_code, m.mem_name, a.att_handi, ISNULL(m.mem_ave, 0) mem_ave, g.game_order, pl_score\n" +
                    "FROM member m\n" +
                    $"JOIN attend a ON m.mem_code = a.att_memcode AND a.att_code = {matchCode}\n" +
                    "JOIN players p ON p.pl_match = 83 AND p.pl_member = m.mem_code\n" +
                    "JOIN games g ON g.game_match = p.pl_match AND g.game_order = p.pl_game";
            return SqlAdapterQuery(query);
        }

        public string GetMatchTitle(int matchCode)
        {
            string query = $"SELECT match_title FROM match WHERE match_code = {matchCode}";
            return ScalaQuery(query).ToString();
        }

        public string GetStartDate()
        {
            string query = "SELECT cf_strval  FROM config where cf_code =  12";
            return ScalaQuery(query).ToString();
        }

        public DataTable GetTotelScore(SearchMemberDto search)
        {
            throw new NotImplementedException();
        }
    }
}
