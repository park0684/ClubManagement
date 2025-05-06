using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Games.Views
{
    public interface INumericPadView
    {
        int Nember { get; set; }

        event EventHandler CloseFormEvent;
        event EventHandler InsertNumberEvent;

        void CloseForm();
        void ShowForm();
    }
}
