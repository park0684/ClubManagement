using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;
using ClubManagement.Games.Models;
using ClubManagement.Games.Views;
using ClubManagement.Games.Repositories;

namespace ClubManagement.Games.Presenters
{
    public class RankAssignmentPresnter
    {
        IRankAssignmentView _view;
        IRecordBoardRepository _repository = new RecordBoardRepository();
        RecordBoardModel _model;

        public RankAssignmentPresnter(IRankAssignmentView view, RecordBoardModel model)
        {
            _view = view;
            _model = model;
            _view.IndividaulSets = _model.IndividaulSideSet;
            _view.IndividaulRankers = _model.IndividualRanker.OrderByDescending(r => r.Score).ToList();
            _view.CloseEvent += CloseForm;
            _view.SaveEvent += SaveRank;
            _view.EditHandiEvent += OnEidtHandi;
            
        }

        /// <summary>
        /// 플레이어 핸디캡 버튼 클릭 시 호출.
        /// 숫자 키패드를 열어 핸디캡을 입력받고 플레이어 값 반영.
        /// </summary>
        private void OnEidtHandi(object sender, HandiEditEventArgs e)
        {
            // 핸디캡 입력 대상 버튼과 해당 플레이어 DTO 가져오기
            var button = e.SenderButton;
            var player = button.Tag as IndividualPlayerDto;

            // 버튼의 현재 텍스트를 int로 변환 (변환 실패 시 0)
            int currentValue = int.TryParse(button.Text, out int result) ? result : 0;

            // 숫자 키패드 뷰 및 프리젠터 생성
            INumericPadView view = new NumericPadView();
            var padPresenter = new NumericPadPresenter(view, newValue =>
            {
                // 키패드에서 입력된 값 버튼 텍스트 및 플레이어 DTO에 반영
                button.Text = newValue.ToString();
                if (player != null)
                    player.AddHandi = newValue;
            }, currentValue);

            // 숫자 키패드 표시
            view.ShowForm();
        }

        /// <summary>
        /// 랭크 저장 버튼 클릭 시 호출.
        /// 랭크 정보를 저장하며, 기존 기록이 있으면 사용자 확인 후 갱신.
        /// </summary>
        private void SaveRank(object sender, EventArgs e)
        {
            // 뷰에서 현재 랭크 데이터를 가져와 모델에 저장
            _model.IndividualRanker = _view.IndividaulRankers;

            // DB에 기존 랭크 기록 여부 확인
            int readCount = _repository.CheckIndividualSideRank(_model.MatchCode, _model.CurrentGame);

            try
            {
                if (readCount > 0)
                {
                    // 기존 기록이 있으면 사용자에게 확인 요청
                    string message = "이미 기록된 사이드 게임입니다.\n삭제 후 다시 등록 하시겠습니까?";
                    if (_view.ShowConfirmation(message))
                    {
                        // 사용자가 삭제 후 재등록을 선택한 경우 기존 기록 갱신
                        _repository.UpdateIndividualRank(_model.IndividualRanker, _model.MatchCode, _model.CurrentGame, true);
                    }
                    else
                    {
                        // 사용자가 재등록을 취소하면 저장 중단
                        return;
                    }
                }
                else
                {
                    // 기존 기록이 없으면 신규 등록
                    _repository.UpdateIndividualRank(_model.IndividualRanker, _model.MatchCode, _model.CurrentGame, false);
                }

                // 저장 완료 후 폼 닫기
                _view.CloseForm();
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        /// <summary>
        /// 폼 종료 버튼 클릭 시 호출.
        /// </summary>
        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
