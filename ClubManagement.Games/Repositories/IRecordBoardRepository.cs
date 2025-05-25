using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Games.Models;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Repositories
{
    public interface IRecordBoardRepository
    {
        //모임 내 게임 조회
        DataTable LoadGameOrder(int code);
        //모임 참석자 기준으로 플레이어 조회
        DataTable LoadAllPalyerList(int code);
        //각 게임별 팀/그룹별 플레이어 조회
        DataTable LoadGamePlayers(int match);
        DataRow LoadGamePlayer(int match, int game, string name);
        DataTable LoadIndividualSideSet(int match);
        DataTable LoadIndividualSideRank(int match, int game);
        int CheckIndividualSideRank(int match, int game);
        string LoadMatchTitle(int matchCode);
        void InsertGame(RecordBoardModel model);
        void InsertGamePlayer(RecordBoardModel model);
        void UpdatePlayerInfo(PlayerInfoDto player, int match);
        void UPdatePlayerScore(PlayerInfoDto player,int match, int game);
        void UpdateIndividualSet(RecordBoardModel model);
        void UpdateIndividualRank(List<IndividualPlayerDto> players, int match, int game, bool reRecord);
    }
}
