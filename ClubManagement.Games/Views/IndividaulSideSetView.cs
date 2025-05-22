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

        private void UpdateTotalPrize(object sender, EventArgs e)
        {
            int total = Prize1st.GetValueOrDefault() + Prize2nd.GetValueOrDefault() + Prize3rd.GetValueOrDefault();
            txtTotalPrize.Text = total.ToString();
        }

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
        private void ViewEvent()
        {
            btnSave.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
            btnClose.Click += (s, e) => CloseEvent?.Invoke(this, EventArgs.Empty);

            txtPrize1st.TextChanged += UpdateTotalPrize;
            txtPrize2nd.TextChanged += UpdateTotalPrize;
            txtPrize3rd.TextChanged += UpdateTotalPrize;

            txtPrize1st.Enter += (s, e) => _currentTextBox = (TextBox)s;
            txtPrize2nd.Enter += (s, e) => _currentTextBox = (TextBox)s;
            txtPrize3rd.Enter += (s, e) => _currentTextBox = (TextBox)s;
            txtHandi1st.Enter += (s, e) => _currentTextBox = (TextBox)s;
            txtHandi2nd.Enter += (s, e) => _currentTextBox = (TextBox)s;
            txtHandi3rd.Enter += (s, e) => _currentTextBox = (TextBox)s;

            txtPrize1st.Click += MoveCursorToEnd;
            txtPrize2nd.Click += MoveCursorToEnd;
            txtPrize3rd.Click += MoveCursorToEnd;
            txtHandi1st.Click += MoveCursorToEnd;
            txtHandi2nd.Click += MoveCursorToEnd;
            txtHandi3rd.Click += MoveCursorToEnd;

            btnNumber0.Click += btnNumber_Click;
            btnNumber1.Click += btnNumber_Click;
            btnNumber2.Click += btnNumber_Click;
            btnNumber3.Click += btnNumber_Click;
            btnNumber4.Click += btnNumber_Click;
            btnNumber5.Click += btnNumber_Click;
            btnNumber6.Click += btnNumber_Click;
            btnNumber7.Click += btnNumber_Click;
            btnNumber8.Click += btnNumber_Click;
            btnNumber9.Click += btnNumber_Click;
            btnBackSpace.Click += btnNumber_Click;
            btnDelete.Click += btnNumber_Click;
        }
        private void btnNumber_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && _currentTextBox != null)
            {
                string input = btn.Text;

                if (char.IsDigit(input, 0))
                {
                    if (_currentTextBox.Text == "0")
                        _currentTextBox.Text = input;
                    else
                        _currentTextBox.Text += input;
                }
                else if (input == "←")
                {
                    if (_currentTextBox.Text == "0")
                        return;
                    if (_currentTextBox.Text.Length > 1)
                        _currentTextBox.Text = _currentTextBox.Text.Substring(0, _currentTextBox.Text.Length - 1);
                    else
                        _currentTextBox.Text = "0";
                }
                else if (input == "삭제")
                {
                    _currentTextBox.Text = "0";
                }
            }

        }
        /// <summary>
        /// 텍스트 박스 클릭 시 포커스 가장 마지막으로 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveCursorToEnd(object sender, EventArgs e)
        {
            if (sender is TextBox txt)
            {
                txt.SelectionStart = txt.Text.Length;
                txt.SelectionLength = 0;
            }
        }
        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }

        public void CloseForm()
        {
            this.Close();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "알림");
        }
    }
}
