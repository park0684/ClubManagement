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
            this.Text = "점수 입력";
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

        /// <summary>
        /// 올커버 여부
        /// </summary>
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
        /// <summary>
        /// 점수 입력란에서 마지막 숫자를 지우는 기능.
        /// 점수가 "0"이 되면 초기화.
        /// </summary>
        private void ScoreBackspace()
        {
            if (lblScore.Text != "0")
            {
                // 마지막 문자 제거
                int textLength = lblScore.Text.Length;
                lblScore.Text = lblScore.Text.Substring(0, textLength - 1);
            }
            else if (lblScore.Text.Length == 1)
            {
                // 한 글자 남았으면 "0"으로 초기화
                lblScore.Text = "0";
            }

            // 총점 갱신
            TotalScoreEdit();
        }

        /// <summary>
        /// 점수 + 핸디캡 합산 총점을 계산하여 표시.
        /// 300 초과 시 300으로 고정.
        /// </summary>
        private void TotalScoreEdit()
        {
            int score = Convert.ToInt32(lblScore.Text);
            int handi = Convert.ToInt32(lblHandi.Text);

            int total = score + handi;
            lblTotalScore.Text = total > 300 ? "300" : total.ToString();
        }

        /// <summary>
        /// 점수와 총점을 0으로 초기화.
        /// </summary>
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
        /// 퍼펙트(300점) 입력 처리.
        /// 퍼펙트로 설정 시: 점수를 300으로 고정하고 버튼 색상 변경.
        /// 퍼펙트 해제 시: 점수와 총점을 0으로 초기화하고 버튼 색상 초기화.
        /// </summary>
        private void EnterPerfact()
        {
            if (!IsPerfact) // 퍼펙트 상태가 아닐 때 → 퍼펙트로 변경
            {
                // 점수와 총점을 300으로 고정
                lblScore.Text = "300";
                lblTotalScore.Text = "300";

                // 버튼 색상 빨강으로 변경
                btnPerfect.BackColor = Color.Red;

                // 버튼 Tag에 상태 true 설정
                btnPerfect.Tag = (bool)true;
            }
            else // 퍼펙트 상태일 때 → 퍼펙트 해제
            {
                // 점수와 총점을 0으로 초기화
                lblScore.Text = "0";
                lblTotalScore.Text = "0";

                // 버튼 색상 흰색으로 변경
                btnPerfect.BackColor = Color.White;

                // 버튼 Tag에 상태 false 설정
                btnPerfect.Tag = (bool)false;
            }
        }

        /// <summary>
        /// 올커버 입력 시 버튼 상태를 토글.
        /// 활성화 시 버튼 색상을 빨강으로.
        /// 비활성화 시 버튼 색상을 흰색으로.
        /// </summary>
        private void EnterAllcover()
        {
            // 현재 버튼 Tag 값이 false (또는 null)일 때 → 올커버 활성화
            if (!(bool)btnAllcover.Tag)
            {
                btnAllcover.Tag = true;                // 상태: 활성화
                btnAllcover.BackColor = Color.Red;     // 버튼 색상: 빨강
            }
            else
            {
                btnAllcover.Tag = false;               // 상태: 비활성화
                btnAllcover.BackColor = Color.White;   // 버튼 색상: 흰색
            }
        }


        /// <summary>
        /// 점수 입력 관련 버튼들의 태그 값을 초기화.
        /// 퍼펙트, 올커버 상태를 false 로 리셋.
        /// </summary>
        private void InitializeButtonTag()
        {
            // 퍼펙트 버튼 상태 초기화
            btnPerfect.Tag = false;
            btnPerfect.BackColor = Color.White;

            // 올커버 버튼 상태 초기화
            btnAllcover.Tag = false;
            btnAllcover.BackColor = Color.White;
        }

        /// <summary>
        /// 점수 입력 화면의 버튼 및 컨트롤 이벤트 바인딩.
        /// </summary>
        private void ViewEvent()
        {
            // 점수 입력 완료 버튼 → EnterScoreEvent 발생
            btnEner.Click += (s, e) => EnterScoreEvent?.Invoke(this, EventArgs.Empty);

            // 닫기 버튼 → CloseFormEvent 발생
            btnClose.Click += (s, e) => CloseFormEvent?.Invoke(this, EventArgs.Empty);

            // 숫자 버튼 클릭 → 점수 숫자 입력 처리
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

            // 점수 지우기: 마지막 자리 삭제
            btnBackSpace.Click += (s, e) => ScoreBackspace();

            // 점수 전체 삭제
            btnDelete.Click += (s, e) => ScoreRemove();

            // 퍼펙트 버튼 클릭 → 퍼펙트 상태 토글
            btnPerfect.Click += (s, e) => EnterPerfact();

            // 올커버 버튼 클릭 → 올커버 상태 토글
            btnAllcover.Click += (s, e) => EnterAllcover();

            // 점수 텍스트 변경 시 → 점수 제한 검사
            lblScore.TextChanged += (s, e) => ScoreLimit();
        }

    }
}
