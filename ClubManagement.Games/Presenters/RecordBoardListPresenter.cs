using System;
using System.Data;
using ClubManagement.Common.Hlepers;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Service;
using ClubManagement.Games.Views;

namespace ClubManagement.Games.Presenters
{
    public class RecordBoardListPresenter
    {
        IRecordboardListView _view;
        IMatchRepository _repository;
        MatchSearchModel _model;
        MatchService _service;
        public RecordBoardListPresenter(IRecordboardListView view, IMatchRepository repository)
        {
            this._view = view;
            this._repository = repository;
            this._model = new MatchSearchModel();
            this._service = new MatchService(_repository);
            this._view.RecordBoardRegistEvent += ScoreBoardSetRegist;
            this._view.RecordBoardEditEvent += ScoreBoardSetEidt;
            this._view.RecordBoarSelectedEvent += ScoreBoardSelected;
            this._view.SearchRecordBoardEvnt += SearchMatchList;
            LoadMatchList();
        }
        private void LoadMatchList()
        {
            _model.FromDate = _view.FromDate;
            _model.ToDate = _view.ToDate;
            _model.IsRecordBoardRegisted = true;
            _model.MatchType = 0;
            
            DataTable source = _service.LoadMatchList(_model);
            source.Columns.Add("type", typeof(string));
            foreach (DataRow row in source.Rows)
            {
                row["type"] = GameHelper.GetMatchType(Convert.ToInt32(row["match_type"]));
            }
            _view.SetDataBinding(source);
        }
        private void SearchMatchList(object sender, EventArgs e)
        {
            LoadMatchList();
        }

        private void ScoreBoardSelected(object sender, EventArgs e)
        {
            int code = _view.GetMatchCode.Value;
            IRecordBoardView view = new RecordBoardView();
            IRecordBoardRepository repository = new RecordBoardRepository();
            RecordBoardPresenter presenter = new RecordBoardPresenter(view, repository);
            presenter.GetMatchInfo(code);
            view.ShowForm();
        }

        private void ScoreBoardSetEidt(object sender, EventArgs e)
        {
            int code = _view.GetMatchCode.Value;
            IRecordBoardRegistView view = new RecordBoardRegistView();
            IRecordBoardRepository repository = new RecordBoardRepository();
            RecordboardRegistPresenter presenter = new RecordboardRegistPresenter(view, repository);
            presenter.LoadGameList(code);
            view.ShowForm();
        }

        private void ScoreBoardSetRegist(object sender, EventArgs e)
        {
            IRecordBoardRegistView view = new RecordBoardRegistView();
            IRecordBoardRepository repository = new RecordBoardRepository();
            new RecordboardRegistPresenter(view, repository);
            view.ShowForm();
        }
    }
}
