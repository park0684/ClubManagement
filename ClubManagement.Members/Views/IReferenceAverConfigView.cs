using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Members.Views
{
    public interface IReferenceAverConfigView
    {
        event EventHandler CloseFormEvent;
        event EventHandler<int> SaveEvent;

        void ShowForm();
        void CloseForm();
        void ShowMessage(string message);
        void GetInterval(int interval);

    }
}
