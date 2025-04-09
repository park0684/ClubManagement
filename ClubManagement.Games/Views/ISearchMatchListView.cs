using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ClubManagement.Games.Views
{
    public interface ISearchMatchListView
    {
        DateTime FromDate { get; set; }
        DateTime ToDate { get; set; }
        int MatchCode { get;}
        string MatchTitle { get;}

        event EventHandler CloseFormEvent;
        event EventHandler SearchMatchEvent;
        event EventHandler SelectedMatchEvent;


        void SetDataBinding(DataTable source);
        void ShowMessage(string message);
        void ShowForm();
        void CloseForm();
    }
}
