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

        /*필드*/

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

        /// <summary>
        /// DataGridView에 데이터 소스 바인딩.
        /// </summary>
        /// <param name="source">바인딩할 데이터 테이블</param>
        public void SetDataBinding(DataTable source)
        {
            dgvMatchList.dgv.DataSource = source;
        }

        /// <summary>
        /// 메시지 박스 출력
        /// </summary>
        /// <param name="message">출력할 메시지</param>
        public void ShowMessage(string message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 주요 이벤트 바인딩 메서드.
        /// </summary>
        private void ViewEvent()
        {
            btnAddGame.Click += (s, e) => RecordBoardRegistEvent?.Invoke(this, EventArgs.Empty);
            btnGameListEdit.Click += (s, e) => RecordBoardEditEvent?.Invoke(this, EventArgs.Empty);
            dgvMatchList.dgv.CellDoubleClick += (s, e) => RecordBoarSelectedEvent?.Invoke(this, EventArgs.Empty);

            // 날짜 이동 및 검색 이벤트
            btnPre.Click += MovePreMonth;
            btnPost.Click += MovePostMounth;
            dtpFromDate.ValueChanged += (s, e) => SearchRecordBoardEvnt?.Invoke(this, EventArgs.Empty);
            dtpToDate.ValueChanged += (s, e) => SearchRecordBoardEvnt?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// DataGridView 초기화 및 컬럼 설정.
        /// </summary>
        private void InitializeDataGridView()
        {
            dgvMatchList = new CustomDataGridViewControl();
            var dgv = dgvMatchList.dgv;
            pnlDataGrid.Controls.Add(dgv);
            dgv.Dock = DockStyle.Fill;

            // 컬럼 추가
            dgv.Columns.Add("code", "모임코드");
            dgv.Columns.Add("date", "날짜");
            dgv.Columns.Add("type", "유형");
            dgv.Columns.Add("title", "모임명");
            dgv.Columns.Add("host", "주최자");
            dgv.Columns.Add("totalPlayer", "참석인원");
            dgv.Columns.Add("member", "회원");
            dgv.Columns.Add("nonMember", "비회원");

            // DataGridView 속성 설정
            dgv.AutoGenerateColumns = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // 공통 포맷 적용
            dgvMatchList.ApplyDefaultColumnSettings();
            dgvMatchList.FormatAsDate("date");
            dgvMatchList.FormatAsStringLeft("title");
            dgvMatchList.FormatAsInt("totalPlayer", "member", "nonMember");

            // DataPropertyName 매핑
            dgv.Columns["code"].DataPropertyName = "match_code";
            dgv.Columns["date"].DataPropertyName = "match_date";
            dgv.Columns["type"].DataPropertyName = "type";
            dgv.Columns["title"].DataPropertyName = "match_title";
            dgv.Columns["host"].DataPropertyName = "match_host";
            dgv.Columns["totalPlayer"].DataPropertyName = "totalplayer";
            dgv.Columns["member"].DataPropertyName = "Member";
            dgv.Columns["nonMember"].DataPropertyName = "guest";
        }

        /// <summary>
        /// FromDate를 현재 월의 1일로 초기화.
        /// </summary>
        private void InitializeDateTimePicker()
        {
            dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        /// <summary>
        /// ▷ 버튼 클릭 시 한 달 뒤로 이동.
        /// 종료일도 조정.
        /// </summary>
        public void MovePostMounth(object sender, EventArgs e)
        {
            dtpFromDate.Value = dtpFromDate.Value.AddMonths(1);
            AdjustToDate();
            SearchRecordBoardEvnt?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// ◁ 버튼 클릭 시 한 달 앞으로 이동.
        /// 종료일도 조정.
        /// </summary>
        private void MovePreMonth(object sender, EventArgs e)
        {
            dtpFromDate.Value = dtpFromDate.Value.AddMonths(-1);
            AdjustToDate();
            SearchRecordBoardEvnt?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// FromDate 기준으로 ToDate를 마지막 날짜 또는 오늘 날짜로 설정.
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
