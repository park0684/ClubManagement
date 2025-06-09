using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Common.Hlepers;
using ClubManagement.Games.Models;
using ClubManagement.Games.Views;
using ClubManagement.Games.Service;
using ClubManagement.Games.Repositories;

namespace ClubManagement.Games.Presenters
{
    class MatchListPresenter
    {
        IMatchListView _view;
        IMatchRepository _repository;
        MatchSearchModel _model;
        MatchService _service;
        public MatchListPresenter(IMatchListView view, IMatchRepository repository)
        {
            this._view = view;
            this._repository = repository;
            this._model = new MatchSearchModel();
            this._service = new MatchService(repository);
            //뷰이벤트 핸들러 설정
            this._view.EditMatchEvent += EditGame;
            this._view.AddMatchEvent += AddGame;
            this._view.SearchMatchEvent += SearchGame;
            this._view.SearchPlayerEvent += SearchPlayerList;
            this._view.EditPlayerEvent += EidtPlayerList;

            LoadMatchList();
        }

        /// <summary>
        /// 뷰에서 검색 조건을 가져와 모델에 반영하고, 경기 목록을 조회해 뷰에 바인딩.
        /// </summary>
        private void LoadMatchList()
        {
            try
            {
                _model.FromDate = _view.MatchFromDate;
                _model.ToDate = _view.MatchToDate;
                _model.MatchType = _view.MatchType ?? 0;
                _model.ExcludeType = _view.ExcludeType;
                DataTable dataSource = _service.LoadMatchList(_model);
                _view.SetGameListBinding(dataSource);
            }
            catch (Exception ex) 
            { 
                _view.ShowMessage(ex.Message); 
            }

        }

        /// <summary>
        /// 특정 경기 코드에 해당하는 선수 목록을 조회하고 뷰에 바인딩.
        /// </summary>
        /// <param name="code">경기 코드 (MatchCode)</param>
        private void LoadPlayerList(int code)
        {
            try
            {
                // 서비스에서 해당 경기의 선수 목록을 DataTable 형태로 가져옴
                DataTable players = _service.LoadPlayerList(code);

                // 조회된 선수 목록을 뷰에 바인딩 (예: DataGridView, ListView 등)
                _view.SetPlayerListBinding(players);
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        /// <summary>
        /// 선수 목록 조회 버튼 클릭 시 호출.
        /// 현재 선택된 경기 코드로 선수 목록을 조회하고 뷰에 표시.
        /// </summary>
        private void SearchPlayerList(object sender, EventArgs e)
        {
            try
            {
                // 뷰에서 현재 선택된 경기 코드 가져오기 (없으면 0으로 처리)
                int code = _view.GetMatchCode ?? 0;

                // 코드가 0 이하일 경우 유효한 경기 없음 → 사용자 알림
                if (code <= 0)
                {
                    _view.ShowMessage("조회된 모임이 없습니다.");
                    return; // 더 이상 진행하지 않음
                }

                // 유효한 코드일 경우 선수 목록을 조회하고 뷰에 바인딩
                LoadPlayerList(code);
            }
            catch (Exception ex)
            {
                // 조회 중 오류 발생 시 사용자에게 메시지 출력
                _view.ShowMessage(ex.Message);
            }
        }

        private void SearchGame(object sender, EventArgs e)
        {
            LoadMatchList();
        }

        /// <summary>
        /// 참가자 리스트 수정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EidtPlayerList(object sender, EventArgs e)
        {
            try
            {
                //선택 모임 코드 설정
                int code = _view.GetMatchCode.Value;

                //참가자 설정 뷰, 리포지토리, 프리젠터 생성
                IMatchPlayerManageView view = new MatchPlayerManageView();
                IMatchRepository repository = _repository;
                MatchPlayerManagePresenter presenter = new MatchPlayerManagePresenter(view, repository);

                // 선택된 경기 코드로 참가자 데이터 로드
                presenter.PlayerManageCall(code);
                
                //폼 종료 후 최신 상태 차감자 조회
                view.ShowForm();
                LoadPlayerList(code);
            }
            catch (Exception ex) { _view.ShowMessage(ex.Message); }
        }

        /// <summary>
        /// 새로운 모임 추가 등록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddGame(object sender, EventArgs e)
        {
            try
            {
                //모임 상세 정보 뷰, 리포지토리, 프리젠터 생성
                IMatchDetailView view = new MatchDetailView();
                IMatchRepository repository = _repository;
                new MatchDetailPresenter(view, repository);
                view.ShowForm();

                //생성 완료 후 최신 상태로 조회
                LoadMatchList();
            }
            catch (Exception ex) { _view.ShowMessage(ex.Message); }
        }

        /// <summary>
        /// 모임 정보 수정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditGame(object sender, EventArgs e)
        {
            try
            {
                // 선택 모임 코드 지정
                int code = _view.GetMatchCode.Value;

                //모임 상세 정보 뷰, 리포지토리, 프리젠터 생성
                IMatchDetailView view = new MatchDetailView();
                IMatchRepository repository = _repository;
                MatchDetailPresenter presenter = new MatchDetailPresenter(view, repository);

                //수정 후 최시 모임 목록으로 조회, 종료 후 참가자 리스트 조회
                presenter.LoadMatch(code);
                view.ShowForm();
                LoadMatchList();
            }
            catch (Exception ex) { _view.ShowMessage(ex.Message); }
        }
    }
}
