using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Members.DTOs;
using ClubManagement.Common.DTOs;
namespace ClubManagement.Members.Services
{
    public interface IMemberScoreService
    {
        DataTable GetTotalScore(SearchScoreDto search);
        (DataTable resutData, int gameOrder) GetMatchScore(SearchScoreDto search);
        string GetMatchTitle(int matchCode);
        string GetStartDate();
        List<ScoreDto> GetMemberScoreList(int member, int interval);
        DataRow GetMemberBaseInfo(int member);
        Dictionary<int, string> GetMemberGrade();
        void SaveMemberGrade(int member, int grade);

        void BulkSvaeMemberGared(List<int> members, int grade);
    }
}
