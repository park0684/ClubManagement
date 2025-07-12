using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Members.DTOs;

namespace ClubManagement.Members.Services
{
    public interface IMemberScoreService
    {
        DataTable GetTotalScore(SearchMemberDto search);
        (DataTable resutData, int gameOrder) GetMatchScore(int matchCode);
        string GetMatchTitle(int matchCode);
        string GetStartDate();
    }
}
