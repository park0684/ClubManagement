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

        private void ViewEvent()
        {
            btnClose.Click += (s, e) => CloseFormEvevnt?.Invoke(this, EventArgs.Empty);
            btnAddGuest.Click += (s, e) => AddGuestEvent?.Invoke(this, EventArgs.Empty);
            btnSave.Click += (s, e) => SaveGuestEvent?.Invoke(this, EventArgs.Empty);
            txtGuestName.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    AddGuestEvent?.Invoke(this, EventArgs.Empty);
            };
            txtGuestName.Click += txtGuestName_Enter;
        }

        public string GuestName
        {
            get { return txtGuestName.Text; }
            set { txtGuestName.Text = value; }
        }
        public List<PlayerInfoDto> GusetDate()
        {
            var guestList = new List<PlayerInfoDto>();

            foreach (Panel pnl in flpGuestList.Controls)
            {
                string guestName = pnl.Controls[0].Text;
                bool gender = false;
                if (pnl.Controls[1].Tag.ToString() == "1")
                    gender = true;
                int handi = Convert.ToInt32(pnl.Controls[4].Text);
                bool isPro = false;
                if (pnl.Controls[2].Tag.ToString() == "1")
                    isPro = true;
                PlayerInfoDto guest = new PlayerInfoDto
                {
                    PlayerName = guestName,
                    MemberCode = 0,
                    IsSelected = true,
                    Gender = gender,
                    Handycap = handi,
                    IsPro = isPro
                };
                guestList.Add(guest);
            }
            return guestList;
        }
        public event EventHandler AddGuestEvent;
        public event EventHandler SaveGuestEvent;
        public event EventHandler CloseFormEvevnt;

        public void CloseForm()
        {
            this.Close();
        }
        public void AddGuestPanel()
        {
            var pnl = new Panel
            {
                Size = new Size(380, 60),
                Text = ""
            };
            var txtName = new TextBox
            {
                Text = string.IsNullOrEmpty(txtGuestName.Text) ? "게스트" : txtGuestName.Text,
                Size = new Size(100, 40),
                Font = new Font("맑은 고딕", 12, FontStyle.Bold),
                Location = new Point(10, 20),
                TextAlign = HorizontalAlignment.Center
            };
            var btnGender = new Button
            {
                Text = "여",
                Tag = "0",
                Location = new Point(119, 14),
                Size = new Size(40, 40),
            };

            var btnPro = new Button
            {
                Text = "프로",
                Tag = "0",
                Location = new Point(168, 14),
                Size = new Size(40, 40),
            };
            var lblHandi = new Label
            {
                Text = "핸디",
                Size = new Size(40, 35),
                Font = new Font("맑은 고딕", 10),
                Location = new Point(217, 24),
                BackColor = Color.Transparent
            };
            var txtHandi = new TextBox
            {
                Text = "0",
                Font = new Font("맑은 고딕", 12, FontStyle.Bold),
                Size = new Size(60, 40),
                Location = new Point(258, 20),
                TextAlign = HorizontalAlignment.Center
            };
            var btnDelete = new Button
            {
                Text = "X",
                Location = new Point(328, 14),
                Size = new Size(40, 40)
            };
            btnDelete.Click += (sender, e) =>
            {
                flpGuestList.Controls.Remove(pnl);
            };
            pnl.Controls.Add(txtName);
            pnl.Controls.Add(btnGender);
            pnl.Controls.Add(btnPro);
            pnl.Controls.Add(lblHandi);
            pnl.Controls.Add(txtHandi);
            pnl.Controls.Add(btnDelete);
            flpGuestList.Controls.Add(pnl);
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
        public void ShowForm()
        {
                
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }
        public void TextBoxClear()
        {
            txtGuestName.Text = "";
        }

        private void txtGuestName_Enter(object sender, EventArgs e)
        {
            var alreadyRunning = Process.GetProcessesByName("TabTip").Any();
            if (!alreadyRunning)
            {
                Process.Start(@"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe");
            }
        }
    }
}
