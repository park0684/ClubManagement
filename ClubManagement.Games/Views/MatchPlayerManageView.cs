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
    public partial class MatchPlayerManageView : Form,IMatchPlayerManageView
    {
        public MatchPlayerManageView()
        {
            InitializeComponent();
            ViewEvent();
            this.Text = "참가자 설정";
        }
        
        public string SearchWord
        {
            get { return txtSearchWord.Text; }
            set { txtSearchWord.Text = value; }
        }

        public bool SecessMember
        {
            get { return chkSecessMember.Checked; }
            set { chkSecessMember.Checked = value; }
        }


        public event EventHandler SearchMemberEvent;
        public event EventHandler<PlayerButtonEventArgs> PlayerAddEvent;
        public event EventHandler<PlayerButtonEventArgs> PlayerRemoveEvent;
        public event EventHandler SavePlayerListEvent;
        public event EventHandler CloseEvent;
        public event EventHandler AddGuestEvent;

        /// <summary>
        /// 폼 종료
        /// </summary>
        public void CloseForm()
        {
            this.Close();
        }

        /// <summary>
        /// 폼을 화면 중앙에서 모달 다이얼로그로 표시.
        /// </summary>
        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowDialog();
        }
        /// <summary>
        /// 회원 리스트를 버튼 형태로 생성하여 flpMemberList에 추가.
        /// </summary>
        /// <param name="members">회원 리스트</param>
        public void SetMemberList(List<PlayerInfoDto> members)
        {
            flpMemberList.Controls.Clear();  // 기존 컨트롤 제거

            foreach (var member in members)
            {
                // 회원 버튼 생성
                Button btn = new Button
                {
                    Size = new Size(90, 50),
                    FlatStyle = FlatStyle.Flat,
                    Text = member.PlayerName,
                    Tag = member.MemberCode,
                    BackColor = member.IsSelected
                        ? Color.FromArgb(65, 100, 223)    // 이미 선택된 회원 색
                        : Color.FromArgb(54, 178, 221)    // 미선택 회원 색
                };

                // 클릭 시 PlayerAddEvent 발생
                btn.Click += (sender, e) => PlayerAddEvent?.Invoke(
                    this,
                    new PlayerButtonEventArgs((int)btn.Tag, btn.Text)
                );

                // 패널에 버튼 추가
                flpMemberList.Controls.Add(btn);
            }
        }

        /// <summary>
        /// 플레이어 리스트를 버튼 형태로 생성하여 flpPlayerList에 추가.
        /// </summary>
        /// <param name="players">플레이어 리스트</param>
        public void SetPlayerList(List<PlayerInfoDto> players)
        {
            flpPlayerList.Controls.Clear();  // 기존 컨트롤 제거

            foreach (var player in players)
            {
                // 플레이어 버튼 생성
                Button btn = new Button
                {
                    Size = new Size(90, 50),
                    FlatStyle = FlatStyle.Flat,
                    Text = player.PlayerName,
                    Tag = player.MemberCode,
                    BackColor = Color.FromArgb(54, 178, 221) // 고정된 플레이어 색
                };

                // 클릭 시 PlayerRemoveEvent 발생
                btn.Click += (sender, e) => PlayerRemoveEvent?.Invoke(
                    this,
                    new PlayerButtonEventArgs((int)btn.Tag, btn.Text)
                );

                // 패널에 버튼 추가
                flpPlayerList.Controls.Add(btn);
            }
        }

        /// <summary>
        /// 회원 버튼 색상 갱신.
        /// </summary>
        /// <param name="memberCode">대상 회원 코드</param>
        /// <param name="isAdded">선택 여부</param>
        public void UpdateButtonColor(int memberCode, bool isAdded)
        {
            foreach (Control control in flpMemberList.Controls)
            {
                if (control is Button btn && (int)btn.Tag == memberCode)
                {
                    // 선택 여부에 따라 색상 변경
                    btn.BackColor = isAdded
                        ? Color.FromArgb(65, 100, 223)   // 선택 색
                        : Color.FromArgb(54, 178, 221);  // 미선택 색

                    break;  // 찾았으면 루프 종료
                }
            }
        }

        /// <summary>
        /// 메시지 박스를 표시.
        /// </summary>
        /// <param name="message">표시할 메시지</param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// 버튼과 컨트롤 이벤트를 바인딩.
        /// </summary>
        private void ViewEvent()
        {
            // 검색 버튼 클릭 → SearchMemberEvent 발생
            btnSearch.Click += (s, e) => SearchMemberEvent?.Invoke(this, EventArgs.Empty);

            // 닫기 버튼 클릭 → CloseEvent 발생
            btnClose.Click += (s, e) => CloseEvent?.Invoke(this, EventArgs.Empty);

            // 저장 버튼 클릭 → SavePlayerListEvent 발생
            btnSave.Click += (s, e) => SavePlayerListEvent?.Invoke(this, EventArgs.Empty);

            // Guest 추가 버튼 클릭 → AddGuestEvent 발생
            btnAddGuest.Click += (s, e) => AddGuestEvent?.Invoke(this, EventArgs.Empty);

            // 검색어 입력란에서 Enter 입력 → SearchMemberEvent 발생
            txtSearchWord.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    SearchMemberEvent?.Invoke(this, EventArgs.Empty);
            };
        }

    }
    /// <summary>
    /// 자동 생성되는 버튼의 이벤트 생성
    /// </summary>
    public class PlayerButtonEventArgs : EventArgs
    {
        public int MemberCode { get; }
        public string MemberName { get; }

        public PlayerButtonEventArgs(int memberCode, string memberName)
        {
            MemberCode = memberCode;
            MemberName = memberName;
        }
    }
}
