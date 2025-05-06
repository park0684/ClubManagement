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
    public class RecordboardRegistPresenter
    {
        private readonly IRecordBoardRegistView _view;
        private readonly IRecordBoardRepository _repository;
        private RecordBoardModel _model;
        public RecordboardRegistPresenter(IRecordBoardRegistView view, IRecordBoardRepository repository)
        {
            this._view = view;
            this._repository = repository;
            this._model = new RecordBoardModel();
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
                _repository.InsertGame(_model);
                _view.CloseForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessge(ex.Message);
            }
        }

        private void EidtPlayer(object sender, EventArgs e)
        {
            int code = _model.MatchCode;
            IMatchPlayerManageView view = new MatchPlayerManageView();
            IMatchRepository repository = new MatchRepository();
            MatchPlayerManagePresenter presenter = new MatchPlayerManagePresenter(view, repository);
            presenter.PlayerManageCall(code);
            view.ShowForm();
            LoadPlayer(code);
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
            //int code = matchCode;
            _model.MatchCode = matchCode;
            DataTable source = _repository.LoadGameOrder(matchCode);
            GameOrderDto gameOrder;
            foreach (DataRow row in source.Rows)
            {
                gameOrder = new GameOrderDto();
                gameOrder.GameSeq = Convert.ToInt32(row["game_order"]);
                gameOrder.GameType = Convert.ToInt32(row["game_type"]);
                gameOrder.PlayerCount = Convert.ToInt32(row["game_player"]);
                gameOrder.PersonalSideGame = Convert.ToInt32(row["game_side"]) == 1 ? true : false;
                gameOrder.AllCoverGame = Convert.ToInt32(row["game_allcover"]) == 1 ? true : false;
                _model.GameList.Add(gameOrder);
            }
            _view.LoadOrder(_model.GameList);
            LoadPlayer(matchCode);
            _view.SetMatchButton();
        }
        public void LoadPlayer(int code)
        {
            IMatchRepository repository = new MatchRepository();
            _model.MatchTitle = _repository.LoadMatchTitle(code).ToString();
            DataTable result = repository.LoadAttendPlayer(code);
            List<PlayerInfoDto> playerList = result.AsEnumerable().Select(row => new PlayerInfoDto
            {
                MemberCode = row.Field<int>("att_memcode"),
                PlayerName = row.Field<string>("att_name"),
                IsSelected = true,
                Gender = row.Field<int>("att_gender") == 0,
                IsPro = row.Field<int>("att_pro") == 1

            }).ToList();
            _model.PlayerList = playerList;
            _view.LoadPlayer(playerList);
            _view.MatchTitle = _model.MatchTitle;
        }
    }
}
