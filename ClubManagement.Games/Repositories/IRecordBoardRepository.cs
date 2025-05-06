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
        DataTable LoadGameOrder(int code);
        DataTable LoadAllPalyerList(int code);
        DataTable LoadGamePlayers(int match);
        DataRow LoadGamePlayer(int match, int game, string name);
        DataTable LoadIndividualSideSet(int match);
        DataTable LoadIndividualSideRank(int match, int game);
        int CheckIndividualSideRank(int match, int game);
        object LoadMatchTitle(int matchCode);
        void InsertGame(RecordBoardModel model);
        void InsertGamePlayer(RecordBoardModel model);
        void UpdatePlayerInfo(PlayerInfoDto player, int match);
        void UPdatePlayerScore(PlayerInfoDto player,int match, int game);
        void UpdateIndividualSet(RecordBoardModel model);
        void UpdateIndividualRank(List<IndividualPlayerDto> players, int match, int game, bool reRecord);
    }
}
