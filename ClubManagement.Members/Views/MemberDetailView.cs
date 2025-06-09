using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubManagement.Common.Hlepers;

namespace ClubManagement.Members.Views
{
    public partial class MemberDetailView : Form, IMemberDetailView
    {
        public MemberDetailView()
        {
            InitializeComponent();
            InitializeComboBox();
            SecessDatePickerSet(cmbStatus, EventArgs.Empty);
            ViewEvent();
        }

        /// <summary>
        /// 뷰 이벤트 등록
        /// </summary>
        private void ViewEvent()
        {
            btnClose.Click += (s, e) => CloseFormEvent?.Invoke(s, e);
            btnOk.Click += (s, e) => SaveEvent?.Invoke(s, e);
            cmbStatus.SelectedIndexChanged += SecessDatePickerSet;
        }

        /// <summary>
        /// 탈퇴일자 입력 활성화 여부 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SecessDatePickerSet(object sender, EventArgs e)
        {
            //회원 상태가 탈퇴 상태가 아니라면 탈퇴일 지정 DatePiker는 비활성화
            if (((KeyValuePair<int, string>)cmbStatus.SelectedItem).Key != 2)
            {
                dtpSecessDate.Enabled = false;
            }
            else
            {
                dtpSecessDate.Enabled = true;
            }
        }

        /// <summary>
        /// 콤보박스 초기화
        /// </summary>
        private void InitializeComboBox()
        {
            //회원 상태 
            foreach (var item in MemberHelper.MemStatus)
            {
                cmbStatus.Items.Add(new KeyValuePair<int, string>(item.Key, item.Value));
            }
            cmbStatus.DisplayMember = "Value";
            cmbStatus.ValueMember = "key";
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.SelectedIndex = 0;

            //성별 
            foreach (var item in MemberHelper.MemGender)
            {
                cmbGender.Items.Add(new KeyValuePair<int, string>(item.Key, item.Value));
            }
            cmbGender.DisplayMember = "Value";
            cmbGender.ValueMember = "key";
            cmbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGender.SelectedIndex = 1;

            //직책
            foreach (var item in MemberHelper.MemPosition)
            {
                cmbPosition.Items.Add(new KeyValuePair<int, string>(item.Key, item.Value));
            }
            cmbPosition.DisplayMember = "Value";
            cmbPosition.ValueMember = "key";
            cmbPosition.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPosition.SelectedIndex = 3;
        }
        public string MemberName
        {
            get { return txtName.Text; }
            set { txtName.Text = value; }
        }

        public string Birth
        {
            get { return txtBrithYear.Text; }
            set { txtBrithYear.Text = value; }
        }

        public string Memo
        {
            get { return txtMemo.Text; }
            set { txtMemo.Text = value; }
        }

        public int? Status
        {
            get { return ((KeyValuePair<int, string>)cmbStatus.SelectedItem).Key; }
            set
            {
                if (value.HasValue)
                {
                    int index = cmbStatus.Items.Cast<KeyValuePair<int, string>>().ToList().FindIndex(item => item.Key == value.Value);
                    if (index >= 0)
                    {
                        cmbStatus.SelectedIndex = index;
                    }
                }
            }
        }

        public int? Position
        {
            get { return ((KeyValuePair<int, string>)cmbPosition.SelectedItem).Key; }
            set
            {
                if (value.HasValue)
                {
                    int index = cmbPosition.Items.Cast<KeyValuePair<int, string>>().ToList().FindIndex(item => item.Key == value.Value);
                    if (index >= 0)
                    {
                        cmbPosition.SelectedIndex = index;
                    }
                }
            }
        }

        public int? Gender
        {
            get { return ((KeyValuePair<int, string>)cmbGender.SelectedItem).Key; }
            set
            {
                if (value.HasValue)
                {
                    int index = cmbGender.Items.Cast<KeyValuePair<int, string>>().ToList().FindIndex(item => item.Key == value.Value);
                    if (index >= 0)
                    {
                        cmbGender.SelectedIndex = index;
                    }
                }
            }
        }

        public DateTime? AccessDate
        {
            get { return dtpAccessDate.Value; }
            set { dtpAccessDate.Value = (DateTime)value; }
        }

        public DateTime? SecessDate
        {
            get { return dtpSecessDate.Value; }
            set { dtpSecessDate.Value = (DateTime)value; }
        }

        public bool IsPro
        {
            get { return chkIsPro.Checked; }
            set { chkIsPro.Checked = value; }
        }

        public event EventHandler SaveEvent;
        public event EventHandler CloseFormEvent;

        public void CloseForm()
        {
            this.Close();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "알림");
        }

        public void ShowForm()
        {
            Form form = (Form)this;
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }
    }
}
