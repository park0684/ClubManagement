using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Views;

namespace ClubManagement.Games.Presenters
{
    public class GuestAddPresenter
    {
        IGuestAddView _view;
        MatchModel _model;

        public GuestAddPresenter(IGuestAddView viewe)
        {
            _view = viewe;
            _model = new MatchModel();
            _view.AddGuestEvent += AddGuest;
            _view.SaveGuestEvent += SaveGuest;
            _view.CloseFormEvevnt += CloseForm;
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

        /// <summary>
        /// 뷰에서 입력된 게스트 정보를 모델에 저장하고 폼을 닫음.
        /// </summary>
        private void SaveGuest(object sender, EventArgs e)
        {
            // 뷰에서 게스트 입력 데이터를 가져옴 (GuestAddView → List<PlayerInfoDto>)
            var guestList = _view.GusetData();

            // 입력된 게스트 데이터를 PlayerInfoDto 리스트로 변환하여 모델에 저장
            _model.GuestList = guestList.Select(g => new PlayerInfoDto
            {
                PlayerName = g.PlayerName, // 이름
                Gender = g.Gender, // 성별
                IsPro = g.IsPro, //프로 여부
                Handycap = g.Handycap, // 핸디캡
                IsSelected = true, //선택 여부 
                MemberCode = 0 //회원이 아니므로 회원코드 0으로 기록
            }).ToList();

            _view.CloseForm();
        }

        /// <summary>
        /// 게스트 추가 버튼 클릭 시 호출.
        /// 뷰에 새로운 게스트 입력 패널을 추가하고 이름 입력란 초기화.
        /// </summary>
        private void AddGuest(object sender, EventArgs e)
        {
            _view.AddGuestPanel();
            _view.GuestName = "";
        }

        public List<PlayerInfoDto> getGuestList()
        {
            return _model.GuestList;
        }
    }
}
