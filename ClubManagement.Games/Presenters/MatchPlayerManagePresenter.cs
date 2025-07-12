using ClubManagement.Games.DTOs;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Service;
using ClubManagement.Games.Views;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ClubManagement.Games.Presenters
{
    public class MatchPlayerManagePresenter
    {
        IMatchPlayerManageView _view;
        IMatchRepository _repository;
        MatchModel _model;
        MatchSearchModel _searchModel;
        MatchService _service;

        public MatchPlayerManagePresenter(IMatchPlayerManageView view, IMatchRepository repository)
        {
            _view = view;
            _repository = repository;
            _model = new MatchModel();
            _searchModel = new MatchSearchModel();
            _service = new MatchService(_repository);
            this._view.SearchMemberEvent += SearchMember;;
            this._view.AddGuestEvent += GuestAdd;
            this._view.PlayerAddEvent += PlayerAdd;
            this._view.PlayerRemoveEvent += PlayerRemove;
            this._view.SavePlayerListEvent += PlayerUpdate;
            this._view.CloseEvent += CloseForm;
        }

        /// <summary>
        /// 게스트 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GuestAdd(object sender, EventArgs e)
        {
            try
            {
                // 게스트 추가 등록 뷰, 리포지토리, 프리젠터 생성
                IGuestAddView view = new GuestAddView();
                IMatchRepository repository = new MatchRepository();
                GuestAddPresenter presenter = new GuestAddPresenter(view);
                List<PlayerInfoDto> guests = new List<PlayerInfoDto>();
                view.ShowForm();
                guests = presenter.getGuestList();
                if (guests == null || guests.Count == 0)
                    return;

                //모델에 참가자 추가 
                _model.PlayerList.AddRange(guests);
                //view에 참가자 출력
                _view.SetPlayerList(_model.PlayerList);
            }
            catch (Exception ex) 
            { _view.ShowMessage(ex.Message); }
        }

        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }

        // <summary>
        /// 플레이어 제거 버튼 클릭 시 호출.
        /// 모델에서 해당 플레이어를 제거하고 뷰를 갱신.
        /// </summary>
        /// <param name="sender">이벤트 발생 소스</param>
        /// <param name="e">플레이어 버튼 이벤트 인자 (MemberName, MemberCode)</param>
        private void PlayerRemove(object sender, PlayerButtonEventArgs e)
        {
            // 이벤트 인자로부터 회원명과 회원코드 추출
            string memberName = e.MemberName;
            int memberCode = (int)e.MemberCode;

            // 모델의 플레이어 리스트에서 제거할 대상 찾기
            var playerToRemove = _model.PlayerList.FirstOrDefault(p => p.MemberCode == memberCode && p.PlayerName == memberName);
            if (playerToRemove != null)
            {
                // 모델에서 플레이어 제거
                _model.PlayerList.Remove(playerToRemove);

                // 회원 코드가 0이 아닌 회원 등록된 경우, 버튼 색상 초기화
                if (memberCode != 0)
                    _view.UpdateButtonColor(memberCode, false);

                // 최신 플레이어 리스트를 뷰에 반영
                _view.SetPlayerList(_model.PlayerList);
            }
        }

        /// <summary>
        /// 플레이어 추가 버튼 클릭 시 호출.
        /// 모델에 플레이어를 추가하고 뷰에 반영.
        /// </summary>
        private void PlayerAdd(object sender, PlayerButtonEventArgs e)
        {
            try
            {
                // 이벤트 인자에서 회원 이름과 코드 추출
                string memberName = e.MemberName;
                int memberCode = (int)e.MemberCode;

                // 이미 추가된 플레이어인지 확인 (중복 방지)
                if (_model.PlayerList.Any(p => p.MemberCode == memberCode))
                {
                    return;// 이미 추가된 경우 아무 작업도 하지 않음
                }

                // 모델의 전체 회원 리스트에서 해당 회원 정보 찾기
                var member = _model.MemberList.FirstOrDefault(m => m.MemberCode == memberCode);
                if (member == null)
                    return; //해당 회원 정보가 없으면 종료
                int handi = (member.IsPro == true ? -5 : 0) + (member.Gender == true ? 15 : 0);
                // 플레이어 정보 DTO 생성 및 PlayerList에 추가
                _model.PlayerList.Add(new PlayerInfoDto
                {
                    MemberCode = member.MemberCode,     // 회원 코드
                    PlayerName = member.PlayerName,     // 이름
                    Gender = member.Gender,             // 성별
                    IsPro = member.IsPro,               // 프로 여부
                    IsSelected = member.IsSelected,     // 선택 여부 (기존 값 반영)
                    Handycap = (member.IsPro == true ? -5 : 0) + (member.Gender == true ? 15 : 0) // 핸디캡
            });

                // 버튼 색상 변경 (선택 상태 표시)
                _view.UpdateButtonColor(memberCode, true);

                // 뷰의 플레이어 리스트 갱신
                _view.SetPlayerList(_model.PlayerList);
            }
            catch (Exception ex) 
            { 
                _view.ShowMessage(ex.Message); 
            }
        }

        /// <summary>
        /// 참가자 관리 호출 진입점.
        /// 주어진 경기 코드로 참가자 목록과 회원 목록을 로드하여 화면에 반영.
        /// </summary>
        /// <param name="matchCode">참가자 관리를 위한 모임 코드</param>
        public void PlayerManageCall(int matchCode)
        {
            try
            {
                // 모델에 현재 모임 코드 설정
                _model.MatchCode = matchCode;

                // 해당 모임 코드에 대한 참가자 목록 로드
                LoadPlayer(matchCode);

                // 참가자 추가를 위한 전체 회원 목록 로드
                LoadMember();
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        /// <summary>
        /// 참가자 정보 저장 버튼 클릭 시 호출.
        /// 모델의 선수 정보를 서비스에 저장하고 폼을 닫음.
        /// </summary>
        private void PlayerUpdate(object sender, EventArgs e)
        {
            try
            {
                // 서비스에 현재 모델(PlayerList, MatchCode 등 포함)을 전달하여 선수 정보 저장
                _service.PlayerUpdate(_model);

                // 저장 완료 후 폼 닫기
                _view.CloseForm();
            }
            catch (Exception ex) 
            { 
                _view.ShowMessage(ex.Message); 
            }
        }

        /// <summary>
        /// 회원 검색 버튼 클릭 시 호출.
        /// 뷰에서 검색 조건을 가져와 모델에 반영하고 회원 목록을 로드.
        /// </summary>
        private void SearchMember(object sender, EventArgs e)
        {
            // 뷰에서 검색어 입력값을 가져와 검색 모델에 설정
            _searchModel.SearchWord = _view.SearchWord;

            // 뷰에서 탈퇴 회원 포함 여부를 가져와 검색 모델에 설정
            _searchModel.IncludeSecessMember = _view.SecessMember;

            // 검색 조건에 맞는 회원 목록 로드
            LoadMember();
        }

        /// <summary>
        /// 회원 목록을 로드하여 모델과 뷰에 반영.
        /// 검색 조건과 현재 선택된 참가자 목록을 기반으로 데이터 조회.
        /// </summary>
        private void LoadMember()
        {
            try
            {
                // 서비스에서 회원 목록을 검색 조건과 현재 참가자 목록을 기준으로 조회
                _model.MemberList = _service.LoadMember(_searchModel, _model.PlayerList);

                // 조회된 회원 목록을 뷰에 바인딩
                _view.SetMemberList(_model.MemberList);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        /// <summary>
        /// 주어진 모임 코드에 대한 참가자 목록을 로드하여 모델과 뷰에 반영.
        /// </summary>
        /// <param name="code">경기 코드 (MatchCode)</param>
        private void LoadPlayer(int code)
        {
            try
            {
                // 서비스에서 해당 모임 코드에 대한 선수 목록 조회
                _model.PlayerList = _service.LoadPlayer(code);

                // 조회된 참가자 목록을 뷰에 바인딩
                _view.SetPlayerList(_model.PlayerList);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }
    }
}
