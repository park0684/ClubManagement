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
    public partial class NumericPadView : Form,INumericPadView
    {
        public NumericPadView()
        {
            InitializeComponent();
            viewEvent();
            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        public int Nember
        {
            get { return Convert.ToInt32(lblNumber.Text); }
            set { lblNumber.Text = value.ToString("#,##0"); }
        }

        public event EventHandler CloseFormEvent;
        public event EventHandler InsertNumberEvent;

        /// <summary>
        /// 폼 종료
        /// </summary>
        public void CloseForm()
        {
            this.Close();
        }
        /// <summary>
        /// 부모창 중앙에서 모달 다이얼로그로 실행.
        /// </summary>
        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }

        /// <summary>
        /// 버튼 이벤트를 바인딩.
        /// 숫자, 백스페이스, 클리어, OK, 닫기 기능 포함.
        /// </summary>
        private void viewEvent()
        {
            // 숫자 버튼 배열 정의
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

            // 숫자 버튼 클릭 이벤트 바인딩
            for (int i = 0; i < buttons.Length; i++)
            {
                int n = i;  // 캡처용 지역 변수 (람다 내부에서 i 값 고정)
                buttons[i].Click += (s, e) => EnterNumber(n.ToString());
            }

            // 기능 버튼 바인딩
            btnBackSpace.Click += (s, e) => NumberBacksapce();
            btnClear.Click += (s, e) => NumberClear();

            // 닫기 버튼 → 닫기 이벤트 발생
            btnClose.Click += (s, e) => CloseFormEvent?.Invoke(this, EventArgs.Empty);

            // OK 버튼 → 숫자 전달 후 폼 닫기
            btnOk.Click += (s, e) =>
            {
                InsertNumberEvent?.Invoke(this, EventArgs.Empty);
                CloseForm();
            };
        }


        /// <summary>
        /// 숫자 입력 초기화 (0으로 설정).
        /// </summary>
        private void NumberClear()
        {
            lblNumber.Text = "0";
        }

        /// <summary>
        /// 숫자 입력 마지막 자리 삭제.
        /// </summary>
        private void NumberBacksapce()
        {
            if (lblNumber.Text == "0")
            {
                // 이미 0이면 아무 처리하지 않음
                return;
            }
            else
            {
                int textLength = lblNumber.Text.Length;

                if (textLength > 1)
                {
                    // 마지막 자리 제거
                    lblNumber.Text = lblNumber.Text.Substring(0, textLength - 1);
                }
                else
                {
                    // 한 자리 남았으면 0으로 초기화
                    lblNumber.Text = "0";
                }
            }
        }

        /// <summary>
        /// 숫자 버튼 입력 처리.
        /// </summary>
        /// <param name="number">입력한 숫자 문자열</param>
        private void EnterNumber(string number)
        {
            if (lblNumber.Text != "0")
            {
                // 기존 숫자 뒤에 추가
                lblNumber.Text += number;
            }
            else
            {
                // 첫 입력은 0을 대체
                lblNumber.Text = number;
            }
        }
    }
}
