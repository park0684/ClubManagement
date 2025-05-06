using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public interface IRankAssignmentView
    {
        List<IndividualPlayerDto> IndividaulRankers { get; set; }
        List<IndividaulSetDto> IndividaulSets { get; set; }
        
        event EventHandler SaveEvent;
        event EventHandler CloseEvent;
        event EventHandler<HandiEditEventArgs> EditHandiEvent;
        event EventHandler EidtRankEvent;

        void CloseForm();
        void ShowForm();
        void ShowMessage(string message);
        void AddPlayerPanel();
        bool ShowConfirmation(string message);
        //void EidtHendi(int handi);
    }
}
