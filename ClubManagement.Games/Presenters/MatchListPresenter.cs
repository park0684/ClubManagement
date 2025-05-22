using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Common.Hlepers;
using ClubManagement.Games.Models;
using ClubManagement.Games.Views;
using ClubManagement.Games.Service;
using ClubManagement.Games.Repositories;

namespace ClubManagement.Games.Presenters
{
    class MatchListPresenter
    {
        IMatchListView _view;
        IMatchRepository _repository;
        MatchSearchModel _model;
        MatchService _service;
        public MatchListPresenter(IMatchListView view, IMatchRepository repository)
        {
            this._view = view;
            this._repository = repository;
            this._model = new MatchSearchModel();
            this._service = new MatchService(repository);
            //뷰이벤트 핸들러 설정
            this._view.EditMatchEvent += EditGame;
            this._view.AddMatchEvent += AddGame;
            this._view.SearchMatchEvent += SearchGame;
            this._view.SearchPlayerEvent += SearchPlayerList;
            this._view.EditPlayerEvent += EidtPlayerList;

            LoadMatchList();
        }

        private void LoadMatchList()
        {
            _model.FromDate = _view.MatchFromDate;
            _model.ToDate = _view.MatchToDate;
            _model.MatchType = _view.MatchType ?? 0;
            _model.ExcludeType = _view.ExcludeType;
            DataTable dataSource = _service.LoadMatchList(_model);
            _view.SetGameListBinding(dataSource);
        }
        private void LoadPlayerList(int code)
        {
            DataTable players = _service.LoadPlayerList(code);
            _view.SetPlayerListBinding(players);
        }
        private void SearchPlayerList(object sender, EventArgs e)
        {
            try
            {
                int code = _view.GetMatchCode ?? 0;
                if (code <= 0)
                {
                    _view.ShowMessage("조회된 모임이 없습니다.");
                    return;
                }
                LoadPlayerList(code);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void SearchGame(object sender, EventArgs e)
        {
            try
            {
                LoadMatchList();
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }
        private void EidtPlayerList(object sender, EventArgs e)
        {
            int code = _view.GetMatchCode.Value;
            IMatchPlayerManageView view = new MatchPlayerManageView();
            IMatchRepository repository = _repository;
            MatchPlayerManagePresenter presenter = new MatchPlayerManagePresenter(view, repository);
            presenter.PlayerManageCall(code);
            view.ShowForm();
            LoadPlayerList(code);
        }

        private void AddGame(object sender, EventArgs e)
        {
            IMatchDetailView view = new MatchDetailView();
            IMatchRepository repository = _repository;
            new MatchDetailPresenter(view, repository);
            view.ShowForm();
            LoadMatchList();
        }

        private void EditGame(object sender, EventArgs e)
        {
            int code = _view.GetMatchCode.Value;
            IMatchDetailView view = new MatchDetailView();
            IMatchRepository repository = _repository;
            MatchDetailPresenter presenter = new MatchDetailPresenter(view, repository);
            presenter.LoadMatch(code);
            view.ShowForm();
            LoadMatchList();

        }
    }
}
