using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManagement.Members.Views
{
    public partial class GradeManageView : Form, IGradeManageView
    {
        public GradeManageView()
        {
            InitializeComponent();
            ViewEvent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ControlBox = false;
            this.Text = "회원 등급 설정";
        }

        public event EventHandler AddGradeEvent;
        public event EventHandler DeleteGradeEvent;
        public event EventHandler<DataTable> SaveEvent;
        public event EventHandler CloseFormEvent;

        public void AddGradeItem()
        {
            int newCode = tlpGrade.RowCount;
            tlpGrade.RowCount = newCode + 1;

            tlpGrade.Height = tlpGrade.RowCount * 30;

            var lblCode = CreateCodeLabel(newCode);
            var txtName = CreateGradeTextBox("");

            tlpGrade.Controls.Add(lblCode, 0, newCode);
            tlpGrade.Controls.Add(txtName, 1, newCode);
        }

        public void BindingGradeData(DataTable result)
        {
            // tplGrade가 있는 패널의 스크롤 사용 활성
            pnlView.AutoScroll = true;
            
            // 바인딩 할 데이터 수에 맞게 행 추가
            tlpGrade.RowCount = result.Rows.Count + 1;
            int i = 1;
            tlpGrade.Height = tlpGrade.RowCount * 30;
            // 데이터 테이블의 로우 정보로 객체 생성 및 등록
            foreach(DataRow row in result.Rows)
            {
                var lblCode = CreateCodeLabel(Convert.ToInt32(row["grd_code"]));
                var txtName = CreateGradeTextBox(row["grd_name"].ToString());

                tlpGrade.Controls.Add(lblCode, 0, i);
                tlpGrade.Controls.Add(txtName, 1, i);

                i++;
            }
        }

        public void CloseForm()
        {
            this.Close();
        }

        public void DeleteGradeItem()
        {
            int lastIndex = tlpGrade.RowCount - 1;
            var lblControl = tlpGrade.GetControlFromPosition(0, lastIndex) as Label;
            var txtControl = tlpGrade.GetControlFromPosition(1, lastIndex) as TextBox;
            tlpGrade.Controls.Remove(lblControl);
            tlpGrade.Controls.Remove(txtControl);
            tlpGrade.RowCount = lastIndex;
            tlpGrade.Height = tlpGrade.RowCount * 30;
        }

        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }

        public void ShowMessagebox(string message)
        {
            MessageBox.Show(message);
        }

        private void ViewEvent()
        {
            btnClose.Click += (s, e) => CloseFormEvent(this, EventArgs.Empty);
            btnAdd.Click += (s, e) => AddGradeEvent(this, EventArgs.Empty);
            btnDelete.Click += (s, e) => DeleteGradeEvent(this, EventArgs.Empty);
            btnSave.Click += GetGradeItem;
        }
        private void GetGradeItem(object sender, EventArgs e)
        {
            var resultDate = new DataTable();
            resultDate.Columns.Add("grd_code", typeof(int));
            resultDate.Columns.Add("grd_name", typeof(string));
            int indexCount = tlpGrade.RowCount - 1;
            for(int i = 1; i <= indexCount; i++)
            {
                var lbl = tlpGrade.GetControlFromPosition(0, i);
                var txt = tlpGrade.GetControlFromPosition(1, i);
                resultDate.Rows.Add(lbl.Text, txt.Text);
            }
            SaveEvent?.Invoke(this, resultDate);
        }
        private Label CreateCodeLabel(int code)
        {
            Label lblCode = new Label
            {
                Text = code.ToString(),
                Dock = DockStyle.Fill,
                Font = new Font("맑은 고딕", 9),
                TextAlign = ContentAlignment.MiddleCenter
            };

            return lblCode;
        }

        private TextBox CreateGradeTextBox(string name)
        {
            TextBox txtName = new TextBox
            {
                Text = name,
                Dock = DockStyle.Fill,
                Font = new Font("맑은 고딕", 9)
            };

            return txtName;
        }
    }
}
