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
    public partial class EnterScoreView : Form, IEnterScoreView
    {
        public EnterScoreView()
        {
            InitializeComponent();
            //FormBorderStyle = FormBorderStyle.None;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            ControlBox = false;
            InitailizeButtonTag();
            ViewEvent();
        }

        public int? Score
        {
            get { return Convert.ToInt32(lblScore.Text); }
            set { lblScore.Text = value.ToString(); }
        }

        public int? Handi
        {
            get { return Convert.ToInt32(lblHandi.Text); }
            set { lblHandi.Text = value.ToString(); }
        }

        public int? TotalScore
        {
            get { return Convert.ToInt32(lblTotalScore.Text); }
            set { lblTotalScore.Text = value.ToString(); }
        }

        public string PlayerName
        {
            get { return lblPlayerName.Text; }
            set { lblPlayerName.Text = value.ToString(); }
        }

        public bool IsPerface
        {
            get { return btnPerfect.Tag is bool val && val; }
            set 
            { 
                btnPerfect.Tag = (bool)value; 
                if(IsPerface == true)
                    btnPerfect.BackColor = Color.Red; 
            }
        }

        public bool IsAllcover
        {
            get { return btnAllcover.Tag is bool val && val; }
            set 
            { 
                btnAllcover.Tag = (bool)value; 
                if((bool)value)
                    btnAllcover.BackColor = Color.Red; 
            }
        }

        public event EventHandler EnterScroeEvent;
        public event EventHandler IsPerFectEvent;
        public event EventHandler IsAllcoverEvent;
        public event EventHandler CloseFormEvent;

        public void CloseForm()
        {
            this.Close();
        }

        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            ShowDialog();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "알림");
        }
        private void EnterScoreNumber(string number)
        {
            if(lblScore.Text != "0")
            {
                lblScore.Text += number;
            }
            else
            {
                lblScore.Text = number;
            }
            TotalScoreEdit();
        }
        private void ScoreBackspace()
        {
            if(lblScore.Text != "0")
            {
                int textLength = lblScore.Text.Length;
                lblScore.Text = lblScore.Text.Substring(0,textLength - 1);
            }
            else if(lblScore.Text.Length == 1)
            {
                lblScore.Text = "0";
            }
            TotalScoreEdit();
        }
        private void TotalScoreEdit()
        {
            int score = Convert.ToInt32(lblScore.Text);
            int handi = Convert.ToInt32(lblHandi.Text);
            lblTotalScore.Text = score + handi > 300 ? "300" : (score + handi).ToString();
        }
        private void ScoreRemove()
        {
            lblScore.Text = "0";
            lblTotalScore.Text = "0";
        }
        private void ScoreLimit()
        {
            if(lblScore.Text.Length > 3)
            {
                lblScore.Text = lblScore.Text.Substring(0, 3);
            }
            else if (string.IsNullOrEmpty(lblScore.Text))
            {
                lblScore.Text = "0";
            }
            else if(Convert.ToInt32(lblScore.Text) > 300)
            {
                ShowMessage("대단한데... 핀이 20개라도 된거야?");
                lblScore.Text = "0";
            }
        }
        private void EnterPerfact()
        {
            if(!IsPerface)
            {
                lblScore.Text = "300";
                lblTotalScore.Text = "300";
                btnPerfect.BackColor = Color.Red;
                btnPerfect.Tag = (bool)true;
            }
            else
            {
                lblScore.Text = "0";
                lblTotalScore.Text = "0";
                btnPerfect.BackColor = Color.White;
                btnPerfect.Tag = (bool)false;
            }
        }
        private void EnterAllcover()
        {
            if(!(bool)btnAllcover.Tag)
            {
                btnAllcover.Tag = (bool)true;
                btnAllcover.BackColor = Color.Red;
            }
            else
            {
                btnAllcover.Tag = (bool)false;
                btnAllcover.BackColor = Color.White;
            }
        }
        private void InitailizeButtonTag()
        {
            btnPerfect.Tag = (bool)false;
            btnAllcover.Tag = (bool)false;
        }
        private void ViewEvent()
        {
            btnEner.Click += (s, e) => EnterScroeEvent?.Invoke(this, EventArgs.Empty);
            btnClose.Click += (s, e) => CloseFormEvent?.Invoke(this, EventArgs.Empty);

            //내부 이벤트
            btnNumber0.Click += (s, e) => EnterScoreNumber("0");
            btnNumber1.Click += (s, e) => EnterScoreNumber("1");
            btnNumber2.Click += (s, e) => EnterScoreNumber("2");
            btnNumber3.Click += (s, e) => EnterScoreNumber("3");
            btnNumber4.Click += (s, e) => EnterScoreNumber("4");
            btnNumber5.Click += (s, e) => EnterScoreNumber("5");
            btnNumber6.Click += (s, e) => EnterScoreNumber("6");
            btnNumber7.Click += (s, e) => EnterScoreNumber("7");
            btnNumber8.Click += (s, e) => EnterScoreNumber("8");
            btnNumber9.Click += (s, e) => EnterScoreNumber("9");
            btnBackSpace.Click += (s, e) => ScoreBackspace();
            btnDelete.Click += (s, e) => ScoreRemove();
            btnPerfect.Click += (s, e) => EnterPerfact();
            btnAllcover.Click += (s, e) => EnterAllcover();
            lblScore.TextChanged += (s, e) => ScoreLimit();

        }

    }
}
