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

        /// <summary>
        /// 특정 게임의 플레이어 정보를 조회하여 모델과 뷰에 반영.
        /// </summary>
        /// <param name="player">플레이어 정보 DTO (이름 전달 및 값 갱신)</param>
        public void GetGameInfo(PlayerInfoDto player)
        {
            DataRow playerInfo = _repository.LoadGamePlayer(_model.MatchCode, _model.CurrentGame,player.PlayerName );
            _model.CurrentGroup = Convert.ToInt32(playerInfo["pl_group"]);
            player.Handycap = Convert.ToInt32(playerInfo["att_handi"]);
            player.Score = Convert.ToInt32(playerInfo["pl_score"]);
            _view.PlayerName = player.PlayerName;
            _view.Handi = player.Handycap;
            _view.Score = player.Score;
            _view.TotalScore = player.Handycap + player.Score > 300 ? 300 : player.Handycap + player.Score;
            _view.IsAllcover = player.IsAllCover;
            _view.IsPerfect = player.Score == 300 ? true : false;
        }

        /// <summary>
        /// 점수 입력 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterScore(object sender, EventArgs e)
        {
            //현재 게임 지정
            var targetGame = _model.GameList.FirstOrDefault(g => g.GameSeq == _model.CurrentGame);

            //현재 그룹 지정
            var targetGroup = targetGame.Groups.FirstOrDefault(g => g.GroupNumber == _model.CurrentGroup);

            //점수 입력 할 플레이어 지정
            var plyer = targetGroup.players.FirstOrDefault(p => p.PlayerName == _view.PlayerName);

            //점수 입력 및 올커버 여부 Dto 등록
            plyer.Score = (int)_view.Score;
            plyer.IsAllCover = _view.IsAllcover;
            try
            {
                //점수 DB에 기록 및 폼 종료
                _repository.UPdatePlayerScore(plyer, _model.MatchCode, _model.CurrentGame);
                _view.CloseForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
            
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
