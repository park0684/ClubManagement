using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Members.Views
{
    public interface IBulkChangeView
    {
        int SelectedItem { get; }

        event EventHandler CloseFormEvent;
        event EventHandler<int> SelectedEvent;

        void ShowForm();
        void ShowMessageBox(string message);
        void CloseForm();
        void SetComboBoxItems(Dictionary<int, string> items);
    }
}
