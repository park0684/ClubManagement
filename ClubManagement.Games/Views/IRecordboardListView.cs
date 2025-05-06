using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace ClubManagement.Games.Views
{
    public interface IRecordboardListView
    {
        DateTime FromDate { get; set; }
        DateTime ToDate { get; set; }
        int? GetMatchCode { get; }

        event EventHandler RecordBoardRegistEvent;
        event EventHandler RecordBoardEditEvent;
        event EventHandler RecordBoarSelectedEvent;
        event EventHandler SearchRecordBoardEvnt;

        void SetDataBinding(DataTable resource);
        void ShowMessage(string message);
    }
}
