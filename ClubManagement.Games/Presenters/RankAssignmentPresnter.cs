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

        private void OnEidtHandi(object sender, HandiEditEventArgs e)
        {
            var button = e.SenderButton;
            var player = button.Tag as IndividualPlayerDto;

            int currentValue = int.TryParse(button.Text, out int result) ? result : 0;

            INumericPadView view = new NumericPadView();
            var padPresenter = new NumericPadPresenter(view, newValue =>
            {
                // 값 반영
                button.Text = newValue.ToString();
                if (player != null)
                    player.AddHandi = newValue;
            }, currentValue);

            view.ShowForm();
        }

        private void SaveRank(object sender, EventArgs e)
        {
            _model.IndividualRanker = _view.IndividaulRankers;
            int readCount = _repository.CheckIndividualSideRank(_model.MatchCode, _model.CurrentGame);
            
            try
            {
                if (readCount > 0)
                {
                    string message = "이미 기록된 사이드 게임입니다.\n삭제 후 다시 등록 하시겠습니까?";
                    if (_view.ShowConfirmation(message))
                    {
                        _repository.UpdateIndividualRank(_model.IndividualRanker, _model.MatchCode, _model.CurrentGame, true);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    _repository.UpdateIndividualRank(_model.IndividualRanker, _model.MatchCode, _model.CurrentGame, false);
                }
                _view.CloseForm();
            }
            catch (Exception ex)
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
