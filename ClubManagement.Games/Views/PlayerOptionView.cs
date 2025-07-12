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

        /// <summary>
        /// 폼을 닫음.
        /// </summary>
        public void CloseForm()
        {
            this.Close();
        }

        /// <summary>
        /// 플레이어 옵션 데이터를 폼에 표시.
        /// </summary>
        /// <param name="player">표시할 플레이어 정보</param>
        public void SetPlayerOption(PlayerInfoDto player)
        {
            _player = player;

            // 이름 표시
            lblPlayerName.Text = player.PlayerName;

            // 각 항목에 맞는 색상/아이콘 설정
            SetLabelColor(_player.Gender, lblIconGender, lblGenderHandi);
            SetLabelColor(_player.IsPro, lblIconPro, lblIsPro);
            SetLabelColor(_player.IndividualSide, lblIconSide, lblSideGame);
            SetLabelColor(_player.AllCoverSide, lblIconAllcover, lblAllcoverGame);

            // 핸디캡 점수 텍스트 박스 표시 2025-07-16 추가
            txtHandi.Text = _player.Handycap.ToString();
        }

        /// <summary>
        /// 마우스 위치 기준으로 폼을 화면에 표시.
        /// </summary>
        public void ShowForm()
        {
            int offsetX = this.Width;

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Cursor.Position.X - offsetX, Cursor.Position.Y);

            this.ShowDialog();
        }

        /// <summary>
        /// 메시지 박스를 표시.
        /// </summary>
        /// <param name="message">표시할 메시지</param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "알림");
        }

        /// <summary>
        /// 버튼 이벤트 바인딩.
        /// </summary>
        private void ViewEvent()
        {
            btnClose.Click += (s, e) => CloseEvent?.Invoke(this, EventArgs.Empty);
            btnSave.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
            txtHandi.TextChanged += txtHandi_TextChanged;
        }

        /// <summary>
        /// 패널과 라벨 클릭 이벤트 바인딩.
        /// 각 항목 토글 기능 연결.
        /// </summary>
        private void BindEvents()
        {
            // 성별 핸디 토글
            pnlGenderHandi.Click += (s, e) => ToggleGender();
            lblIconGender.Click += (s, e) => ToggleGender();
            lblGenderHandi.Click += (s, e) => ToggleGender();

            // 프로 여부 토글
            pnlProHandi.Click += (s, e) => TogglePro();
            lblIconPro.Click += (s, e) => TogglePro();
            lblIsPro.Click += (s, e) => TogglePro();

            // 사이드 게임 여부 토글
            pnlSideGame.Click += (s, e) => ToggleSideGame();
            lblIconSide.Click += (s, e) => ToggleSideGame();
            lblSideGame.Click += (s, e) => ToggleSideGame();

            // 올커버 게임 여부 토글
            pnlAllcoverGame.Click += (s, e) => ToggleAllCover();
            lblIconAllcover.Click += (s, e) => ToggleAllCover();
            lblAllcoverGame.Click += (s, e) => ToggleAllCover();
        }

        /// <summary>
        /// 성별 토글 처리.
        /// </summary>
        private void ToggleGender()
        {
            _player.Gender = !_player.Gender;

            //체크 여부에 따라 핸디 변경 및 텍스트 박스 표시 2025-07-16 추가
            _player.Handycap = !_player.Gender ? _player.Handycap - 15 : _player.Handycap + 15;
            txtHandi.Text = _player.Handycap.ToString();

            SetLabelColor(_player.Gender, lblIconGender, lblGenderHandi);
        }

        /// <summary>
        /// 프로 여부 토글 처리.
        /// </summary>
        private void TogglePro()
        {
            _player.IsPro = !_player.IsPro;
            //체크 여부에 따라 핸디 변경 및 텍스트 박스 표시 2025-07-16 추가
            _player.Handycap = !_player.IsPro ? _player.Handycap + 5 : _player.Handycap - 5;
            txtHandi.Text = _player.Handycap.ToString();

            SetLabelColor(_player.IsPro, lblIconPro, lblIsPro);
        }

        /// <summary>
        /// 사이드 게임 여부 토글 처리.
        /// </summary>
        private void ToggleSideGame()
        {
            _player.IndividualSide = !_player.IndividualSide;
            SetLabelColor(_player.IndividualSide, lblIconSide, lblSideGame);
        }

        /// <summary>
        /// 올커버 여부 토글 처리.
        /// </summary>
        private void ToggleAllCover()
        {
            _player.AllCoverSide = !_player.AllCoverSide;
            SetLabelColor(_player.AllCoverSide, lblIconAllcover, lblAllcoverGame);
        }

        /// <summary>
        /// 활성화 여부에 따라 아이콘과 색상 변경.
        /// </summary>
        /// <param name="isActive">활성화 상태</param>
        /// <param name="iconLabel">아이콘 라벨</param>
        /// <param name="textLabel">텍스트 라벨</param>
        private void SetLabelColor(bool isActive, Label iconLabel, Label textLabel)
        {
            var color = isActive ? Color.LimeGreen : Color.Gray;

            // 아이콘 변경 (활성화: 체크, 비활성화: X)
            iconLabel.Text = isActive ? "\uE930" : "\uEA3F";

            iconLabel.ForeColor = color;
            textLabel.ForeColor = color;
        }

        private void txtHandi_TextChanged(object sender, EventArgs e)
        {
            _player.Handycap = Convert.ToInt32(txtHandi.Text);
        }
    }
}
