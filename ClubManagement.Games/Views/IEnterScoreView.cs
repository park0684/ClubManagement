using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Games.Views
{
    public interface IEnterScoreView
    {
        int? Score { get; set; }
        int? Handi { get; set; }
        int? TotalScore { get; set; }
        string PlayerName { get;  set; }
        bool IsPerfact { get; set; }
        bool IsAllcover { get; set; }

        event EventHandler EnterScoreEvent;
        event EventHandler IsPerFectEvent;
        event EventHandler IsAllcoverEvent;
        event EventHandler CloseFormEvent;

        void ShowForm();
        void CloseForm();
        void ShowMessage(string message);

    }
}
