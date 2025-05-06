using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Games.DTOs;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Views;

namespace ClubManagement.Games.Presenters
{
    public class EnterScorePresenter
    {
        IEnterScoreView _view;
        IRecordBoardRepository _repository;
        RecordBoardModel _model;
        public EnterScorePresenter(IEnterScoreView view, IRecordBoardRepository repository, RecordBoardModel model)
        {
            _view = view;
            _repository = repository;
            _model = model;
            _view.CloseFormEvent += CloseForm;
            _view.EnterScoreEvent += EnterScore;
            
        }
        public void GetGameInfo(PlayerInfoDto player)
        {
            DataRow playerInfo = _repository.LoadGamePlayer(_model.MatchCode, _model.CurrentGame,player.PlayerName );
            _model.CurrentGroup = Convert.ToInt32(playerInfo["pl_group"]);
            player.Handycap = Convert.ToInt32(playerInfo["pl_handi"]);
            player.Score = Convert.ToInt32(playerInfo["pl_score"]);
            _view.PlayerName = player.PlayerName;
            _view.Handi = player.Handycap;
            _view.Score = player.Score;
            _view.TotalScore = player.Handycap + player.Score > 300 ? 300 : player.Handycap + player.Score;
            _view.IsAllcover = player.IsAllCover;
            _view.IsPerfact = player.Score == 300 ? true : false;
        }
        private void EnterScore(object sender, EventArgs e)
        {
            var targetGame = _model.GameList.FirstOrDefault(g => g.GameSeq == _model.CurrentGame);
            var targetGroup = targetGame.Groups.FirstOrDefault(g => g.GroupNumber == _model.CurrentGroup);
            var plyer = targetGroup.players.FirstOrDefault(p => p.PlayerName == _view.PlayerName);
            plyer.Score = (int)_view.Score;
            plyer.IsAllCover = _view.IsAllcover;
            _repository.UPdatePlayerScore(plyer, _model.MatchCode, _model.CurrentGame);
            _view.CloseForm();
        }

        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
