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
    public partial class StatementDetailView : Form,IStatementDetailView
    {
        public StatementDetailView()
        {
            InitializeComponent();
            InitializeComboBox();
            ViewEvent();
            txtMemberName.Enabled = false;
            btnDelete.Visible = false;
        }

        /// <summary>
        /// 이벤트 등록
        /// </summary>
        private void ViewEvent()
        {
            btnSave.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
            btnClose.Click += (s, e) => CloseEvent?.Invoke(this, EventArgs.Empty);
            btnSelectMember.Click += (s, e) => SelectMemberEvent?.Invoke(this, EventArgs.Empty);
            btnDelete.Click += (s, e) => DeleteEvent?.Invoke(this, EventArgs.Empty);
            cmbStatementType.SelectedIndexChanged += (s, e) => TypeChaingedSet();
            txtDueAmount.TextChanged += (s, e) => DuesAmountChaingedEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 전표 유형 설정에 따른 텍스트 박스 설정
        /// </summary>
        public void TypeChaingedSet()
        {
            switch (((KeyValuePair<int, string>)cmbStatementType.SelectedItem).Value)
            {
                case "회비":
                    grpDue.Enabled = true;
                    grpWihtdrawal.Enabled = false;
                    txtWithdrawalAmount.Text = "";
                    txtWithdrawalDetail.Text = "";
                    txtDueAmount.Text = "10000";
                    cmbCount.SelectedIndex = 0;
                    break;
                case "지출":
                    grpWihtdrawal.Enabled = true;
                    grpDue.Enabled = false;
                    txtDueAmount.Text = "";
                    txtMemberName.Text = "";
                    cmbCount.SelectedIndex = 0;
                    break;
                case "면제":
                    grpDue.Enabled = true;
                    grpWihtdrawal.Enabled = false;
                    txtWithdrawalAmount.Text = "";
                    txtWithdrawalDetail.Text = "";
                    txtDueAmount.Text = "0";
                    cmbCount.SelectedIndex = 0;
                    break;
                case "기타":
                    grpWihtdrawal.Enabled = true;
                    grpDue.Enabled = false;
                    txtDueAmount.Text = "";
                    txtMemberName.Text = "";
                    cmbCount.SelectedIndex = 0;
                    break;
                case "기타입금":
                    grpDue.Enabled = true;
                    grpWihtdrawal.Enabled = false;
                    txtWithdrawalAmount.Text = "";
                    txtWithdrawalDetail.Text = "";
                    txtDueAmount.Text = "0";
                    cmbCount.SelectedIndex = 0;
                    break;

            }
        }

        /// <summary>
        /// 전표 유형 콤보박스 초기화
        /// </summary>
        private void InitializeComboBox()
        {
            foreach (var item in MemberHelper.DuesType)
            {
                cmbStatementType.Items.Add(new KeyValuePair<int, string>(item.Key, item.Value));
            }
            cmbStatementType.DisplayMember = "Value";
            cmbStatementType.ValueMember = "Key";
            cmbStatementType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatementType.SelectedIndex = 0;

            for (int i = 1; i < 13; i++)
            {
                cmbCount.Items.Add(i);
            }
            cmbCount.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCount.SelectedIndex = 0;
        }
        public DateTime StatementDate
        {
            get { return dtpStatementDate.Value; }
            set { dtpStatementDate.Value = value; ; }
        }

        public int? StatementType
        {
            get { return (int)((KeyValuePair<int, string>)cmbStatementType.SelectedItem).Key; }
            set
            {
                int index = cmbStatementType.Items.Cast<KeyValuePair<int, string>>().ToList().FindIndex(item => item.Key == value);
                if (index >= 0)
                {
                    cmbStatementType.SelectedIndex = index;
                }

            }
        }

        public int DueCount
        {
            get { return (int)cmbCount.SelectedItem; }
            set { cmbCount.SelectedItem = value; }
        }

        public int? DueAmount
        {
            get 
            {
                if (int.TryParse(txtDueAmount.Text, out var result))
                    return result;
                return 0;
            }
            set { txtDueAmount.Text = (value ?? 0).ToString(); }
        }

        public int? Withdrawal
        {
            get { return Convert.ToInt32(txtWithdrawalAmount.Text); }
            set { txtWithdrawalAmount.Text = value.ToString(); }
        }

        public string MemberName
        {
            get { return txtMemberName.Text; }
            set { txtMemberName.Text = value; }
        }

        public string WithdrawalDetail
        {
            get { return txtWithdrawalDetail.Text; }
            set { txtWithdrawalDetail.Text = value; }
        }
        public bool IsWithdrawal
        {
            get
            {
                int selectedKey = ((KeyValuePair<int, string>)cmbStatementType.SelectedItem).Key;
                if (selectedKey == 1 || selectedKey == 3 || selectedKey == 4)
                {
                    return false;
                }
                return true;
            }

        }

        public string Memo
        {
            get { return txtMemo.Text; }
            set { txtMemo.Text = value; }
        }

        public int? Apply
        {
            get { return Convert.ToInt32(cmbCount.SelectedItem); }
            set { cmbCount.SelectedItem = value.ToString(); }
        }

        public event EventHandler SaveEvent;
        public event EventHandler CloseEvent;
        public event EventHandler SelectMemberEvent;
        public event EventHandler DeleteEvent;
        public event EventHandler TypeChaingedEvnet;
        public event EventHandler DuesAmountChaingedEvent;

        public void CloseForm()
        {
            this.Close();
        }

        public void SetDeleteButtonVisivle()
        {
            btnDelete.Visible = true;
        }

        public void ShowForm()
        {
            Form form = (Form)this;
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        public void SetApplyCounter(int counter)
        {
            cmbCount.SelectedItem = counter;
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
