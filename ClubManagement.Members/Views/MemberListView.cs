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
using ClubManagement.Common.Controls;
using ClubManagement.Members.DTOs;

namespace ClubManagement.Members.Views
{
    public partial class MemberListView : Form, IMemberListView
    {
        CustomDataGridViewControl dgvMemberList;
        public MemberListView()
        {
            InitializeComponent();
            InitializDateTimePicker();
            InitializeComboBox();
            InitializeDataGridView();
            ViewEvent();
        }
        /// <summary>
        /// 콤보박스 초기화
        /// </summary>
        private void InitializeComboBox()
        {
            //회원 상태 콤보박스 
            //MemberHelper에서 상태값 참조 등록
            cmbStatus.Items.Add("전체");
            foreach (var item in MemberHelper.MemStatus)
            {
                cmbStatus.Items.Add(new KeyValuePair<int, string>(item.Key, item.Value));
            }
            cmbStatus.DisplayMember = "Value";
            cmbStatus.ValueMember = "key";
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.SelectedIndex = 1;

            //정렬 조건 콤보박스
            string[] sortItems = new string[] { "기본", "참석율↑", "참석율↓", "정기전↑", "정기전↓", "정기전&참석율↑" };
            foreach(var item in sortItems )
            {
                cmbSortType.Items.Add(item);
            }
            cmbSortType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSortType.SelectedIndex = 0;

            //게임참가기간내 회원 포함여부 지정 콤보박스
            string[] inclusion = new string[] { "전체회원", "참석자", "미참석자" };
            foreach(var item in inclusion)
            {
                cmbInclude.Items.Add(item);
            }
            cmbInclude.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbInclude.SelectedIndex = 0;
            cmbInclude.Enabled = false;
        }

        /// <summary>
        /// DataPicker 상태 초기화
        /// </summary>
        private void InitializDateTimePicker()
        {
            dtpAccFromDate.Enabled = false;
            dtpAccToDate.Enabled = false;
            dtpSecFromDate.Enabled = false;
            dtpSecToDate.Enabled = false;
            dtpGameFromDate.Enabled = false;
            dtpGameToDate.Enabled = false;
            chkExclude.Enabled = false;
        }

        /// <summary>
        /// 데이터 그리드 초기화 및 칼럼 설정
        /// </summary>
        private void InitializeDataGridView()
        {
            dgvMemberList = new CustomDataGridViewControl();
            var dgv = dgvMemberList.dgv;
            pnlDataGrid.Controls.Add(dgv);
            dgv.Dock = DockStyle.Fill;

            // 그리드 칼럼 추가
            dgv.Columns.Add("MemberCode", "code");
            dgv.Columns.Add("Name", "이름");
            dgv.Columns.Add("Brith", "생년");
            dgv.Columns.Add("Gender", "성별");
            dgv.Columns.Add("Status", "상태");
            dgv.Columns.Add("Position", "직책");
            dgv.Columns.Add("RegularMatch", "정기전");
            dgv.Columns.Add("RegularRate", "참석율");
            dgv.Columns.Add("IrregularMatch", "비정기전");
            dgv.Columns.Add("EventMatch", "이벤트전");
            dgv.Columns.Add("Payment", "납부");
            dgv.Columns.Add("NonPayament", "미납");
            dgv.Columns.Add("AccessDate", "가입일");
            dgv.Columns.Add("SecessDate", "탈퇴일");
            dgv.Columns.Add("LastMatchDate", "참가일");
            dgv.Columns.Add("Memo", "메모");
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ReadOnly = true;

            // 바인딩을 위한 프로퍼티 네임 설정
            //foreach(DataGridViewColumn column in dgv.Columns)
            //{
            //    column.DataPropertyName = column.Name;
            //}
            dgv.Columns["No"].DataPropertyName = "No";
            dgv.Columns["MemberCode"].DataPropertyName = "mem_code";
            dgv.Columns["Name"].DataPropertyName = "mem_name";
            dgv.Columns["Brith"].DataPropertyName = "mem_birth";
            dgv.Columns["Gender"].DataPropertyName = "gender";
            dgv.Columns["Status"].DataPropertyName = "status";
            dgv.Columns["Position"].DataPropertyName = "position";
            dgv.Columns["RegularMatch"].DataPropertyName = "regular_count";
            dgv.Columns["RegularRate"].DataPropertyName = "regularRate";
            dgv.Columns["IrregularMatch"].DataPropertyName = "irregular_count";
            dgv.Columns["EventMatch"].DataPropertyName = "event_count";
            dgv.Columns["Payment"].DataPropertyName = "payment";
            dgv.Columns["NonPayament"].DataPropertyName = "nonPayment";
            dgv.Columns["AccessDate"].DataPropertyName = "mem_access";
            dgv.Columns["SecessDate"].DataPropertyName = "mem_secess";
            dgv.Columns["LastMatchDate"].DataPropertyName = "match_last";
            dgv.Columns["Memo"].DataPropertyName = "mem_memo";

            //코드 칼럼 및 칼럼 자동생성 비화성
            dgv.Columns["MemberCode"].Visible = false;
            dgv.AutoGenerateColumns = false;

            //칼럼 포멧 설정
            dgvMemberList.ApplyDefaultColumnSettings();
            dgvMemberList.FormatAsInt("irregular", "regular", "event", "payment", "nonPayent");
            dgvMemberList.FormatAsStringCenter("name", "birth", "gender", "status", "position");
            dgvMemberList.FormatAsDecimal("regularRate");
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

        }

