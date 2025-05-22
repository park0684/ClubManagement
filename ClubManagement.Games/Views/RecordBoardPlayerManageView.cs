using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public partial class RecordBoardPlayerManageView : Form,IRecordBoardPlayerManageView
    {
        public RecordBoardPlayerManageView()
        {
            InitializeComponent();
            ViewEvnet();
            this.Text = "팀원 설정";
        }

        public event EventHandler SaveEvent;
        public event EventHandler CloaseEvent;
        public event EventHandler<participantButtonEventArgs> PlayerAddEvent;
        public event EventHandler<participantButtonEventArgs> PlayerRemoveEvent;

        public void CloseForm()
        {
            this.Close();
        }
        /// <summary>
        /// 그룹내 참가 등록된 플레이어 버튼 생성
        /// 클릭 시 삭제
        /// </summary>
        /// <param name="particitands"></param>
        public void CreateAttendButton(List<PlayerInfoDto> particitands)
        {
            flpAttendList.Controls.Clear();
            //flpPlayerList.AutoScroll = true;
            foreach (var participant in particitands)
            {
                Button btn = new Button
                {
                    Size = new Size(90, 50),
                    FlatStyle = FlatStyle.Flat,
                    Text = participant.PlayerName,
                    Tag = participant.IsSelected,
                    BackColor = Color.FromArgb(54, 178, 221)
                };
                btn.Click += (sender, e) => PlayerRemoveEvent?.Invoke(this, new participantButtonEventArgs((bool)btn.Tag, btn.Text));
                flpAttendList.Controls.Add(btn);
            }
        }
        /// <summary>
        /// 그룹내 등록 또는 다른 그룹 등록된 플레이어 구분 목적으로 색상 변경
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="isCurrentGroup"></param>
        /// <param name="isSelected"></param>
        private void SetPlayerButtonColor(Button btn, bool isCurrentGroup, bool isSelected)
        {
            //int group = Convert.ToInt32(btn.Tag)
            if (isSelected)
            {
                if (isCurrentGroup == true)
                    btn.BackColor = Color.SkyBlue;  // 현재 그룹 등록
                else
                    btn.BackColor = Color.IndianRed;  // 다른 그룹 등록
            }
            else
            {
                btn.BackColor = Color.LightGray;  // 미등록
            }
        }
        /// <summary>
        /// 플레이어 버튼 생성
        /// 그룹내 등록 또는 다른 그룹 등록된 플레이어는 SetPlayerButtonColor 통해 색상으로 구분 및 작동 정지
        /// </summary>
        /// <param name="gameOrder"></param>
        /// <param name="groupNumber"></param>
        /// <param name="players"></param>
        public void CreatePlayerButton(GameOrderDto gameOrder, int groupNumber, List<PlayerInfoDto> players)
        {
            //패널 초기화
            flpPlayerList.Controls.Clear();

            //전체 플레이어 리스트 정보를 통해 플레이어 버튼 생성
            foreach(var player in players)
            {
                Button btn = new Button
                {
                    Size = new Size(90, 50),
                    FlatStyle = FlatStyle.Flat,
                    Text = player.PlayerName,
                    Tag = player.IsSelected,
                };
                bool foundInCurrentGroup = false;
                bool isSeledted = false;
                foreach (var group in gameOrder.Groups)
                {
                    var existPlayer = group.players.FirstOrDefault(p => p.PlayerName == player.PlayerName);

                    if (existPlayer != null)
                    {
                        btn.Tag = true;  // 등록된 상태
                        isSeledted = true;
                        if (group.GroupNumber == groupNumber)
                            foundInCurrentGroup = true;

                        break; // 찾았으면 더 볼 필요 없음
                    }
                }
                SetPlayerButtonColor(btn, foundInCurrentGroup, isSeledted);
                btn.Click += (sender, e) => {
                    if (sender is Button clickedBtn)
                    {
                        PlayerAddEvent?.Invoke(this, new participantButtonEventArgs(Convert.ToBoolean(clickedBtn.Tag), clickedBtn.Text));
                    }
                };
                flpPlayerList.Controls.Add(btn);
            }
        }

        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "알림");
        }

        public void UpdateButtonColor(string player,bool isCurrentGroup, bool isSelected)
        {
            foreach (Control control in flpPlayerList.Controls)
            {
                if (control is Button btn && btn.Text== player)
                {
                    if (isSelected)
                    {
                        if (isCurrentGroup == true)
                            btn.BackColor = Color.SkyBlue;  // 현재 그룹 등록
                        else
                            btn.BackColor = Color.IndianRed;  // 다른 그룹 등록
                    }
                    else
                    {
                        btn.BackColor = Color.LightGray;  // 미등록
                    }
                }
            }
        }

        private void ViewEvnet()
        {
            btnSave.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
            btnClose.Click += (s, e) => CloaseEvent?.Invoke(this, EventArgs.Empty);
        }

        
    }
    /// <summary>
    /// 자동 생성되는 버튼의 이벤트 생성
    /// </summary>
    public class participantButtonEventArgs : EventArgs
    {
        public bool Attend { get; }
        public string MemberName { get; }

        public participantButtonEventArgs(bool attend, string memberName)
        {
            Attend = attend;
            MemberName = memberName;
        }
    }
}
