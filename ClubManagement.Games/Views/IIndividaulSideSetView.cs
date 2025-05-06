using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Games.Views
{
    public interface IIndividualSideSetView
    {
        int? Prize1st { get; set; }
        int? Prize2nd { get; set; }
        int? Prize3rd { get; set; }
        int? Handi1st { get; set; }
        int? Handi2nd { get; set; }
        int? Handi3rd { get; set; }

        event EventHandler SaveEvent;
        event EventHandler CloseEvent;

        void ShowForm();
        void CloseForm();
        void ShowMessage(string message);


    }
}
