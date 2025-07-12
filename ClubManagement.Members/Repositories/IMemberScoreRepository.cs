using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Members.DTOs;

namespace ClubManagement.Members.Repositories
{
    public interface IMemberScoreRepository
    {
        DataTable GetTotelScore(SearchMemberDto search);
        DataTable GetMatchScore(int matchCode);
        string GetMatchTitle(int matchCode);
        string GetStartDate();

    }
}
