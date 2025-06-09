using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;
using ClubManagement.Games.Views;

namespace ClubManagement.Games.Presenters
{
    public class PlayerOtionPresenter
    {
        IPlayerOptionView _view;
        PlayerInfoDto _player;
        public event Action<PlayerInfoDto> UpdatePlayer;

        public PlayerOtionPresenter(IPlayerOptionView view, PlayerInfoDto player)
        {
            _view = view;
            _player = player;
            _view.CloseEvent += CloseForm;
            _view.SaveEvent += SavePlayer;
            _view.SetPlayerOption(_player);
        }

        /// <summary>
        /// 플레이어 저장 버튼 클릭 시 호출.
        /// 업데이트 이벤트를 발생시키고 폼을 닫음.
        /// </summary>
        private void SavePlayer(object sender, EventArgs e)
        {
            // UpdatePlayer 이벤트가 구독되어 있으면 호출 (뷰의 UpdatePlayer 데이터를 전달)
            UpdatePlayer?.Invoke(_view.UpdatePlayer);

            // 폼 닫기
            _view.CloseForm();
        }

        /// <summary>
        /// 폼 종료 처리
        /// </summary>
        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
