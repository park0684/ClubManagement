using ClubManagement.Games.DTOs;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Service;
using ClubManagement.Games.Views;
using System;

namespace ClubManagement.Games.Presenters
{
    public class RecordboardRegistPresenter
    {
        private IRecordBoardRegistView _view;
        private IRecordBoardRepository _repository;
        private RecordBoardModel _model;
        private RecordBoardService _service;
        public RecordboardRegistPresenter(IRecordBoardRegistView view, IRecordBoardRepository repository)
        {
            this._view = view;
            this._repository = repository;
            this._model = new RecordBoardModel();
            this._service = new RecordBoardService(_repository);
            this._view.CloseFormEvent += CloseForm;
            this._view.AddOrderEvent += AddGame;
            this._view.EditPlayerEvent += EidtPlayer;
            this._view.SaveOrderEvent += SaveGames;
            this._view.SelectGameEvent += SelectMatch;
            this._view.MatchTitle = "";
        }

        /// <summary>
        /// 모임 선택 버튼 클릭 시 호출.
        /// 모임 선택 팝업을 열고, 선택된 값을 모델과 뷰에 반영.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectMatch(object sender, EventArgs e)
        {
            // 모임(경기) 검색 뷰, 리포지토리, 프리젠터 생성
            ISearchMatchListView view = new SearchMatchListView();
            IMatchRepository repository = new MatchRepository();
            new SearchMatchListPresenter(view, repository);

            // 선택 이벤트 구독: 사용자가 모임을 선택하면 호출
            view.SelectedMatchEvent += (s, args) =>
            {
                // 선택된 모임 정보를 모델에 반영
                _model.MatchCode = view.MatchCode;
                _model.MatchTitle = view.MatchTitle;

                // 팝업 폼 닫기
                view.CloseForm();
            };

            // 모임 검색 폼 표시
            view.ShowForm();

            // 선택된 모임 정보를 메인 뷰에 반영
            _view.MatchTitle = _model.MatchTitle;    // 모임명
            _view.OrderCount = _model.MatchCode;     // 모임 코드 (여기선 주문수 또는 순번 대신 코드 활용)

            // 이미 등록된 기록인지 확인
            if (view.IsRecodeRegisted == true)
            {
                // 이미 등록된 경우 사용자에게 알림
                view.ShowMessage("이미 등록된 모임입니다");

                // 해당 모임의 게임 목록 로드
                LoadGameList(_model.MatchCode);
                return; // 추가 작업 없이 종료
            }

            // 선택된 모임 코드가 유효하면 선수 목록 로드
            if (_model.MatchCode > 0)
                LoadPlayer(_model.MatchCode);
        }

        /// <summary>
        /// 게임 저장 버튼 클릭 시 호출.
        /// 현재 모델의 게임 데이터를 서비스에 저장하고 폼 종료
        /// </summary>
        private void SaveGames(object sender, EventArgs e)
        {
            try
            {
                // 현재 모델(_model)의 게임 데이터를 서비스에 전달하여 저장 (Insert)
                _service.InsertGames(_model);

                // 저장이 완료되면 폼을 종료
                _view.CloseForm();
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);  
            }
        }

        /// <summary>
        /// 플레이어 관리 버튼 클릭 시 호출.
        /// 플레이어 관리 화면을 열어 현재 모임의 플레이어 정보를 수정하고, 완료 후 목록을 새로 로드.
        /// </summary>
        private void EidtPlayer(object sender, EventArgs e)
        {
            // 현재 모델에 설정된 모임 코드 가져오기
            int match = _model.MatchCode;

            // 플레이어 관리용 뷰, 리포지토리, 프리젠터 생성
            IMatchPlayerManageView view = new MatchPlayerManageView();
            IMatchRepository repository = new MatchRepository();
            MatchPlayerManagePresenter presenter = new MatchPlayerManagePresenter(view, repository);

            // 선택된 모임 코드에 대한 플레이어 데이터 로드 및 관리 준비
            presenter.PlayerManageCall(match);

            // 플레이어 관리 폼 표시
            view.ShowForm();

            // 플레이어 관리 화면 닫힌 후 최신 플레이어 목록 다시 로드하여 뷰에 반영
            LoadPlayer(match);
        }

        /// <summary>
        /// 게임 추가 버튼 클릭 시 호출.
        /// 현재 모임에 새 게임 순서를 추가하고 뷰에 반영.
        /// </summary>
        private void AddGame(object sender, EventArgs e)
        {
            // 모임이 지정되지 않은 경우 사용자에게 알림 후 종료
            if (_model.MatchCode == 0)
            {
                _view.ShowMessage("모임을 먼저 지정하세요");
                return;
            }

            // 새 게임 순번 결정 (기존 게임 수 + 1)
            int gameCount = _model.GameList.Count + 1;

            // 새 게임 순서 객체 생성 (기본 값 세팅)
            GameOrderDto gameOrder = new GameOrderDto
            {
                GameSeq = gameCount,        // 게임 순번
                GameType = 1,               // 기본 게임 타입 (예: 정기전)
                PlayerCount = 1,            // 기본 플레이어 수
                AllCoverGame = false,       // 올커버 여부 (기본 false)
                PersonalSideGame = false    // 개인 사이드 게임 여부 (기본 false)
            };

            // 모델의 게임 리스트에 추가
            _model.GameList.Add(gameOrder);

            // 뷰에 새 게임 순서를 표시
            _view.AddNewOrder(gameOrder);
        }

        /// <summary>
        /// 폼 종료 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }

        /// <summary>
        /// 지정된 모임 코드로 게임 목록을 로드하고 뷰에 반영.
        /// 게임 목록과 플레이어 목록을 갱신하고, 관련 버튼 상태를 업데이트.
        /// </summary>
        /// <param name="matchCode">모임 코드</param>
        public void LoadGameList(int matchCode)
        {
            // 모델에 현재 모임 코드 설정
            _model.MatchCode = matchCode;

            // 서비스에서 해당 모임의 게임 목록을 가져와 모델에 저장
            _model.GameList = _service.LoadGames(matchCode);

            // 뷰에 게임(게임 순서) 목록을 표시
            _view.LoadOrder(_model.GameList);

            // 해당 모임의 플레이어 목록을 로드하고 뷰에 반영
            LoadPlayer(matchCode);

            // 뷰의 모임 관련 버튼 상태를 갱신 (예: 추가/편집 버튼 활성화 여부)
            _view.SetMatchButton();
        }

        /// <summary>
        /// 지정된 모임 코드로 플레이어(참가자) 목록을 로드하고 뷰에 반영.
        /// </summary>
        /// <param name="match">모임 코드</param>
        public void LoadPlayer(int match)
        {
            // 서비스에서 해당 모임의 플레이어(참가자) 목록을 조회하여 모델에 저장
            _model.PlayerList = _service.LoadAttendees(match);

            // 뷰에 플레이어 목록을 표시
            _view.LoadPlayer(_model.PlayerList);
        }
    }
}
