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
    public partial class SearchMatchView : Form, ISearchMatchView
    {
        CustomDataGridViewControl dgvMatchList;
        public SearchMatchView()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeDateTimePicker();
            ViewEvent();
        }

        public event EventHandler CloseFormEvent;
        public event EventHandler SearchMatchEvent;
        public event EventHandler SelectedMatchEvent;

        /// <summary>
        /// 뷰의 주요 컨트롤 이벤트를 초기화 후 바인딩.
        /// 모임 목록 그리드 더블클릭, 날짜 변경, 월 이동 버튼, 닫기 버튼 이벤트를 연결.
        /// </summary>
        private void ViewEvent()
        {
            btnClose.Click += (s, e) => CloseFormEvent?.Invoke(this, EventArgs.Empty);

            dgvMatchList.dgv.CellDoubleClick += (s, e) => SelectedMatchEvent?.Invoke(this, EventArgs.Empty);

            // 내부 컨트롤 이벤트
            btnPre.Click += MovePreMonth;
            btnPost.Click += MovePostMounth;
            dtpFromDate.ValueChanged += (s, e) => SearchMatchEvent?.Invoke(this, EventArgs.Empty);
            dtpToDate.ValueChanged += (s, e) => SearchMatchEvent?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// 모임 목록 데이터 그리드 뷰(dgvMatchList)를 초기화하고 컬럼과 스타일을 설정.
        /// </summary>
        /// <remarks>
        /// - CustomDataGridViewControl을 생성하여 패널(pnlDataGrid)에 추가
        /// - 모임 코드, 날짜, 유형, 모임명, 주최자, 참석인원 등 컬럼 생성
        /// - 컬럼에 DataPropertyName 바인딩
        /// - 읽기 전용, 행 전체 선택, 자동 생성 컬럼 해제
        /// - CustomDataGridViewControl의 공통 스타일 및 포맷 적용
        /// </remarks>
        private void InitializeDataGridView()
        {
            dgvMatchList = new CustomDataGridViewControl();
            var dgv = dgvMatchList.dgv;

            pnlDataGrid.Controls.Add(dgv);
            dgv.Dock = DockStyle.Fill;

            // 컬럼 정의
            dgv.Columns.Add("code", "모임코드");
            dgv.Columns.Add("date", "날짜");
            dgv.Columns.Add("type", "유형");
            dgv.Columns.Add("title", "모임명");
            dgv.Columns.Add("host", "주최자");
            dgv.Columns.Add("totalPlayer", "참석인원");
            dgv.Columns.Add("member", "회원");
            dgv.Columns.Add("nonMember", "비회원");
            dgv.Columns.Add("isRecord", "등록여부");
            dgv.Columns["isRecord"].Visible = false; // 내부 데이터용, 화면 숨김

            // 기본 설정
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoGenerateColumns = false;

            // 공통 스타일 적용
            dgvMatchList.ApplyDefaultColumnSettings();
            dgvMatchList.FormatAsDate("date");
            dgvMatchList.FormatAsStringLeft("title");
            dgvMatchList.FormatAsInt("totalPlayer", "member", "nonMember", "code");

            // 개별 스타일
            dgv.Columns["code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // 데이터 바인딩 속성 설정
            dgv.Columns["code"].DataPropertyName = "match_code";
            dgv.Columns["date"].DataPropertyName = "match_date";
            dgv.Columns["type"].DataPropertyName = "type";
            dgv.Columns["title"].DataPropertyName = "match_title";
            dgv.Columns["host"].DataPropertyName = "match_host";
            dgv.Columns["totalPlayer"].DataPropertyName = "totalplayer";
            dgv.Columns["member"].DataPropertyName = "Member";
            dgv.Columns["nonMember"].DataPropertyName = "guest";
            dgv.Columns["isRecord"].DataPropertyName = "match_record";
        }

        /// <summary>
        /// FromDate DateTimePicker의 값을 현재 월의 1일로 초기화.
        /// </summary>
        private void InitializeDateTimePicker()
        {
            dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        /// <summary>
        /// ▷ 버튼 클릭 시 실행
        /// FromDate를 한 달 뒤로 이동시키고 ToDate를 보정한 뒤, SearchMatchEvent를 발생.
        /// </summary>
        /// <param name="sender">이벤트를 발생시킨 컨트롤</param>
        /// <param name="e">이벤트 데이터</param>
        public void MovePostMounth(object sender, EventArgs e)
        {
            dtpFromDate.Value = dtpFromDate.Value.AddMonths(1);
            AdjustToDate();
            SearchMatchEvent?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// ◁ 버튼 클릭 시 실행
        /// FromDate 한 달 전으로 변경
        /// ToDate 보정 후 SearchMatchEvent 발생
        /// </summary>
        /// <param name="sender">이벤트 발생 컨트롤</param>
        /// <param name="e">이벤트 데이터</param>
        private void MovePreMonth(object sender, EventArgs e)
        {
            dtpFromDate.Value = dtpFromDate.Value.AddMonths(-1);
            AdjustToDate();
            SearchMatchEvent?.Invoke(this, EventArgs.Empty);
        }


        /// <summary>
        /// DateTimePicker 변경 버튼 클릭시 FromDate 기준 ToDate 자동 보정
        /// 현재 월이면 오늘 날짜, 아니면 월 마지막 날 설정
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

        /// <summary>
        /// DataGridView 데이터 바인딩
        /// </summary>
        public void SetDataBinding(DataTable source)
        {
            dgvMatchList.dgv.DataSource = source;
        }

        /// <summary>
        /// 메시지 박스 생성
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "알림");
        }


        /// <summary>
        /// 폼 중앙 표시
        /// </summary>
        public void ShowForm()
        {
            StartPosition = FormStartPosition.CenterParent;
            ShowDialog();
        }

        /// <summary>
        /// 폼 종료
        /// </summary>
        public void CloseForm()
        {
            Close();
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

        public int MatchCode
        {
            get
            {
                if (dgvMatchList.dgv.CurrentRow == null) return -1;
                return Convert.ToInt32(dgvMatchList.dgv.CurrentRow.Cells["code"].Value);
            }
        }

        public string MatchTitle
        {
            get
            {
                if (dgvMatchList.dgv.CurrentRow == null) return string.Empty;
                return dgvMatchList.dgv.CurrentRow.Cells["title"].Value.ToString();
            }
        }

        public bool IsRecodeRegisted
        {
            get
            {
                if (dgvMatchList.dgv.CurrentCell == null)
                    return false;
                return Convert.ToBoolean(dgvMatchList.dgv.CurrentRow.Cells["isRecord"].Value);
            }

        }
    }
}
