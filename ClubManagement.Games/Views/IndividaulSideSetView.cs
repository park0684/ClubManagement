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
    public partial class IndividaulSideSetView : Form, IIndividualSideSetView
    {
        private TextBox _currentTextBox;
        public IndividaulSideSetView()
        {
            InitializeComponent();
            InitializeTextBox();
            ViewEvent();
            txtPrize1st.Focus();
            this.Text = "개인 사이드 게임 설정";
        }

        public int? Prize1st
        {
            get { return Convert.ToInt32(txtPrize1st.Text); }
            set { txtPrize1st.Text = value.ToString(); }
        }

        public int? Prize2nd
        {
            get { return Convert.ToInt32(txtPrize2nd.Text); }
            set { txtPrize2nd.Text = value.ToString(); }
        }

        public int? Prize3rd
        {
            get { return Convert.ToInt32(txtPrize3rd.Text); }
            set { txtPrize3rd.Text = value.ToString(); }
        }

        public int? Handi1st
        {
            get { return Convert.ToInt32(txtHandi1st.Text); }
            set { txtHandi1st.Text = value.ToString(); }
        }

        public int? Handi2nd
        {
            get { return Convert.ToInt32(txtHandi2nd.Text); }
            set { txtHandi2nd.Text = value.ToString(); }
        }

        public int? Handi3rd
        {
            get { return Convert.ToInt32(txtHandi3rd.Text); }
            set { txtHandi3rd.Text = value.ToString(); }
        }


        public event EventHandler SaveEvent;
        public event EventHandler CloseEvent;
        /// <summary>
        /// 상금 총합을 갱신.
        /// 1등, 2등, 3등 상금의 합을 계산하여 txtTotalPrize 에 표시.
        /// </summary>
        private void UpdateTotalPrize(object sender, EventArgs e)
        {
            int total = Prize1st.GetValueOrDefault() + Prize2nd.GetValueOrDefault() + Prize3rd.GetValueOrDefault();
            txtTotalPrize.Text = total.ToString();
        }

        /// <summary>
        /// 핸디캡, 상금 관련 텍스트 박스를 초기화 (모두 0).
        /// </summary>
        private void InitializeTextBox()
        {
            txtHandi1st.Text = "0";
            txtHandi2nd.Text = "0";
            txtHandi3rd.Text = "0";
            txtPrize1st.Text = "0";
            txtPrize2nd.Text = "0";
            txtPrize3rd.Text = "0";
            txtTotalPrize.Text = "0";
        }

        /// <summary>
        /// 버튼 및 텍스트 박스 이벤트 바인딩.
        /// </summary>
        private void ViewEvent()
        {
            // 저장, 닫기 버튼
            btnSave.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
            btnClose.Click += (s, e) => CloseEvent?.Invoke(this, EventArgs.Empty);

            // 상금 값 변경 시 총합 업데이트
            txtPrize1st.TextChanged += UpdateTotalPrize;
            txtPrize2nd.TextChanged += UpdateTotalPrize;
            txtPrize3rd.TextChanged += UpdateTotalPrize;

            // 텍스트 박스 배열
            var textBoxs = new[]
            {
                txtPrize1st,
                txtPrize2nd,
                txtPrize3rd,
                txtHandi1st,
                txtHandi2nd,
                txtHandi3rd
            };

            //텍스트 박스 이벤트 이벤트 등록
            foreach(var textBox in textBoxs)
            {
                // 텍스트박스 포커스 시 현재 입력 대상 지정
                textBox.Enter += (s, e) => _currentTextBox = (TextBox)s;

                // 텍스트박스 클릭 시 커서를 맨 끝으로 이동
                textBox.Click += MoveCursorToEnd;
            }

            // 숫자 버튼 및 기능 버튼 (←, 삭제) 클릭 이벤트
            //숫자 버튼 배열
            var buttons = new[]
            {
                btnNumber0,
                btnNumber1,
                btnNumber2,
                btnNumber3,
                btnNumber4,
                btnNumber5,
                btnNumber6,
                btnNumber7,
                btnNumber8,
                btnNumber9
            };

            //숫자 버튼 이벤트 등록
            foreach(var btn in buttons)
            {
                btn.Click += btnNumber_Click;
            }
            // 백스페이스 이벤트 등록
            btnBackSpace.Click += btnNumber_Click;
            // 초기화 이벤트 등록
            btnDelete.Click += btnNumber_Click;
        }

        /// <summary>
        /// 숫자 및 기능 버튼 클릭 처리.
        /// </summary>
        private void btnNumber_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && _currentTextBox != null)
            {
                string input = btn.Text;

                if (char.IsDigit(input, 0))
                {
                    // 숫자 입력 처리
                    if (_currentTextBox.Text == "0")
                        _currentTextBox.Text = input;
                    else
                        _currentTextBox.Text += input;
                }
                else if (input == "←")
                {
                    // 백스페이스 처리
                    if (_currentTextBox.Text == "0")
                        return;

                    if (_currentTextBox.Text.Length > 1)
                        _currentTextBox.Text = _currentTextBox.Text.Substring(0, _currentTextBox.Text.Length - 1);
                    else
                        _currentTextBox.Text = "0";
                }
                else if (input == "삭제")
                {
                    // 삭제 처리
                    _currentTextBox.Text = "0";
                }
            }
        }

        /// <summary>
        /// 텍스트 박스 클릭 시 커서를 가장 끝으로 이동.
        /// </summary>
        private void MoveCursorToEnd(object sender, EventArgs e)
        {
            if (sender is TextBox txt)
            {
                txt.SelectionStart = txt.Text.Length;
                txt.SelectionLength = 0;
            }
        }

        /// <summary>
        /// 폼을 화면 중앙에서 모달 실행 이베느
        /// </summary>
        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }

        /// <summary>
        /// 폼 종료 이벤트
        /// </summary>
        public void CloseForm()
        {
            this.Close();
        }

        /// <summary>
        /// 메시지 박스 이벤트
        /// </summary>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "알림");
        }
    }
}
