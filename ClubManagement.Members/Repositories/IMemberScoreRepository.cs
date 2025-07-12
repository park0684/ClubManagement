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
        DataTable GetTotalScore(SearchScoreDto search);
        DataTable GetMatchScore(SearchScoreDto search, int gameCount);
        string GetMatchTitle(int matchCode);
        string GetStartDate();
        int GetGameCount(int matchCode);
        DataTable LoadMatchList(SearchMatchDto search);

        DataTable GetMemberScoreList(SearchScoreDto search, int member);
        DataRow GetMemberBaseInfo(int member);
        DataTable GetGradeInfo();
        void UpdateMemberGrade(int member, int grade);
        void UpdateMemberGradeBulk(List<int> members, int grade);
    }
}
