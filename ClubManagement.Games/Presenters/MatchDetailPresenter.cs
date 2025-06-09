using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Common.Hlepers;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Service;
using ClubManagement.Games.Views;
namespace ClubManagement.Games.Presenters
{
    public class MatchDetailPresenter
    {
        private IMatchDetailView _view;
        private IMatchRepository _repository;
        private MatchModel _model;
        private MatchService _service;

        public MatchDetailPresenter(IMatchDetailView view, IMatchRepository repository)
        {
            this._view = view;
            this._repository = repository;
            this._model = new MatchModel();
            this._service = new MatchService(_repository);
            this._view.CloseEvenvt += CloseAction;
            this._view.SaveEvent += SaveMatch;
            _model.IsNew = true;
        }

        /// <summary>
        /// 모임 코드에 해당하는 경기 정보를 서비스에서 로드하고 뷰에 반영.
        /// </summary>
        /// <param name="code">경기 코드 (MatchCode)</param>
        public void LoadMatch(int code)
        {
            // 서비스에서 경기 정보를 가져와 모델에 저장
            _model = _service.LoadMatch(code);

            // 뷰의 컨트롤에 모델 데이터를 표시
            _view.GameTitle = _model.MatchTitle;             // 경기 제목
            _view.GameHost = _model.MatchHost;               // 주최자
            _view.GameMemo = _model.MatchMemo;               // 메모
            _view.GameDate = (DateTime)_model.MatchDate;     // 경기 일자
            _view.GameType = (int)_model.MatchType;          // 경기 유형
        }
        /// <summary>
        /// 뷰에서 입력된 경기 정보를 모델에 저장하고 서비스로 저장 요청.
        /// 성공 시 폼을 닫음.
        /// </summary>
        private void SaveMatch(object sender, EventArgs e)
        {
            // 뷰에서 입력된 값을 모델에 반영
            _model.MatchTitle = _view.GameTitle;     // 제목
            _model.MatchHost = _view.GameHost;       // 주최자
            _model.MatchDate = _view.GameDate;       // 일자
            _model.MatchType = _view.GameType;       // 유형
            _model.MatchMemo = _view.GameMemo;       // 메모

            try
            {
                // 모델을 서비스에 저장 (Insert/Update 구분은 서비스 내부 처리)
                _service.SaveMatch(_model);
                // 저장 성공 후 폼 닫기
                _view.CloseForm();
            }
            catch (Exception ex)
            {
                // 저장 중 예외 발생 시 사용자에게 메시지 표시
                _view.ShowMessage(ex.Message);
            }            
        }

        /// <summary>
        /// 폼을 닫는 액션 처리.
        /// </summary>
        private void CloseAction(object sender, EventArgs e)
        {
            _view.CloseForm();
        }


    }
}
