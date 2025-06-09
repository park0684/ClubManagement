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

        /// <summary>
        /// 폼 종료.
        /// </summary>
        public void CloseForm()
        {
            this.Close();
        }

        /// <summary>
        /// 그룹 내 등록된 플레이어 버튼 생성.
        /// 클릭 시 PlayerRemoveEvent 발생 (플레이어 제거용)
        /// </summary>
        /// <param name="particitands">그룹에 등록된 플레이어 목록</param>
        public void CreateAttendButton(List<PlayerInfoDto> particitands)
        {
            flpAttendList.Controls.Clear();

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

                // 클릭 시 제거 이벤트 호출
                btn.Click += (sender, e) => PlayerRemoveEvent?.Invoke(this, new participantButtonEventArgs((bool)btn.Tag, btn.Text));
                flpAttendList.Controls.Add(btn);
            }
        }

        /// <summary>
        /// 플레이어 버튼 색상 변경.
        /// 현재 그룹 여부 + 등록 상태에 따라 색상 변경.
        /// </summary>
        /// <param name="btn">대상 버튼</param>
        /// <param name="isCurrentGroup">현재 그룹 여부</param>
        /// <param name="isSelected">등록 여부</param>
        private void SetPlayerButtonColor(Button btn, bool isCurrentGroup, bool isSelected)
        {
            if (isSelected)
            {
                if (isCurrentGroup)
                    btn.BackColor = Color.SkyBlue;     // 현재 그룹 등록
                else
                    btn.BackColor = Color.IndianRed;   // 다른 그룹 등록
            }
            else
            {
                btn.BackColor = Color.LightGray;       // 미등록
            }
        }

        /// <summary>
        /// 전체 플레이어 버튼 생성.
        /// 그룹 내 등록/미등록 여부에 따라 색상 및 클릭 이벤트 부여.
        /// </summary>
        /// <param name="gameOrder">게임 순서 정보</param>
        /// <param name="groupNumber">현재 그룹 번호</param>
        /// <param name="players">플레이어 전체 리스트</param>
        public void CreatePlayerButton(GameOrderDto gameOrder, int groupNumber, List<PlayerInfoDto> players)
        {
            flpPlayerList.Controls.Clear();

            foreach (var player in players)
            {
                Button btn = new Button
                {
                    Size = new Size(90, 50),
                    FlatStyle = FlatStyle.Flat,
                    Text = player.PlayerName,
                    Tag = player.IsSelected
                };

                bool foundInCurrentGroup = false;
                bool isSeledted = false;

                // 그룹별 등록 상태 확인
                foreach (var group in gameOrder.Groups)
                {
                    var existPlayer = group.players.FirstOrDefault(p => p.PlayerName == player.PlayerName);

                    if (existPlayer != null)
                    {
                        btn.Tag = true;
                        isSeledted = true;

                        if (group.GroupNumber == groupNumber)
                            foundInCurrentGroup = true;

                        break; // 찾으면 루프 종료
                    }
                }

                // 색상 적용
                SetPlayerButtonColor(btn, foundInCurrentGroup, isSeledted);

                // 클릭 이벤트 → PlayerAddEvent 호출
                btn.Click += (sender, e) =>
                {
                    if (sender is Button clickedBtn)
                    {
                        PlayerAddEvent?.Invoke(this, new participantButtonEventArgs(Convert.ToBoolean(clickedBtn.Tag), clickedBtn.Text));
                    }
                };

                flpPlayerList.Controls.Add(btn);
            }
        }

        /// <summary>
        /// 폼을 중앙 모달로 표시.
        /// </summary>
        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }

        /// <summary>
        /// 메시지 박스 표시.
        /// </summary>
        /// <param name="message">메시지 내용</param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "알림");
        }

        /// <summary>
        /// 플레이어 버튼 색상 업데이트 (외부 요청 시 호출)
        /// </summary>
        /// <param name="player">플레이어 이름</param>
        /// <param name="isCurrentGroup">현재 그룹 여부</param>
        /// <param name="isSelected">등록 여부</param>
        public void UpdateButtonColor(string player, bool isCurrentGroup, bool isSelected)
        {
            foreach (Control control in flpPlayerList.Controls)
            {
                if (control is Button btn && btn.Text == player)
                {
                    SetPlayerButtonColor(btn, isCurrentGroup, isSelected);
                }
            }
        }

        /// <summary>
        /// Save / Close 버튼 이벤트 바인딩.
        /// </summary>
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
