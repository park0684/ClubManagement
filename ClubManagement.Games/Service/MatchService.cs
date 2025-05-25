using ClubManagement.Common.Hlepers;
using ClubManagement.Games.DTOs;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ClubManagement.Games.Service
{
    public class MatchService
    {
        IMatchRepository _repository;
        public MatchService(IMatchRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 모임 리스트 조회
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataTable LoadMatchList(MatchSearchModel model)
        {
            DataTable result = _repository.LoadMatchList(model);
            result.Columns.Add("No");
            int i = 1;
            foreach (DataRow row in result.Rows)
            {
                row["No"] = i++;
            }
            return result;
        }

        /// <summary>
        /// 선택된 모임의 참석자 조회
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable LoadPlayerList(int code)
        {
            DataTable result = _repository.LoadPlayerList(code);
            result.Columns.Add("No");
            result.Columns.Add("gender");
            result.Columns.Add("memberType");
            int i = 1;
            foreach (DataRow row in result.Rows)
            {
                row["No"] = i++;
                row["gender"] = MemberHelper.GetMemberGenger(Convert.ToInt32(row["att_gender"]));
                row["memberType"] = GameHelper.GetPlayerType(Convert.ToInt32(row["att_memtype"]));
            }

            result.Columns.Remove("att_gender");
            result.Columns.Remove("att_memtype");

            return result;
        }

        /// <summary>
        /// 선택된 모임 상세 정보 조회
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public MatchModel LoadMatch(int code)
        {
            DataRow result = _repository.LoadMatchInfo(code);

            return new MatchModel
            {
                MatchCode = code,
                IsNew = false,
                MatchTitle = result["match_title"].ToString(),
                MatchHost = result["match_host"].ToString(),
                MatchMemo = result["match_memo"].ToString(),
                MatchDate = (DateTime)result["match_date"],
                MatchType = (int)result["match_type"]
            };
        }

        /// <summary>
        /// 모임 설정 저장
        /// 신규의 경우 Insert, 수정의 경우 Update 진행
        /// </summary>
        /// <param name="model"></param>
        public void SaveMatch(MatchModel model)
        {
            if (model.IsNew)
            {
                _repository.InsertMatch(model);
            }
            else
            {
                _repository.UpdateMatch(model);
            }
        }

        /// <summary>
        /// 회원 정보 조회
        /// </summary>
        /// <param name="model"></param>
        /// <param name="players"></param>
        /// <returns></returns>
        public List<PlayerInfoDto> LoadMember(MatchSearchModel model, List<PlayerInfoDto> players)
        {
            DataTable resutl = _repository.LoadMember(model);
            List<PlayerInfoDto> memberList = resutl.AsEnumerable().Select(row => new PlayerInfoDto
            {
                MemberCode = row.Field<int>("mem_code"),
                PlayerName = row.Field<string>("mem_name"),
                IsSelected = players.Any(p => p.MemberCode == row.Field<int>("mem_code")),
                Gender = row.Field<int>("mem_gender") == 1,
                IsPro = row.Field<int>("mem_pro") == 1

            }).ToList();

            return memberList;
        }

        /// <summary>
        /// 참석자 리스트 조회
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public List<PlayerInfoDto> LoadPlayer(int match)
        {
            DataTable result = _repository.LoadAttendPlayer(match);
            List<PlayerInfoDto> playerList = result.AsEnumerable().Select(row => new PlayerInfoDto
            {
                MemberCode = row.Field<int>("att_memcode"),
                PlayerName = row.Field<string>("att_name"),
                IsSelected = true,
                Gender = row.Field<int>("att_gender") == 1,
                IsPro = row.Field<int>("att_pro") == 1

            }).ToList();
            return playerList;
        }

        /// <summary>
        /// 참석자 리스트 저장
        /// </summary>
        /// <param name="model"></param>
        public void PlayerUpdate(MatchModel model)
        {
            _repository.UpdateMatchPlayer(model);
        }
    }
}
