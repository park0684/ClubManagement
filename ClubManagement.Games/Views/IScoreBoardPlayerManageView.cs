using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public interface IScoreBoardPlayerManageView
    {

        event EventHandler SaveEvent;
        event EventHandler CloaseEvent;
        event EventHandler<participantButtonEventArgs> PlayerAddEvent;
        event EventHandler<participantButtonEventArgs> PlayerRemoveEvent;
        void CloseForm();
        void ShowForm();
        void ShowMessage(string message);
        void CreatePlayerButton(GameOrderDto gameOrder, int groupNumber, List<PlayerInfoDto> players);
        void CreateAttendButton(List<PlayerInfoDto> participant);
        void UpdateButtonColor(string player,bool isCurrentGroup, bool isSelected );
    }
}
