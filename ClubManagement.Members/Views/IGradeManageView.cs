using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ClubManagement.Members.Views
{
    public interface IGradeManageView
    {
        event EventHandler AddGradeEvent;
        event EventHandler DeleteGradeEvent;
        event EventHandler<DataTable> SaveEvent;
        event EventHandler CloseFormEvent;

        void CloseForm();
        void ShowForm();
        void ShowMessagebox(string message);
        void BindingGradeData(DataTable result);
        void AddGradeItem();
        void DeleteGradeItem();
        

    }
}
