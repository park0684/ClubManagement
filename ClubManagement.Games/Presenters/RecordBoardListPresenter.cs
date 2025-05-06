using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Common.Hlepers;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Views;

namespace ClubManagement.Games.Presenters
{
    public class RecordBoardListPresenter
    {
        IRecordboardListView _view;
        IRecordBoardRepository _repository;
        MatchSearchModel _model;
        public RecordBoardListPresenter(IRecordboardListView view, IRecordBoardRepository repository)
        {
            this._view = view;
            this._repository = repository;
            this._model = new MatchSearchModel();
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
            IMatchRepository repository = new MatchRepository();
            DataTable source = repository.LoadMatchList(_model);
            int i = 1;
            source.Columns.Add("No", typeof(int));
            source.Columns.Add("type", typeof(string));
            foreach (DataRow row in source.Rows)
            {
                row["No"] = i++;
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
