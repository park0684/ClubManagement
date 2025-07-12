using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Common.Hlepers;
using ClubManagement.Members.Models;
using ClubManagement.Members.Views;
using ClubManagement.Members.Repositories;
using ClubManagement.Members.DTOs;

namespace ClubManagement.Members.Presenters
{
    public class SearchMatchPresenter
    {
        ISearchMatchView _view;
        IMemberScoreRepository _repository;
        SearchMatchDto _dto;

        public SearchMatchPresenter(ISearchMatchView view, IMemberScoreRepository repository)
        {
            _view = view;
            _repository = repository;
            _dto = new SearchMatchDto();
            _view.SearchMatchEvent += LoadMatchList;
            _view.CloseFormEvent += CloseForm;
            _view.SelectedMatchEvent += CloseForm;
            LoadMatch();
        }
        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }

        private void LoadMatchList(object sender, EventArgs e)
        {
            LoadMatch();
        }
        private void LoadMatch()
        {
            _dto.FromDate = _view.FromDate;
            _dto.ToDate = _view.ToDate;
            _dto.IsRecordBoardRegisted = false;
            _dto.MatchType = 0;
            DataTable source;

            try
            {
                source = _repository.LoadMatchList(_dto);
                _view.SetDataBinding(source);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
                return;
            }

            int i = 1;
            source.Columns.Add("No");
            source.Columns.Add("type");
            foreach (DataRow row in source.Rows)
            {
                row["No"] = i++;
                row["type"] = GameHelper.GetMatchType(Convert.ToInt32(row["match_type"]));
            }
            source.Columns.Remove("match_type");
            _view.SetDataBinding(source);
        }
    }
}
