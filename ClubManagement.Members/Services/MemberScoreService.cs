using ClubManagement.Members.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Members.Repositories;

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
        public DataTable GetTotalScore(SearchMemberDto search)
        {
            var result = _repository.GetTotelScore(search);

            return result;
        }

        public string GetMatchTitle(int matchCode)
        {
            var result = _repository.GetMatchTitle(matchCode);

            return result;
        }

        public (DataTable resutData, int gameOrder) GetMatchScore(int matchCode)
        {
            var result = _repository.GetMatchScore(matchCode);
            var gameCount = Convert.ToInt32(result.Compute("MAX(game_order)", string.Empty));
            var columns = new Dictionary<string, string>();

            return (result, gameCount);
        }

        public string GetStartDate()
        {
            var result = _repository.GetStartDate();
            return result;
        }
    }
}
