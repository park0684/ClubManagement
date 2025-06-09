using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public interface IRecordBoardRegistView
    {
        string MatchTitle { get; set; }
        int OrderCount { get; set; }
        List<GameOrderDto> GameOrderList { get; set; }
        List<PlayerInfoDto> PlayerList { get; set; }

        event EventHandler AddOrderEvent;
        event EventHandler SaveOrderEvent;
        event EventHandler CloseFormEvent;
        event EventHandler SelectGameEvent;
        event EventHandler EditPlayerEvent;

        void LoadOrder(List<GameOrderDto> orders);
        void LoadPlayer(List<PlayerInfoDto> players);
        void AddNewOrder(GameOrderDto newOrder);
        void SearchGame();
        void CloseForm();
        void ShowForm();
        void ShowMessage(string message);
        void SetMatchButton();

    }
}
