using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace ClubManagement.Games.Views
{
    public interface IScoreboardListView
    {
        DateTime FromDate { get; set; }
        DateTime ToDate { get; set; }
        int? GetMatchCode { get; }

        event EventHandler ScoreBoardRegistEvent;
        event EventHandler ScoreBoardEditEvent;
        event EventHandler ScoreBoarSelectedEvent;
        event EventHandler SearchScoreBoardEvnt;

        void SetDataBinding(DataTable resource);
        void ShowMessage(string message);
    }
}
