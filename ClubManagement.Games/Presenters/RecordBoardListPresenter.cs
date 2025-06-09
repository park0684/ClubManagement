using System;
using System.Data;
using ClubManagement.Common.Hlepers;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Service;
using ClubManagement.Games.Views;

namespace ClubManagement.Games.Presenters
{
    public class RecordBoardListPresenter
    {
        IRecordboardListView _view;
        IMatchRepository _repository;
        MatchSearchModel _model;
        MatchService _service;
        public RecordBoardListPresenter(IRecordboardListView view, IMatchRepository repository)
        {
            this._view = view;
            this._repository = repository;
            this._model = new MatchSearchModel();
            this._service = new MatchService(_repository);
            this._view.RecordBoardRegistEvent += ScoreBoardSetRegist;
            this._view.RecordBoardEditEvent += RecordBoardSetEidt;
            this._view.RecordBoarSelectedEvent += ScoreRecordSelected;
            this._view.SearchRecordBoardEvnt += SearchMatchList;
            LoadMatchList();
        }

        /// <summary>
        /// 모임 목록을 로드하고 뷰에 데이터 바인딩.
        /// 검색 조건은 FromDate, ToDate, IsRecordBoardRegisted = true, MatchType = 0 으로 고정.
        /// </summary>
        private void LoadMatchList()
        {
            // 뷰의 검색 조건을 모델에 반영
            _model.FromDate = _view.FromDate;
            _model.ToDate = _view.ToDate;
            _model.IsRecordBoardRegisted = true;  // 기록판 등록된 모임만 조회
            _model.MatchType = 0;                 // MatchType 0 = 전체 (또는 기본값)

            // 서비스에서 모임 목록 조회
            DataTable source = _service.LoadMatchList(_model);

            // 조회된 데이터에 type 컬럼 추가 (문자열 타입)
            source.Columns.Add("type", typeof(string));

            // match_type 코드 → 문자열 변환하여 type 컬럼 채움
            foreach (DataRow row in source.Rows)
            {
                row["type"] = GameHelper.GetMatchType(Convert.ToInt32(row["match_type"]));
            }

            // 뷰에 최종 데이터 바인딩
            _view.SetDataBinding(source);
        }

        /// <summary>
        /// 모임 목록 검색 버튼 클릭 시 호출.
        /// 단순히 LoadMatchList 메서드 실행.
        /// </summary>
        private void SearchMatchList(object sender, EventArgs e)
        {
            LoadMatchList();
        }

        /// <summary>
        /// 경기기록 버튼 클릭 시 호출.
        /// 선택된 모임 코드의 기록 정보를 로드하고 게임기록 화면을 표시.
        /// </summary>
        private void ScoreRecordSelected(object sender, EventArgs e)
        {
            // 현재 선택된 모임 코드
            int code = _view.GetMatchCode.Value;

            // 점수판 뷰/리포지토리/프리젠터 생성
            IRecordBoardView view = new RecordBoardView();
            IRecordBoardRepository repository = new RecordBoardRepository();
            RecordBoardPresenter presenter = new RecordBoardPresenter(view, repository);

            // 모임 정보 로드
            presenter.GetMatchInfo(code);

            // 점수판 폼 표시
            view.ShowForm();
        }

        /// <summary>
        /// 게임기록 설정(수정) 버튼 클릭 시 호출.
        /// 선택된 모임의 게임 목록을 로드하여 점수판 설정 화면 표시.
        /// </summary>
        private void RecordBoardSetEidt(object sender, EventArgs e)
        {
            // 현재 선택된 모임 코드
            int code = _view.GetMatchCode.Value;

            // 점수판 등록(수정) 뷰/리포지토리/프리젠터 생성
            IRecordBoardRegistView view = new RecordBoardRegistView();
            IRecordBoardRepository repository = new RecordBoardRepository();
            RecordboardRegistPresenter presenter = new RecordboardRegistPresenter(view, repository);

            // 모임의 게임 목록 로드
            presenter.LoadGameList(code);

            // 게임기록 등록/수정 폼 표시
            view.ShowForm();
        }

        /// <summary>
        /// 게임기록 설정(신규 등록) 버튼 클릭 시 호출.
        /// 게임기록 등록 화면을 새로 생성하고 표시.
        /// </summary>
        private void ScoreBoardSetRegist(object sender, EventArgs e)
        {
            // 게임기록 등록 뷰/리포지토리/프리젠터 생성
            IRecordBoardRegistView view = new RecordBoardRegistView();
            IRecordBoardRepository repository = new RecordBoardRepository();
            new RecordboardRegistPresenter(view, repository);

            // 게임기록 등록 폼 표시
            view.ShowForm();
        }
    }
}
