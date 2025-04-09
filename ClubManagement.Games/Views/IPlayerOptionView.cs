using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;
namespace ClubManagement.Games.Views
{
    public interface IPlayerOptionView
    {
        PlayerInfoDto UpdatePlayer { get; }

        event EventHandler SaveEvent;
        event EventHandler CloseEvent;

        void ShowForm();
        void CloseForm();
        void ShowMessage(string message);
        void SetPlayerOption(PlayerInfoDto player);   
    }
}
