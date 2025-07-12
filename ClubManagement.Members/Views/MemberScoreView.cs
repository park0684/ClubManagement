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
        /// <summary>
        /// 조회 시작일
        /// </summary>
        public DateTime FromDate => dtpFromDate.Value;
        /// <summary>
        /// 조회 종료일
        /// </summary>
        public DateTime ToDate => dtpToDate.Value;
        /// <summary>
        /// 회원 상태
        /// </summary>
        public int Status => ((KeyValuePair<int, string>)cmbStatus.SelectedItem).Key;
        /// <summary>
        /// 정렬 조건
        /// </summary>
        public int SortType => cmbSortType.SelectedIndex;
        /// <summary>
        /// 검색 회원명
        /// </summary>
        public string SearchWord => txtSearchWord.Text;
        /// <summary>
        /// 모임 타이틀
        /// </summary>
        public string MatchTitel {set => txtMatchTitle.Text = value; }

        /// <summary>
        /// 모임 검색 여부
        /// </summary>
        public bool IsMatch => chkMatchSearch.Checked;
        /// <summary>
        /// 선택 회원 상태 제외
        /// </summary>
        public bool ExculuedMember => chkExclude.Checked;

        public event EventHandler SearchScoreEvent;
        public event Action<int> MemberSelectedEvent;
        public event EventHandler MatachSearchEvent;
        public event EventHandler CheckedEvent;
        public event EventHandler<List<int>> GradeUpdateEvet;

        /// <summary>
        /// 데이터 그리드 뷰에 데이터 바인딩
        /// </summary>
        /// <param name="result"></param>
        public void MemberScoreBinding(DataTable result)
        {
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

        /// <summary>
        /// 데이터 그리드 뷰 칼럼 등록 및 설정
        /// </summary>
        /// <param name="columns"></param>
        public void SetDatagirColumns(Dictionary<string, string> columns)
        {
            dgvScoreList.dgv.DataSource = null;
            dgvScoreList.dgv.Rows.Clear();
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
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dgvScoreList.dgv.AllowUserToResizeColumns = true;
            dgvScoreList.dgv.Columns["mem_code"].Visible = false;
            
        }

        /// <summary>
        /// 메시지 박스 표시
        /// </summary>
        /// <param name="messaga"></param>
        public void ShowMessga(string messaga)
        {
            MessageBox.Show(messaga);
        }

        /// <summary>
        /// 이벤트 등록
        /// </summary>
        private void ViewEvent()
        {
            btnSearch.Click += (s, e) => SearchScoreEvent?.Invoke(this, EventArgs.Empty);
            chkMatchSearch.CheckedChanged += chkMatchSearch_checked;
            dgvScoreList.dgv.CellDoubleClick += dgvScoreList_DoubleClick;
            
        }

        /// <summary>
        /// 데이터 그리드 뷰 더블 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvScoreList_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if( e.RowIndex >= 0)
            {
                int memberCode = Convert.ToInt32(dgvScoreList.dgv.CurrentRow.Cells["mem_code"].Value);
                MemberSelectedEvent?.Invoke(memberCode);
            }
        }

        /// <summary>
        /// 데이터 그리드 뷰 생성 및 초기화
        /// </summary>
        private void InitiailizeDataGridView()
        {
            dgvScoreList = new CustomDataGridViewControl();
            pnlDataGrid.Controls.Add(dgvScoreList.dgv);
            dgvScoreList.dgv.Dock = DockStyle.Fill;
            dgvScoreList.dgv.ReadOnly = true;
            btnPost.Click += MovePostMounth;
            btnPre.Click += MovePreMonth;
            dgvScoreList.dgv.MouseClick += dgvScoreList_RightClick;
            dgvScoreList.dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        /// <summary>
        /// 모임체크 버튼 클릭시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkMatchSearch_checked(object sender, EventArgs e)
        {
            //if (chkMatchSearch.Checked)
            MatachSearchEvent?.Invoke(this, EventArgs.Empty);
            //else
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
        private void dgvScoreList_RightClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                ContextMenuStrip context = new ContextMenuStrip();
                ToolStripSeparator separator = new ToolStripSeparator();

                context.Items.Add("펼쳐보기");
                context.Items.Add("줄여보기");
                context.Items.Add(separator);
                context.Items.Add("등급 일괄 설정");

                context.Show(pnlDataGrid, e.Location);
                context.ItemClicked += new ToolStripItemClickedEventHandler(Menu_Click);
            }
        }
        private void Menu_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            string clickItem = e.ClickedItem.Text;

            switch (clickItem)
            {
                case "펼쳐보기":
                    ColumnExpand();
                    break;
                case "줄여보기":
                    ColumnHiden();
                    break;
                case "등급 일괄 설정":
                    MemberGradeUpdate();
                    break;
            }
        }
        private void MemberGradeUpdate()
        {
            var selectedMember = new List<int>();
            
            foreach(DataGridViewRow row in dgvScoreList.dgv.SelectedRows)
            {
                if (row.Cells["mem_code"].Value != null )
                {
                    int.TryParse(row.Cells["mem_code"].Value.ToString(), out int memcode);
                    selectedMember.Add(memcode);
                }
            }
            GradeUpdateEvet?.Invoke(this, selectedMember);
        }
        private void ColumnExpand()
        {
            foreach (DataGridViewColumn column in dgvScoreList.dgv.Columns)
            {
                column.Visible = true;
            }
            dgvScoreList.dgv.Columns["mem_code"].Visible = false;
        }
        private void ColumnHiden()
        {
            var dgv = dgvScoreList.dgv;
            foreach (DataGridViewColumn column in dgvScoreList.dgv.Columns)
            {
                column.Visible = false;
            }
            dgv.Columns["No"].Visible = true;
            dgv.Columns["mem_name"].Visible = true;
            dgv.Columns["reference_average"].Visible = true;
            dgv.Columns["average_score"].Visible = true;
            dgv.Columns["reference_gap"].Visible = true;
        }
    }
}
