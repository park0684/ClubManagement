using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Games.Models;
using ClubManagement.Games.DTOs;
using ClubManagement.Games.Service;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Views;

namespace ClubManagement.Games.Presenters
{
    public class RecordBoardPlayerManagePresenter
    {
        IRecordBoardPlayerManageView _view;
        IRecordBoardRepository _repository;
        RecordBoardModel _model;
        RecordBoardService _service;

        public RecordBoardPlayerManagePresenter(IRecordBoardPlayerManageView view,IRecordBoardRepository repository,RecordBoardModel model)
        {
            _view = view;
            _repository = repository;
            _model = model;
            _service = new RecordBoardService(_repository);
            this._view.CloaseEvent += CloseForm;
            this._view.SaveEvent += ParticipantsUpdate;
            this._view.PlayerAddEvent += ParticipantAdd;
            this._view.PlayerRemoveEvent += ParticitantRemove;
            
        }
        private void ParticitantRemove(object sender, participantButtonEventArgs e)
        {
            var currentGame = _model.GameList.FirstOrDefault(g => g.GameSeq == _model.CurrentGame);
            var currentGroup = currentGame.Groups.FirstOrDefault(g => g.GroupNumber == _model.CurrentGroup);
            var player = currentGroup.players.FirstOrDefault(p => p.PlayerName == e.MemberName);

            currentGroup.players.Remove(player);
            player.IsSelected = false;

            _view.UpdateButtonColor(player.PlayerName, false, false);
            _view.CreateAttendButton(currentGroup.players);
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
            player.IsSelected = true;
            _view.UpdateButtonColor(playerName, true, true);
            _view.CreateAttendButton(particitandPlayer);
        }

        private void ParticipantsUpdate(object sender, EventArgs e)
        {
            try
            {
                _service.InsertGamePlayer(_model);
                _view.CloseForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
