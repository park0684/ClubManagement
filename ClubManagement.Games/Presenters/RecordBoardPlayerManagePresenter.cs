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

        public RecordBoardPlayerManagePresenter(IRecordBoardPlayerManageView view, IRecordBoardRepository repository, RecordBoardModel model)
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

        /// <summary>
        /// 참가자 제거 버튼 클릭 시 호출.
        /// 현재 게임/그룹에서 해당 플레이어를 제거하고 UI 갱신.
        /// </summary>
        private void ParticitantRemove(object sender, participantButtonEventArgs e)
        {
            // 현재 게임, 그룹, 플레이어 찾기
            var currentGame = _model.GameList.FirstOrDefault(g => g.GameSeq == _model.CurrentGame);
            var currentGroup = currentGame.Groups.FirstOrDefault(g => g.GroupNumber == _model.CurrentGroup);
            var player = currentGroup.players.FirstOrDefault(p => p.PlayerName == e.MemberName);

            // 재등록을 위해 그룹에서 플레이어 제거
            currentGroup.players.Remove(player);
            player.IsSelected = false;

            // 버튼 색상 초기화 및 참가자 버튼 다시 생성
            _view.UpdateButtonColor(player.PlayerName, false, false);
            _view.CreateAttendButton(currentGroup.players);
        }

        /// <summary>
        /// 플레이어 버튼 및 참가자 버튼을 뷰에 다시 생성.
        /// 현재 게임/그룹 기준으로 버튼 생성.
        /// </summary>
        public void SetPlayerList()
        {
            var allPlayer = _model.PlayerList;
            var currentGame = _model.GameList.FirstOrDefault(g => g.GameSeq == _model.CurrentGame);
            var currentGroup = currentGame.Groups.FirstOrDefault(group => group.GroupNumber == _model.CurrentGroup);
            var particitandPlayer = currentGroup.players;

            // 모든 플레이어 버튼 생성 (참가 여부 포함)
            _view.CreatePlayerButton(currentGame, _model.CurrentGroup, allPlayer);

            // 현재 참가자 버튼 생성
            _view.CreateAttendButton(particitandPlayer);
        }

        /// <summary>
        /// 참가자 추가 버튼 클릭 시 호출.
        /// 현재 게임/그룹에 플레이어를 추가하고 UI 갱신.
        /// </summary>
        private void ParticipantAdd(object sender, participantButtonEventArgs e)
        {
            string playerName = e.MemberName;
            var currentGame = _model.GameList.FirstOrDefault(g => g.GameSeq == _model.CurrentGame);
            var currentGroup = currentGame.Groups.FirstOrDefault(group => group.GroupNumber == _model.CurrentGroup);
            var particitandPlayer = currentGroup.players;

            // 이미 추가된 플레이어는 중복 추가 방지
            if (particitandPlayer.Any(p => p.PlayerName == playerName))
            {
                return;
            }

            // 플레이어 추가
            var player = _model.PlayerList.FirstOrDefault(p => p.PlayerName == playerName);
            particitandPlayer.Add(player);
            player.IsSelected = true;

            // 버튼 상태 갱신 및 참가자 버튼 다시 생성
            _view.UpdateButtonColor(playerName, true, true);
            _view.CreateAttendButton(particitandPlayer);
        }

        /// <summary>
        /// 참가자 정보 저장 처리.
        /// </summary>
        private void ParticipantsUpdate(object sender, EventArgs e)
        {
            try
            {
                // 모델 정보 기반으로 플레이어 데이터 저장
                _service.InsertGamePlayer(_model);
                _view.CloseForm();
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        /// <summary>
        /// 폼 종료 처리.
        /// </summary>
        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
