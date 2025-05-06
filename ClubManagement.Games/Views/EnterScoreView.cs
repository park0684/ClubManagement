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
            InitializeButtonTag();
            ViewEvent();
        }
        
        /// <summary>
        /// 입력 점수
        /// </summary>
        public int? Score
        {
            get { return Convert.ToInt32(lblScore.Text); }
            set { lblScore.Text = value.ToString(); }
        }

        /// <summary>
        /// 핸디캡 점수
        /// </summary>
        public int? Handi
        {
            get { return Convert.ToInt32(lblHandi.Text); }
            set { lblHandi.Text = value.ToString(); }
        }

        /// <summary>
        /// 입력 점수 + 핸디캡 점수
        /// 300점을 넘길 수 없음
        /// </summary>
        public int? TotalScore
        {
            get { return Convert.ToInt32(lblTotalScore.Text); }
            set { lblTotalScore.Text = value.ToString(); }
        }

        /// <summary>
        /// 플레이어 이름
        /// </summary>
        public string PlayerName
        {
            get { return lblPlayerName.Text; }
            set { lblPlayerName.Text = value.ToString(); }
        }

        /// <summary>
        /// 퍼펙트 여부
        /// </summary>
        public bool IsPerfact
        {
            get { return btnPerfect.Tag is bool val && val; }
            set 
            { 
                btnPerfect.Tag = (bool)value; 
                if(IsPerfact == true)
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

        public event EventHandler EnterScoreEvent;
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

        /// <summary>
        /// 키패드 입력 값을 통한 점수 표시
        /// </summary>
        /// <param name="number"></param>
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

        /// <summary>
        /// 입력 점수 제한
        /// 입력 가능한 최대 점수는 300점으로 3자리 이상 입력, 300점 이상 입력을 제한
        /// </summary>
        private void ScoreLimit()
        {
            if(lblScore.Text.Length > 3) // 라벨로 입력된 글자수가 3자리 입력제한
            {
                lblScore.Text = lblScore.Text.Substring(0, 3);
            }
            else if (string.IsNullOrEmpty(lblScore.Text)) //공백 또는 NULL 일 경우 0으로 표시
            {
                lblScore.Text = "0";
            }
            else if(Convert.ToInt32(lblScore.Text) > 300) // 300점 이상 입력 시 메시지 박스 발생
            {
                ShowMessage("300점 이상은 입력 할 수 없습니다");
                lblScore.Text = "0";
            }
        }

        /// <summary>
        /// 퍼펙트로 입력시 점수는 300점으로 표시
        /// 핸디 여부와 상관없이 합계 점수도 300점으로 표시되며
        /// btnPerfect 버튼 색상 변경 
        /// </summary>
        private void EnterPerfact()
        {
            if(!IsPerfact)
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

        /// <summary>
        /// 올커버로 입력시 btnAllcover 색상 변경
        /// </summary>
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

        /// <summary>
        /// 버튼 태그값 초기화
        /// </summary>
        private void InitializeButtonTag()
        {
            btnPerfect.Tag = (bool)false;
            btnAllcover.Tag = (bool)false;
        }
        private void ViewEvent()
        {
            btnEner.Click += (s, e) => EnterScoreEvent?.Invoke(this, EventArgs.Empty);
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
