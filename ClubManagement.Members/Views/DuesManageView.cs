﻿using System;
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

namespace ClubManagement.Members.Views
{
    public partial class DuesManageView : Form,IDuesManageView
    {
        CustomDataGridViewControl dgvMemberList;
        CustomDataGridViewControl dgvStatementList;
        public DuesManageView()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitailizeComboBox();
            ViewEvevnt();
            dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        /// <summary>
        /// 뷰 이벤트 연결 설정
        /// </summary>
        private void ViewEvevnt()
        {
            btnSearchMember.Click += (s, e) => MemberSearchEvent?.Invoke(this, EventArgs.Empty); // 회원검색 이벤트
            btnAdd.Click += (s, e) => StatementAddEvent?.Invoke(this, EventArgs.Empty); // 전표 추가 등록
            btnPost.Click += MovePostMounth; // 조회 날짜 한달후로 이동 이벤트
            btnPre.Click += MovePreMonth; // 조회 날짜 한달전으로 이동
            dgvMemberList.dgv.CellDoubleClick += (s, e) => StatementSearchEvent?.Invoke(this, EventArgs.Empty); //회원리스트 더블 클릭
            dgvStatementList.dgv.CellDoubleClick += (s, e) => StatementEditEvent?.Invoke(this, EventArgs.Empty); // 전표 더블 클릭
            dtpFromDate.ValueChanged += (s, e) => StatementSearchEvent?.Invoke(this, EventArgs.Empty); // 날짜 변경 시 전표 조회
            dtpToDate.ValueChanged += (s, e) => StatementSearchEvent?.Invoke(this, EventArgs.Empty); // 날짜 변경 시 전표 조회
            cmbStatus.SelectedIndexChanged += (s, e) => StatementSearchEvent?.Invoke(this, EventArgs.Empty); // 전표 유형 선택
        }

        /// <summary>
        /// 데이트 그리드 뷰 초기화 및 칼럼 설정
        /// </summary>
        private void InitializeDataGridView()
        {

            dgvMemberList = new CustomDataGridViewControl();
            dgvStatementList = new CustomDataGridViewControl();
            var dgvMember = dgvMemberList.dgv;
            var dgvState = dgvStatementList.dgv;
            pnlMember.Controls.Add(dgvMember);
            dgvMember.Dock = DockStyle.Fill;
            pnlStatemet.Controls.Add(dgvState);
            dgvState.Dock = DockStyle.Fill;

            //회원 리스트 칼럼 설정
            dgvMember.Columns.Add("code", "코드");
            dgvMember.Columns.Add("name", "회원명");
            dgvMember.Columns.Add("totalDues", "대상");
            dgvMember.Columns.Add("payment", "납부");
            dgvMember.Columns.Add("nonPayment", "미납");
            dgvMember.Columns.Add("free", "면제");
            dgvMember.ReadOnly = true;
            dgvMember.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            foreach (DataGridViewColumn column in dgvMember.Columns)
            {
                column.DataPropertyName = column.Name;
            }
            dgvMember.AutoGenerateColumns = false;
            dgvMember.Columns["Code"].Visible = false;
            dgvMemberList.ApplyDefaultColumnSettings();
            dgvMemberList.FormatAsStringCenter("mem_birht");
            dgvMemberList.FormatAsStringLeft("name");
            dgvMemberList.FormatAsInt("totalDues", "payment", "nonPayment", "fee");

            //매출입 전표 칼럼 설정
            dgvState.Columns.Add("date", "날짜");
            dgvState.Columns.Add("type", "항목");
            dgvState.Columns.Add("statement", "내용");
            dgvState.Columns.Add("applay", "적용");
            dgvState.Columns.Add("deposit", "입금액");
            dgvState.Columns.Add("withdrawal", "출금액");
            dgvState.Columns.Add("balance", "잔액");
            dgvState.Columns.Add("memo", "메모");
            dgvState.Columns.Add("code", "코드");
            dgvState.ReadOnly = true;
            dgvState.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            foreach (DataGridViewColumn column in dgvState.Columns)
            {
                column.DataPropertyName = column.Name;
            }
            dgvState.AutoGenerateColumns = false;

            dgvStatementList.ApplyDefaultColumnSettings();
            dgvStatementList.FormatAsInt("deposit", "withdrawal", "balance");
            dgvStatementList.FormatAsStringCenter("No", "code", "type");
            dgvStatementList.FormatAsStringLeft("memo");
            dgvStatementList.FormatAsDate("date");
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
            StatementSearchEvent?.Invoke(this, EventArgs.Empty);

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
            StatementSearchEvent?.Invoke(this, EventArgs.Empty);
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
        
        /// <summary>
        /// 콤보 박스 초기화 및 설정
        /// </summary>
        private void InitailizeComboBox()
        {
            //전표 유형 참조
            var items = MemberHelper.DuesType.Select(kvp => new KeyValuePair<int, string>(kvp.Key, kvp.Value)).ToList();
            items.Insert(0, new KeyValuePair<int, string>(-1, "전체"));

            cmbStatus.DataSource = items;
            cmbStatus.DisplayMember = "Value";
            cmbStatus.ValueMember = "Key";
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.SelectedIndex = 0;
        }

        /// <summary>
        /// 회원 DataGridView에 데이터소스 바인딩 메서드
        /// </summary>
        /// <param name="members"></param>
        public void SetMemberListBinding(DataTable members)
        {
            dgvMemberList.dgv.DataSource = members;
        }

        /// <summary>
        /// 전표 DataGridViewdp 데이터소스 바인딩 메서드
        /// </summary>
        /// <param name="states"></param>
        public void SetStateListBinding(DataTable states)
        {
            dgvStatementList.dgv.DataSource = states;
        }

        // 필드
        public string SearchWord
        {
            get { return txtSearchWord.Text; }
            set { txtSearchWord.Text = value; }
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

        public bool SecessInclude
        {
            get { return chkSeceInculde.Checked; }
            set { chkSeceInculde.Checked = value; }
        }
        public int? GetMemberCode
        {
            get
            {
                if (dgvMemberList.dgv.SelectedRows.Count > 0)
                {
                    return Convert.ToInt32(dgvMemberList.dgv.CurrentRow.Cells["Code"].Value);
                }
                return null;
            }
        }

        public int? GetStateMentCode
        {
            get
            {
                if (dgvStatementList.dgv.SelectedRows.Count > 0)
                {
                    return Convert.ToInt32(dgvStatementList.dgv.CurrentRow.Cells["code"].Value);
                }
                return null;
            }
        }

        public int? StateType 
        {

            get
            {
                if (cmbStatus.SelectedValue is int val)
                    return val;
                return null;
            }
            set
            {
                foreach (var item in cmbStatus.Items)
                {
                    if (item is KeyValuePair<int, string> kv && kv.Key == value)
                    {
                        cmbStatus.SelectedItem = kv;
                        break;
                    }
                }
            }
        }

        //이벤트
        public event EventHandler MemberSearchEvent;
        public event EventHandler StatementSearchEvent;
        public event EventHandler StatementAddEvent;
        public event EventHandler StatementEditEvent;
    }
}