        /// <summary>
        /// view 이벤트 연결 설정
        /// </summary>
        private void ViewEvent()
        {
            btnSearch.Click += (s, e) => SearchEvent?.Invoke(this, EventArgs.Empty); // 검색 버튼
            txtSearchWord.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                    SearchEvent?.Invoke(this, EventArgs.Empty); // 검색어 입력 후 enter 이벤트
            };
            btnAddMember.Click += (s, e) => AddMemberEvent?.Invoke(this, EventArgs.Empty); // 새회원 추가 버튼 클릭
            dgvMemberList.dgv.CellDoubleClick += (s, e) => EidtMemberEvent?.Invoke(this, EventArgs.Empty); //  회원 상세보기 
            chkAccDate.CheckedChanged += SetAccDatePickersEnabled; //가입일 체크
            chkSecDate.CheckedChanged += SetSecDatePickersEnabled; //탈퇴일 체크 
            chkGameDate.CheckedChanged += SetGameDatePickersEnabled; //참석일 체크 박스 이벤트
            cmbStatus.SelectedIndexChanged += SetStatusExcludeCheckBoxSetㄴㅅㅁ; //상태 콤보 박스 변경시 이벤트
            chkExRegularGeme.CheckedChanged += SetCheckBoxEnable; //정기전 제외 체크 박스 이벤트
            chkExIrregularGame.CheckedChanged += SetCheckBoxEnable; //비정기전 제외 체크 박스 이벤트
            chkExEventGame.CheckedChanged += SetCheckBoxEnable; // 이벤트전 제외 체크 박스 이벤트
        }

        /// <summary>
        /// 데이터그리드뷰 데이터소스 바인딩 
        /// </summary>
        /// <param name="source"></param>
        public void SetMemberListBindingSource(BindingSource source)
        {
            dgvMemberList.dgv.DataSource = source;
        }

        /// <summary>
        /// 상태 콤보박스 선태값에 따른 제외 체크박스 활성화 여부
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetStatusExcludeCheckBoxSetㄴㅅㅁ(object sender, EventArgs e)
        {
            if (cmbStatus.SelectedIndex == 0)
            {
                chkExclude.Checked = false;
                chkExclude.Enabled = false;
            }
            else
            {
                chkExclude.Enabled = true;
            }
        }

        /// <summary>
        /// 모임 유형 제외 체크 박스 관리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetCheckBoxEnable(object sender, EventArgs e)
        {
            if (chkExRegularGeme.Checked && chkExIrregularGame.Checked && chkExEventGame.Checked)
            {
                MessageBox.Show("최소 한개 이상의 게임은 선택해야 합니다", "알림");
                CheckBox lastChecked = sender as CheckBox;
                if (lastChecked != null)
                {
                    lastChecked.Checked = false;
                }
            }
        }

        /// <summary>
        /// 가입일 DataPicker 활성화 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetAccDatePickersEnabled(object sender, EventArgs e)
        {
            bool isEnabled = chkAccDate.Checked;
            dtpAccFromDate.Enabled = isEnabled;
            dtpAccToDate.Enabled = isEnabled;
        }

