using ClubManagement.Games.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManagement.Games.Views
{
    public partial class PlayerOptionView : Form,IPlayerOptionView
    {
        private PlayerInfoDto _player = new PlayerInfoDto();
        public PlayerInfoDto UpdatePlayer => _player;

        public PlayerOptionView()
        {
            InitializeComponent();
            this.ControlBox = false;
            this.Text = "참가자 설정";
            ViewEvent();
            BindEvents();

        }

        public event EventHandler SaveEvent;
        public event EventHandler CloseEvent;

        public void CloseForm()
        {
            this.Close();
        }

        public void SetPlayerOption(PlayerInfoDto player)
        {
            _player = player;
            lblPlayerName.Text = player.PlayerName;
            SetLabelColor(_player.Gender, lblIconGender, lblGenderHandi);
            SetLabelColor(_player.IsPro, lblIconPro, lblIsPro);
            SetLabelColor(_player.IndividualSide, lblIconSide, lblSideGame);
            SetLabelColor(_player.AllCoverSide, lblIconAllcover, lblAllcoverGame);
        }

        public void ShowForm()
        {
            int offsetX = this.Width;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Cursor.Position.X - offsetX, Cursor.Position.Y);
            this.ShowDialog();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "알림");
        }
        private void ViewEvent()
        {
            btnClose.Click += (s, e) => CloseEvent?.Invoke(this, EventArgs.Empty);
            btnSave.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
        }

        private void BindEvents()
        {
            pnlGenderHandi.Click += (s, e) => ToggleGender();
            lblIconGender.Click += (s, e) => ToggleGender();
            lblGenderHandi.Click += (s, e) => ToggleGender();

            pnlProHandi.Click += (s, e) => TogglePro();
            lblIconPro.Click += (s, e) => TogglePro();
            lblIsPro.Click += (s, e) => TogglePro();

            pnlSideGame.Click += (s, e) => ToggleSideGame();
            lblIconSide.Click += (s, e) => ToggleSideGame();
            lblSideGame.Click += (s, e) => ToggleSideGame();

            pnlAllcoverGame.Click += (s, e) => ToggleAllCover();
            lblIconAllcover.Click += (s, e) => ToggleAllCover();
            lblAllcoverGame.Click += (s, e) => ToggleAllCover();
        }
        private void ToggleGender()
        {
            _player.Gender = !_player.Gender;
            SetLabelColor(_player.Gender, lblIconGender, lblGenderHandi);
        }
        
        private void TogglePro()
        {
            _player.IsPro = !_player.IsPro;
            SetLabelColor(_player.IsPro, lblIconPro, lblIsPro);
        }
        private void ToggleSideGame()
        {
            _player.IndividualSide = !_player.IndividualSide;
            SetLabelColor(_player.IndividualSide, lblIconSide, lblSideGame);
        }
        private void ToggleAllCover()
        {
            _player.AllCoverSide = !_player.AllCoverSide;
            SetLabelColor(_player.AllCoverSide, lblIconAllcover, lblAllcoverGame);
        }
        private void SetLabelColor(bool isActive, Label iconLabel, Label textLabel)
        {
            var color = isActive ? Color.LimeGreen : Color.Gray;
            iconLabel.Text = isActive ? "\uE930" : "\uEA3F";
            iconLabel.ForeColor = color;
            textLabel.ForeColor = color;
        }


    }
}
