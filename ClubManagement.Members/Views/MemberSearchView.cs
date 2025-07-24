using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ClubManagement.Members.Views
{
    public partial class MemberSearchView : Form,IMemberSearchView
    {
        public MemberSearchView()
        {
            InitializeComponent();
            ViewEvent();
            this.Text = "회원 검색";
        }

        public string SearchWord
        {
            get { return txtSearchWord.Text; }
            set { txtSearchWord.Text = value; }
        }

        public int MemberCode
        {
            get; private set;
        }

        public string MemberName
        { get; private set; }
        public bool IsInculde { get => chkSecetInculde.Checked; set => chkSecetInculde.Checked = value; }

        public event EventHandler MemberSeachEvent;
        public event EventHandler SelectMemberEvent;
        public event EventHandler CloseFormEvent;

        /// <summary>
        ///  폼 종료.
        /// </summary>
        public void CloseForm()
        {
            Close();
        }

        /// <summary>
        /// 회원 목록 버튼 생성
        /// </summary>
        /// <param name="members"></param>
        public void SetMemberList(DataTable members)
        {
            flpMemberList.Controls.Clear();
            foreach (DataRow member in members.Rows)
            {
                //회원 버튼 생성
                Button btn = new Button
                {
                    Size = new Size(90, 35),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(126, 112, 247),
                    ForeColor = Color.White,
                    Text = member["mem_name"].ToString(),
                    Tag = member["mem_code"],
                };
                btn.FlatAppearance.BorderSize = 0;

                //회원 버튼에 이벤트 설정
                btn.Click += (sender, e) =>
                {
                    MemberCode = (int)((Button)sender).Tag;
                    MemberName = ((Button)sender).Text;
                    SelectMemberEvent?.Invoke(this, EventArgs.Empty);
                };

                //플로우레이어 패널에 버튼 추가
                flpMemberList.Controls.Add(btn);
            }
            //스크롤 활성화
            flpMemberList.AutoScroll = true;
        }

        /// <summary>
        /// 부모창 가운데 다이얼로그 싫애
        /// </summary>
        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }

        /// <summary>
        /// 메시지 박스 표시
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "알림");
        }

        /// <summary>
        /// 이벤트 등록
        /// </summary>
        private void ViewEvent()
        {
            btnSearch.Click += (s, e) => MemberSeachEvent?.Invoke(this, EventArgs.Empty);
            btnClose.Click += (s, e) => CloseFormEvent?.Invoke(this, EventArgs.Empty);
            txtSearchWord.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    MemberSeachEvent?.Invoke(this, EventArgs.Empty);
            };
        }
    }
}
