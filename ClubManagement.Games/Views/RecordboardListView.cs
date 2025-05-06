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
using ClubManagement.Common.Hlepers;

namespace ClubManagement.Games.Views
{
    public partial class RecordboardListView : Form,IRecordboardListView
    {
        CustomDataGridViewControl dgvMatchList;
        public RecordboardListView()
        {
            InitializeComponent();
            InitializeDateTimePicker();
            InitializeDataGridView();
            ViewEvent();
        }
        private void ViewEvent()
        {
            btnAddGame.Click += (s, e) => RecordBoardRegistEvent?.Invoke(this, EventArgs.Empty);
            btnGameListEdit.Click += (s, e) => RecordBoardEditEvent?.Invoke(this, EventArgs.Empty);
            dgvMatchList.dgv.CellDoubleClick += (s, e) => RecordBoarSelectedEvent?.Invoke(this, EventArgs.Empty);

            // 내부 컨트롤러
            btnPre.Click += MovePreMonth;
            btnPost.Click += MovePostMounth;
        }
        private void InitializeDataGridView()
        {
            dgvMatchList = new CustomDataGridViewControl();
            var dgv = dgvMatchList.dgv;
            pnlDataGrid.Controls.Add(dgv);
            dgv.Dock = DockStyle.Fill;
            dgv.Columns.Add("code", "모임코드");
            dgv.Columns.Add("date", "날짜");
            dgv.Columns.Add("type", "유형");
            dgv.Columns.Add("title", "모임명");
            dgv.Columns.Add("host", "주최자");
            dgv.Columns.Add("totalPlayer", "참석인원");
            dgv.Columns.Add("member", "회원");
            dgv.Columns.Add("nonMember", "비회원");
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMatchList.ApplyDefaultColumnSettings();
            dgvMatchList.FormatAsDate("date");
            dgvMatchList.FormatAsStringLeft("title");
            dgvMatchList.FormatAsInt("totalPlayer", "member", "nonMember");

            dgv.Columns["code"].DataPropertyName = "match_code";
            dgv.Columns["date"].DataPropertyName = "match_date";
            dgv.Columns["type"].DataPropertyName = "type";
            dgv.Columns["title"].DataPropertyName = "match_title";
            dgv.Columns["host"].DataPropertyName = "match_host";
            dgv.Columns["totalPlayer"].DataPropertyName = "totalplayer";
            dgv.Columns["member"].DataPropertyName = "Member";
            dgv.Columns["nonMember"].DataPropertyName = "guest";

        }
        private void InitializeDateTimePicker()
        {
            dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
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
            AdjustToDate();
            SearchRecordBoardEvnt?.Invoke(this, EventArgs.Empty);

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
            AdjustToDate();
            SearchRecordBoardEvnt?.Invoke(this, EventArgs.Empty);
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

        public DateTime FromDate
        {
            get { return dtpFromDate.Value; }
            set { dtpFromDate.Value = value; }
        }

        public DateTime ToDate
        {
            get { return dtpToDate.Value; }
            set { dtpToDate.Value = value; }
        }

        public int? GetMatchCode
        {
            get
            {
                if (dgvMatchList.dgv.SelectedRows.Count > 0)
                {
                    return Convert.ToInt32(dgvMatchList.dgv.CurrentRow.Cells["code"].Value);
                }
                return null;
            }
        }

    

        public event EventHandler RecordBoardRegistEvent;
        public event EventHandler RecordBoardEditEvent;
        public event EventHandler RecordBoarSelectedEvent;
        public event EventHandler SearchRecordBoardEvnt;

        public void SetDataBinding(DataTable source)
        {
            dgvMatchList.dgv.DataSource = source;
        }

        public void ShowMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}