        /// <summary>
        /// 탈퇴일 DataPicker 활성화 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetSecDatePickersEnabled(object sender, EventArgs e)
        {
            bool isEnabled = chkSecDate.Checked;
            dtpSecFromDate.Enabled = isEnabled;
            dtpSecToDate.Enabled = isEnabled;
        }

        /// <summary>
        /// 참가일 DataPicker 및 포함 콤보박스 활성화 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetGameDatePickersEnabled(object sender, EventArgs e)
        {
            bool isEnabled = chkGameDate.Checked;
            dtpGameFromDate.Enabled = isEnabled;
            dtpGameToDate.Enabled = isEnabled;
            cmbInclude.Enabled = isEnabled;
            if(!isEnabled)
            {
                cmbInclude.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 메시지 박스 생성 메서드
        /// </summary>
        /// <param name="message"></param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "알림");
        }

        /// <summary>
        /// 회원검색 결과 DataGridView 바인딩 메서드
        /// </summary>
        /// <param name="members"></param>
        public void MemberListBinding(DataTable members)
        {
          
            dgvMemberList.dgv.DataSource = members;

        }

        public void ClearDataGridView()
        {
            dgvMemberList.dgv.Rows.Clear();
        }

        // 필드
        public string SearchWord
        {
            get { return txtSearchWord.Text; }
            set { txtSearchWord.Text = value; }
        }
        public int? Status
        {
            get
            {
                if (cmbStatus.SelectedIndex == 0)
                    return 0;
                return ((KeyValuePair<int, string>)cmbStatus.SelectedItem).Key;
            }
            set { cmbStatus.SelectedIndex = (int)value; }
        }
        public DateTime? AccFromDate
        {
            get { return dtpAccFromDate.Value; }
            set { dtpAccFromDate.Value = (DateTime)value; }
        }
        public DateTime? AccToDate
        {
            get { return dtpAccToDate.Value; }
            set { dtpAccToDate.Value = (DateTime)value; }
        }
        public DateTime? SecFromDate
        {
            get { return dtpSecFromDate.Value; }
            set { dtpSecFromDate.Value = (DateTime)value; }
        }
        public DateTime? SecToDate
        {
            get { return dtpSecToDate.Value; }
            set { dtpSecToDate.Value = (DateTime)value; }
        }
        public DateTime? GameFromDate
        {
            get { return dtpGameFromDate.Value; }
            set { dtpGameFromDate.Value = (DateTime)value; }
        }
        public DateTime? GameToDate
        {
            get { return dtpGameToDate.Value; }
            set { dtpGameToDate.Value = (DateTime)value; }
        }
        public bool ExcludeRegular
        {
            get { return chkExRegularGeme.Checked; }
            set { chkExRegularGeme.Checked = value; }
        }
        public bool ExcludeIrregular
        {
            get { return chkExIrregularGame.Checked; }
            set { chkExRegularGeme.Checked = value; }
        }
        public bool ExcludeEvent
        {
            get { return chkExEventGame.Checked; }
            set { chkExEventGame.Checked = value; }
        }
        public bool ExcludeMember
        {
            get { return chkExclude.Checked; }
            set { chkExclude.Checked = value; }
        }
        public bool AccessCheck
        {
            get { return chkAccDate.Checked; }
            set { chkAccDate.Enabled = value; }
        }
        public bool SecessCheck
        {
            get { return chkSecDate.Checked; }
            set { chkSecDate.Checked = value; }
        }
        public bool GameCheck
        {
            get { return chkGameDate.Checked; }
            set { chkGameDate.Checked = value; }
        }

        public int? SelectedCode
        {
            get
            {
                if (dgvMemberList.dgv.SelectedRows.Count > 0)
                {
                    return Convert.ToInt32(dgvMemberList.dgv.CurrentRow.Cells["MemberCode"].Value);
                }
                return null;
            }
        }

        public int? SortType { get => cmbSortType.SelectedIndex; }
        public int? AttendInclude { get => cmbInclude.SelectedIndex; }

        // 이벤트
        public event EventHandler SearchEvent;
        public event EventHandler AddMemberEvent;
        public event EventHandler EidtMemberEvent;
    }
}
