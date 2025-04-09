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

        private void SavePlayer(object sender, EventArgs e)
        {
            UpdatePlayer?.Invoke(_view.UpdatePlayer);
            _view.CloseForm();
        }

        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
