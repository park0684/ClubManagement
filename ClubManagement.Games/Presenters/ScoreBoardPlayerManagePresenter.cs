using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Games.Models;
using ClubManagement.Games.DTOs;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Views;

namespace ClubManagement.Games.Presenters
{
    public class ScoreBoardPlayerManagePresenter
    {
        IScoreBoardPlayerManageView _view;
        IScoreBoardRepository _repository;
        ScoreBoardModel _model;

        public ScoreBoardPlayerManagePresenter(IScoreBoardPlayerManageView view,IScoreBoardRepository repository,ScoreBoardModel model)
        {
            _view = view;
            _repository = repository;
            _model = model;
            this._view.CloaseEvent += CloseForm;
            this._view.SaveEvent += ParticipantsUpdate;
            this._view.PlayerAddEvent += ParticipantAdd;
            this._view.PlayerRemoveEvent += ParticitantRemove;
            
        }
        private void ParticitantRemove(object sender, participantButtonEventArgs e)
        {
            string playerName = e.MemberName;
            bool participant = e.Attend;
            var currentGame = _model.GameList.FirstOrDefault(g => g.GameSeq == _model.CurrentGame);
            var currentGroup = currentGame.Groups.FirstOrDefault(group => group.GroupNumber == _model.CurrentGroup);
            var particitandPlayer = currentGroup.players;
            var playerToRemove = particitandPlayer.FirstOrDefault(p => p.PlayerName == playerName);
            if(playerToRemove != null)
            {
                particitandPlayer.Remove(playerToRemove);
                _view.UpdateButtonColor(playerName,false, false);
                _view.CreateAttendButton(particitandPlayer);
                playerToRemove.IsSelected = false;
            }
        }
        public  void SetPlayerList()
        {
            var allPlayer = _model.PlayerList;
            var currentGame = _model.GameList.FirstOrDefault(g => g.GameSeq == _model.CurrentGame);
            var currentGroup = currentGame.Groups.FirstOrDefault(group => group.GroupNumber == _model.CurrentGroup);
            var particitandPlayer = currentGroup.players;
            _view.CreatePlayerButton(currentGame, _model.CurrentGroup, allPlayer);
            _view.CreateAttendButton(particitandPlayer);
        }
        private void ParticipantAdd(object sender, participantButtonEventArgs e)
        {
            string playerName = e.MemberName;
            var currentGame = _model.GameList.FirstOrDefault(g => g.GameSeq == _model.CurrentGame);
            var currentGroup = currentGame.Groups.FirstOrDefault(group => group.GroupNumber == _model.CurrentGroup);
            var particitandPlayer = currentGroup.players;
            if (particitandPlayer.Any(p => p.PlayerName == playerName))
            {
                return;
            }
            var player = _model.PlayerList.FirstOrDefault(p => p.PlayerName == playerName);
            particitandPlayer.Add(player);
            //(new PlayerInfoDto {PlayerName = player.PlayerName});
            player.IsSelected = true;
            _view.UpdateButtonColor(playerName, true, true);
            _view.CreateAttendButton(particitandPlayer);
        }

        private void ParticipantsUpdate(object sender, EventArgs e)
        {
            _repository.InsertGamePlayer(_model);
            _view.CloseForm();
        }

        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
