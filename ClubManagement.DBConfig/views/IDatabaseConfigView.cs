using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.DBConfig.views
{
    public interface IDatabaseConfigView
    {
        string Address { get; set; }
        int Port { get; set; }
        string User { get; set; }
        string Password { get; set; }
        string Database { get; set; }

        event EventHandler CloseFromEvent;
        event EventHandler SaveEvent;
        event EventHandler TestEvent;

        void CloseForm();
        void ShowForm();
        void ShowMessage(string message);
        bool ShowConfirmation(string message);
    }
}
