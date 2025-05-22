using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public interface IRecordBoardView
    {
        //필드
        string MatchTitle { set; }
        string GameSeq { set; }

        //이벤트
        event Action<int> GameButtonClick;
        event Action<int, int> AssignPlayerClick;
        event EventHandler SetIndividualSideEvent;
        event EventHandler AllcoverGameSetEvent;
        event Action<string> PlayerOptionEvent;
        event Action<PlayerInfoDto> EnterScoreEvent;
        event EventHandler SaveIndividualRankEvent;

        
        //메서드
        void ShowForm();
        void CloseForm();
        void ShowMessage(string message);
        void SetAllPlayerList(List<GameOrderDto> groups, List<PlayerInfoDto> players);
        void SetBindingSideGame(List<PlayerInfoDto> players);
        void SetSideGamePlayerList(List<PlayerInfoDto> Players);
        void SetAllcoverGamePlayers(List<PlayerInfoDto> players);
        void SetGroupScoreList(GameOrderDto groups);
        void flpGameGroupClear();
        void AddPlayerPanal(PlayerInfoDto player);
        void CreateGroupPanal(GroupDto group, int gameSeq);
        void CreateGameButton(List<GameOrderDto> games);
        void LoadAllcoverGamePlayers(GameOrderDto game);
        List<IndividualPlayerDto> SetIndividualSideRank(int rank);
        bool ShowConfirmation(string message);
        void BindingIndividualScore(DataTable players);
    }
}
