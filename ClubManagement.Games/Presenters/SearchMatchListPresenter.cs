using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Common.Hlepers;
using ClubManagement.Games.DTOs;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Views;

namespace ClubManagement.Games.Presenters
{
    public class SearchMatchListPresenter
    {
        ISearchMatchListView _view;
        IMatchRepository _repository;
        MatchSearchModel _model;

        public SearchMatchListPresenter(ISearchMatchListView view, IMatchRepository repository)
        {
            _view = view;
            _repository = repository;
            _model = new MatchSearchModel();
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
            _model.FromDate = _view.FromDate;
            _model.ToDate = _view.ToDate;
            _model.IsScoreBoardRegisted = false;
            _model.MatchType = 0;
            DataTable source;

            try
            {
                source = _repository.LoadMatchList(_model);
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
