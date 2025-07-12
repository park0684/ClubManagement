using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubManagement.Common.Controls;

namespace ClubManagement.Members.Views
{
    public partial class MemberScoreView : Form,IMemberScoreView
    {
        CustomDataGridViewControl dgvScoreList;
        public MemberScoreView()
        {
            InitializeComponent();
            InitiailizeDataGridView();
            ViewEvent();
        }

        public DateTime FromDate => dtpFromDate.Value;

        public DateTime ToDate => dtpToDate.Value;

        public int Status => ((KeyValuePair<int, string>)cmbStatus.SelectedItem).Key;

        public int SortType => cmbSortType.SelectedIndex;

        public string SearchWord => txtSearchWord.Text;

        public string MatchTitel {set => txtMatchTitle.Text = value; }

        public bool IsSearchDate => chkDateSearch.Checked;

        public bool IsMatch => chkMatchSearch.Checked;

        public bool ExculuedMember => chkExclude.Checked;

        public event EventHandler SearchScoreEvent;
        public event EventHandler<int> MemberSelectedEvent;
        public event EventHandler MatachSearchEvent;
        public event EventHandler CheckedEvent;

        public void MemberScoreBinding(DataTable result)
        {
            dgvScoreList.dgv.Rows.Clear();
            dgvScoreList.dgv.DataSource = result;
        }

        /// <summary>
        /// 콤보박스 초기화 메서드
        /// </summary>
        /// <param name="items">추가할 콤보박스 아이템</param>
        public void SetComboboxItem(Dictionary<string,Dictionary<int, string>> items)
        {
            //var statusItems = items.Where( i => i.Key == "status").ToDictionary(i => i.Value., i => i.Value.Item2);
            foreach(var item in items)
            {
                switch (item.Key)
                {
                    case "status":
                        var statusItems = item.Value;
                        statusItems.Add(-1, "전체");
                        cmbStatus.DataSource = statusItems.ToList();
                        cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
                        cmbStatus.DisplayMember = "Value";
                        cmbStatus.ValueMember = "Key";
                        break;
                    case "sort":
                        var sortItems = item.Value;
                        cmbSortType.DataSource = sortItems.ToList();
                        cmbSortType.DropDownStyle = ComboBoxStyle.DropDownList;
                        cmbSortType.DisplayMember = "Value";
                        cmbSortType.ValueMember = "Key";
                        break;
                }
            }
        }

        public void SetDatagirColumns(Dictionary<string, string> columns)
        {
            dgvScoreList.dgv.Columns.Clear();
            foreach(var column in columns)
            {
                dgvScoreList.dgv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = column.Key,
                    DataPropertyName = column.Key,
                    HeaderText = column.Value
                });
            }
            foreach (DataGridViewColumn column in dgvScoreList.dgv.Columns)
            {
                column.HeaderCell.Style.WrapMode = DataGridViewTriState.False; //칼럼 헤더 줄바꿈 방지
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // 셀 크기 자동 조절
            }
            dgvScoreList.dgv.AllowUserToResizeColumns = true;
        }

        public void ShowMessga(string messaga)
        {
            MessageBox.Show(messaga);
        }

        private void ViewEvent()
        {
            btnSearch.Click += (s, e) => SearchScoreEvent?.Invoke(this, EventArgs.Empty);
            chkMatchSearch.CheckedChanged += chkMatchSearch_checked;
            chkDateSearch.CheckedChanged += chkDateSearch_chcked;
        }
        private void InitiailizeDataGridView()
        {
            dgvScoreList = new CustomDataGridViewControl();
            pnlDataGrid.Controls.Add(dgvScoreList.dgv);
            dgvScoreList.dgv.Dock = DockStyle.Fill;
            dgvScoreList.dgv.ReadOnly = true;
            btnPost.Click += MovePostMounth;
            btnPre.Click += MovePreMonth;
        }

        /// <summary>
        /// 모임체크 버튼 클릭시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkMatchSearch_checked(object sender, EventArgs e)
        {
            if (chkMatchSearch.Checked)
                chkDateSearch.Checked = false;
            MatachSearchEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 전체기간 체크 이벤트
        /// 체크시 모임체크에 체크가 되어 있다면 체크를 해제 하고 모임체크 이벤트를 실행 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDateSearch_chcked(object sender, EventArgs e)
        {
            if(chkDateSearch.Checked)
            {
                chkMatchSearch.Checked = false;
            }
            //MatachSearchEvent?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// ▷ 버튼 클릭시 실행 메소드
        /// 날짜를 한달 뒤로 이동 시킴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MovePostMounth(object sender, EventArgs e)
        {
            dtpFromDate.Value = dtpFromDate.Value.AddMonths(1);
            //dtpToDate.Value = dtpToDate.Value.AddMonths(1);
            AdjustToDate();
        }

        /// <summary>
        /// ◁ 버튼 클릭시 실행 메소드
        /// 날짜를 한달전으로 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovePreMonth(object sender, EventArgs e)
        {
            dtpFromDate.Value = dtpFromDate.Value.AddMonths(-1);
            //dtpToDate.Value = dtpToDate.Value.AddMonths(-1);
            AdjustToDate();
        }

        /// <summary>
        /// DateTimePicker 변경 버튼 클릭시 종료일자를 마지막 날짜로 자동 지정
        /// </summary>
        private void AdjustToDate()
        {
            DateTime fromDate = dtpFromDate.Value;
            DateTime today = DateTime.Now;
            int lastDayOfMonth = DateTime.DaysInMonth(fromDate.Year, fromDate.Month);

            // 현재 월이면 오늘 날짜, 아니면 해당 월의 마지막 날
            if (fromDate.Year == today.Year && fromDate.Month == today.Month)
            {
                dtpToDate.Value = today;
            }
            else
            {
                dtpToDate.Value = new DateTime(fromDate.Year, fromDate.Month, lastDayOfMonth);
            }
        }

    }
}
