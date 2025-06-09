using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public partial class GuestAddView : Form, IGuestAddView
    {
        public GuestAddView()
        {
            InitializeComponent();
            ViewEvent();
            flpGuestList.AutoScroll = true;
            this.Text = "게스트 추가";
        }      

        public string GuestName
        {
            get { return txtGuestName.Text; }
            set { txtGuestName.Text = value; }
        }
        
        public event EventHandler AddGuestEvent;
        public event EventHandler SaveGuestEvent;
        public event EventHandler CloseFormEvevnt;


        /// <summary>
        /// Guest 입력 패널 데이터를 읽어 PlayerInfoDto 리스트로 변환.
        /// </summary>
        /// <returns>Guest 정보 리스트</returns>
        public List<PlayerInfoDto> GusetData()
        {
            var guestList = new List<PlayerInfoDto>();

            // FlowLayoutPanel에 추가된 각 Guest 패널 순회
            foreach (Panel pnl in flpGuestList.Controls)
            {
                // 0번째 컨트롤: Guest 이름 (TextBox 또는 Label)
                string guestName = pnl.Controls[0].Text;

                // 1번째 컨트롤: 성별 (Tag: "1" = 남성, "0" = 여성)
                bool gender = false;
                if (pnl.Controls[1].Tag.ToString() == "1")
                    gender = true;

                // 4번째 컨트롤: 핸디캡 (TextBox)
                int handi = Convert.ToInt32(pnl.Controls[4].Text);

                // 2번째 컨트롤: 프로 여부 (Tag: "1" = 프로)
                bool isPro = false;
                if (pnl.Controls[2].Tag.ToString() == "1")
                    isPro = true;

                // PlayerInfoDto 생성
                PlayerInfoDto guest = new PlayerInfoDto
                {
                    PlayerName = guestName,   // 이름
                    MemberCode = 0,           // Guest는 회원 코드 없음
                    IsSelected = true,        // Guest는 기본적으로 선택 상태
                    Gender = gender,          // 성별
                    Handycap = handi,         // 핸디캡
                    IsPro = isPro             // 프로 여부
                };

                // 리스트에 추가
                guestList.Add(guest);
            }

            return guestList;
        }

        /// <summary>
        /// 폼 종료 메서드
        /// </summary>
        public void CloseForm()
        {
            this.Close();
        }

        /// <summary>
        /// Guest 정보를 입력할 패널을 생성하여 flpGuestList에 추가.
        /// 패널에는 이름, 성별 버튼, 프로 버튼, 핸디캡 입력란, 삭제 버튼 포함.
        /// </summary>
        public void AddGuestPanel()
        {
            //패널 생성
            var pnl = new Panel
            {
                Size = new Size(380, 60),
                Text = ""
            };

            //게스트 이름 텍스트박스 생성 및 초기화
            var txtName = new TextBox
            {
                Text = string.IsNullOrEmpty(txtGuestName.Text) ? "게스트" : txtGuestName.Text,
                Size = new Size(100, 40),
                Font = new Font("맑은 고딕", 12, FontStyle.Bold),
                Location = new Point(10, 20),
                TextAlign = HorizontalAlignment.Center
            };

            //성별 버튼 생성
            var btnGender = new Button
            {
                Text = "여",
                Tag = "0",
                Location = new Point(119, 14),
                Size = new Size(40, 40),
            };

            //프로여부 버튼 생성
            var btnPro = new Button
            {
                Text = "프로",
                Tag = "0",
                Location = new Point(168, 14),
                Size = new Size(40, 40),
            };

            //핸디캡 라벨 생성
            var lblHandi = new Label
            {
                Text = "핸디",
                Size = new Size(40, 35),
                Font = new Font("맑은 고딕", 10),
                Location = new Point(217, 24),
                BackColor = Color.Transparent
            };

            //핸디캡 점수 텍스트 박스 생성
            var txtHandi = new TextBox
            {
                Text = "0",
                Font = new Font("맑은 고딕", 12, FontStyle.Bold),
                Size = new Size(60, 40),
                Location = new Point(258, 20),
                TextAlign = HorizontalAlignment.Center
            };

            //삭제 버튼 생성
            var btnDelete = new Button
            {
                Text = "X",
                Location = new Point(328, 14),
                Size = new Size(40, 40)
            };

            //삭제 버튼 클릭 이벤트 등록 -> 클릭시 삭제
            btnDelete.Click += (sender, e) =>
            {
                flpGuestList.Controls.Remove(pnl);
            };

            //생성 컨트롤 패널에 등록
            pnl.Controls.Add(txtName);
            pnl.Controls.Add(btnGender);
            pnl.Controls.Add(btnPro);
            pnl.Controls.Add(lblHandi);
            pnl.Controls.Add(txtHandi);
            pnl.Controls.Add(btnDelete);

            //게스트 패널 플로우레이아웃 패널에 등록
            flpGuestList.Controls.Add(pnl);

            //성별 버튼 클릭 이벤트 생성 -> 성별 전환 및 여성일 경우 핸디 점수 적용
            btnGender.Click += (sender, e) =>
            {

                btnGender.Tag = btnGender.Tag.ToString() == "0" ? "1" : "0";
                btnGender.BackColor = btnGender.Tag.ToString() == "1" ? Color.AliceBlue : Color.Transparent;
                int i = Convert.ToInt32(txtHandi.Text);
                if (btnGender.Tag.ToString() == "1")
                {
                    txtHandi.Text = (i + 15).ToString();
                }
                else
                {
                    txtHandi.Text = (i - 15).ToString();
                }
            };

            //프로여부 버튼 클릭 이벤트 생성 -> 클릭시 프로여부 및 핸디 점수 점용
            btnPro.Click += (sender, e) =>
            {

                btnPro.Tag = btnPro.Tag.ToString() == "0" ? "1" : "0";
                btnPro.BackColor = btnPro.Tag.ToString() == "1" ? Color.AliceBlue : Color.Transparent;
                int i = Convert.ToInt32(txtHandi.Text);
                if (btnPro.Tag.ToString() == "1")
                {
                    txtHandi.Text = (i - 5).ToString();
                }
                else
                {
                    txtHandi.Text = (i + 5).ToString();
                }
            };
        }

        /// <summary>
        /// 폼 실행 메서드
        /// </summary>
        public void ShowForm()
        {
            //시작 위치는 부모창 가운데
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }


        public void TextBoxClear()
        {
            txtGuestName.Text = "";
        }

        /// <summary>
        /// 컨트롤 이벤트를 바인딩.
        /// </summary>
        private void ViewEvent()
        {
            // 닫기 버튼 클릭 → CloseFormEvevnt 이벤트 발생
            btnClose.Click += (s, e) => CloseFormEvevnt?.Invoke(this, EventArgs.Empty);

            // Guest 추가 버튼 클릭 → AddGuestEvent 이벤트 발생
            btnAddGuest.Click += (s, e) => AddGuestEvent?.Invoke(this, EventArgs.Empty);

            // 저장 버튼 클릭 → SaveGuestEvent 이벤트 발생
            btnSave.Click += (s, e) => SaveGuestEvent?.Invoke(this, EventArgs.Empty);

            // txtGuestName에서 Enter 키 입력 → AddGuestEvent 이벤트 발생
            txtGuestName.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    AddGuestEvent?.Invoke(this, EventArgs.Empty);
            };

            //txtGuestName 클릭 → 포커스 처리 메서드 실행
            txtGuestName.Click += txtGuestName_Enter;
        }

        /// <summary>
        /// Guest 이름 입력란 클릭 시 터치 키보드(TabTip.exe)를 실행.
        /// 이미 실행 중이면 새로 실행하지 않음.
        /// </summary>
        private void txtGuestName_Enter(object sender, EventArgs e)
        {
            // 현재 실행 중인 프로세스 중 TabTip.exe (터치 키보드)가 있는지 확인
            var alreadyRunning = Process.GetProcessesByName("TabTip").Any();

            // 터치 키보드가 실행 중이 아니면 실행
            if (!alreadyRunning)
            {
                Process.Start(@"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe");
            }
        }
    }
}
