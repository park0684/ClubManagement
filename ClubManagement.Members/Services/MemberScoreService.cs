using ClubManagement.Members.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Members.Repositories;
using ClubManagement.Common.DTOs;

namespace ClubManagement.Members.Services
{
    public class MemberScoreService : IMemberScoreService
    {
        IMemberScoreRepository _repository;
        public MemberScoreService(IMemberScoreRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 회원별 점수 목록 조회
        /// </summary>
        /// <param name="search">검색 조건 DTO</param>
        /// <returns></returns>
        public DataTable GetTotalScore(SearchScoreDto search)
        {
            var result = _repository.GetTotalScore(search);

            return result;
        }

        public string GetMatchTitle(int matchCode)
        {
            var result = _repository.GetMatchTitle(matchCode);

            return result;
        }

        public (DataTable resutData, int gameOrder) GetMatchScore(SearchScoreDto search)
        {
            var gameCount = _repository.GetGameCount(search.MatchCode);
            var result = _repository.GetMatchScore(search, gameCount);

            return (result, gameCount);
        }

        public string GetStartDate()
        {
            var result = _repository.GetStartDate();
            return result;
        }

        /// <summary>
        /// 지정 회원의 기간내 모임,게임별 점수를 조회
        /// </summary>
        /// <param name="member">회원코드</param>
        /// <param name="interval">조회 간격</param>
        /// <returns></returns>
        public List<ScoreDto> GetMemberScoreList(int member, int interval)
        {
            var search = new SearchScoreDto
            {
                ToDate = DateTime.Now,
                FromDate = interval == 0 ? Convert.ToDateTime(_repository.GetStartDate()) : DateTime.Now.AddMonths(-interval)
            };
            var resultDate = _repository.GetMemberScoreList(search, member);
            
            List<ScoreDto> scores = new List<ScoreDto>();
            var scoreDate = resultDate;
            foreach(DataRow row in scoreDate.Rows)
            {
                var score = new ScoreDto
                {
                    GameTitle = row["match_title"].ToString(),
                    GameDate = Convert.ToDateTime(row["match_date"]),
                    GameAverage = Convert.ToDecimal(row["game_average"]),
                    TotalScore = Convert.ToInt32(row["game_totalscore"]),
                    GameScore = new List<int>()
                };
                foreach (DataColumn col in scoreDate.Columns)
                {
                    if (col.ColumnName.StartsWith("Game"))
                    {
                        var val = row[col];
                        if (val != DBNull.Value)
                        {
                            score.GameScore.Add(Convert.ToInt32(val));
                        }
                    }
                }
                scores.Add(score);
            }
            return scores;
        }

        public DataRow GetMemberBaseInfo(int member)
        {
            var reulst = _repository.GetMemberBaseInfo(member);
            return reulst;
        }

        public Dictionary<int, string> GetMemberGrade()
        {
            var resultDate = _repository.GetGradeInfo();
            var result = new Dictionary<int, string>();
            foreach(DataRow row in resultDate.Rows)
            {
                result.Add(Convert.ToInt32(row["grd_code"]), row["grd_name"].ToString());
            }
            return result;
        }

        public void SaveMemberGrade(int member, int grade)
        {
            _repository.UpdateMemberGrade(member, grade);
        }

        public void BulkSvaeMemberGared(List<int> members, int grade)
        {
            _repository.UpdateMemberGradeBulk(members, grade);
        }

        public void UpdateGradeInfo(DataTable updateItems)
        {
            var preItems = _repository.GetGradeInfo();
                        
            int preCount = preItems.Rows.Count;
            int postCount = updateItems.Rows.Count;

            // DB에 있는 데이터가 수정 데이터보다 클 경우 
            if (preCount > postCount) 
                _repository.UpdateGradeInfo(updateItems, postCount + 1);//삭제 시작 코드 같이 전달
            else 
                _repository.UpdateGradeInfo(updateItems); // 같거나 작을 경우 수정 데이터만 전달
        }

        public int GetAverageInterval()
        {
            return _repository.GetAverageInterval();
        }

        public void SaveReferenceAverageInterval(int interval)
        {
            _repository.UpdateAverageInter(interval);
        }
    }
}
