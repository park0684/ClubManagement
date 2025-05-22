using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Common.Hlepers;
using ClubManagement.Games.DTOs;
using ClubManagement.Games.Models;
using ClubManagement.Games.Service;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Views;

namespace ClubManagement.Games.Presenters
{
    public class RecordboardRegistPresenter
    {
        private IRecordBoardRegistView _view;
        private IRecordBoardRepository _repository;
        private RecordBoardModel _model;
        private RecordBoardService _service;
        public RecordboardRegistPresenter(IRecordBoardRegistView view, IRecordBoardRepository repository)
        {
            this._view = view;
            this._repository = repository;
            this._model = new RecordBoardModel();
            this._service = new RecordBoardService(_repository);
            this._view.CloseFormEvent += CloseForm;
            this._view.AddOrderEvent += AddGame;
            this._view.EditPlayerEvent += EidtPlayer;
            this._view.SaveOrderEvent += SaveGames;
            this._view.SelectGameEvent += SelectMatch;
            this._view.MatchTitle = "";
        }

        /// <summary>
        /// 모임 검색 및 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectMatch(object sender, EventArgs e)
        {
            ISearchMatchListView view = new SearchMatchListView();
            IMatchRepository repository = new MatchRepository();
            new SearchMatchListPresenter(view, repository);
            view.SelectedMatchEvent += (s, args) =>
            {
                _model.MatchCode = view.MatchCode;
                _model.MatchTitle = view.MatchTitle;
                view.CloseForm();
            };
            view.ShowForm();
            
            _view.MatchTitle = _model.MatchTitle;
            _view.OrderCount = _model.MatchCode;
            if (view.IsRecodeRegisted == true)
            {
                view.ShowMessage("이미 등록된 모임입니다"); 
                LoadGameList(_model.MatchCode);
                return;
            }
            if (_model.MatchCode > 0)
                LoadPlayer(_model.MatchCode);
        }

        private void SaveGames(object sender, EventArgs e)
        {
            try
            {
                _service.InsertGames(_model);
                _view.CloseForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessge(ex.Message);
            }
        }

        private void EidtPlayer(object sender, EventArgs e)
        {
            int match = _model.MatchCode;
            IMatchPlayerManageView view = new MatchPlayerManageView();
            IMatchRepository repository = new MatchRepository();
            MatchPlayerManagePresenter presenter = new MatchPlayerManagePresenter(view, repository);
            presenter.PlayerManageCall(match);
            view.ShowForm();
            LoadPlayer(match);
        }

        private void AddGame(object sender, EventArgs e)
        {
            //_view.AddNewOrder();
            if(_model.MatchCode == 0)
            {
                _view.ShowMessge("모임을 먼저 지정하세요");
                return;
            }
            int gameCount = _model.GameList.Count + 1;
            GameOrderDto gameOrder = new GameOrderDto
            {
                GameSeq = gameCount,
                GameType = 1,
                PlayerCount = 1,
                AllCoverGame = false,
                PersonalSideGame = false
            };
            _model.GameList.Add(gameOrder);
            _view.AddNewOrder(gameOrder);
        }

        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
        public void LoadGameList(int matchCode)
        {
            _model.MatchCode = matchCode;
            _model.GameList = _service.LoadGames(matchCode);
            _view.LoadOrder(_model.GameList);
            LoadPlayer(matchCode);
            _view.SetMatchButton();
        }
        public void LoadPlayer(int match)
        {
            _model.PlayerList = _service.LoadAttendees(match);
            _view.LoadPlayer(_model.PlayerList);
        }
    }
}
