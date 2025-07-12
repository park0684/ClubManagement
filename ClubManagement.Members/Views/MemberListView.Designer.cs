
namespace ClubManagement.Members.Views
{
    partial class MemberListView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnAddMember = new System.Windows.Forms.Button();
            this.pnlDataGrid = new System.Windows.Forms.Panel();
            this.dtpGameToDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkAccDate = new System.Windows.Forms.CheckBox();
            this.dtpAccFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpSecToDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpAccToDate = new System.Windows.Forms.DateTimePicker();
            this.chkSecDate = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpSecFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpGameFromDate = new System.Windows.Forms.DateTimePicker();
            this.chkGameDate = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkExRegularGeme = new System.Windows.Forms.CheckBox();
            this.chkExEventGame = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkExclude = new System.Windows.Forms.CheckBox();
            this.txtSearchWord = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbSortType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkExIrregularGame = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbInclude = new System.Windows.Forms.ComboBox();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnSearch.Location = new System.Drawing.Point(12, 106);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnAddMember
            // 
            this.btnAddMember.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnAddMember.Location = new System.Drawing.Point(93, 106);
            this.btnAddMember.Name = "btnAddMember";
            this.btnAddMember.Size = new System.Drawing.Size(75, 23);
            this.btnAddMember.TabIndex = 8;
            this.btnAddMember.Text = "회원 등록";
            this.btnAddMember.UseVisualStyleBackColor = true;
            // 
            // pnlDataGrid
            // 
            this.pnlDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDataGrid.Location = new System.Drawing.Point(12, 135);
            this.pnlDataGrid.Name = "pnlDataGrid";
            this.pnlDataGrid.Size = new System.Drawing.Size(1107, 272);
            this.pnlDataGrid.TabIndex = 6;
            // 
            // dtpGameToDate
            // 
            this.dtpGameToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpGameToDate.Location = new System.Drawing.Point(203, 20);
            this.dtpGameToDate.Name = "dtpGameToDate";
            this.dtpGameToDate.Size = new System.Drawing.Size(102, 23);
            this.dtpGameToDate.TabIndex = 5;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkAccDate);
            this.groupBox3.Controls.Add(this.dtpAccFromDate);
            this.groupBox3.Controls.Add(this.dtpSecToDate);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.dtpAccToDate);
            this.groupBox3.Controls.Add(this.chkSecDate);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.dtpSecFromDate);
            this.groupBox3.Location = new System.Drawing.Point(363, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(353, 88);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            // 
            // chkAccDate
            // 
            this.chkAccDate.AutoSize = true;
            this.chkAccDate.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.chkAccDate.Location = new System.Drawing.Point(6, 22);
            this.chkAccDate.Name = "chkAccDate";
            this.chkAccDate.Size = new System.Drawing.Size(62, 19);
            this.chkAccDate.TabIndex = 4;
            this.chkAccDate.Text = "등록일";
            this.chkAccDate.UseVisualStyleBackColor = true;
            // 
            // dtpAccFromDate
            // 
            this.dtpAccFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAccFromDate.Location = new System.Drawing.Point(74, 20);
            this.dtpAccFromDate.Name = "dtpAccFromDate";
            this.dtpAccFromDate.Size = new System.Drawing.Size(102, 23);
            this.dtpAccFromDate.TabIndex = 5;
            // 
            // dtpSecToDate
            // 
            this.dtpSecToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSecToDate.Location = new System.Drawing.Point(203, 50);
            this.dtpSecToDate.Name = "dtpSecToDate";
            this.dtpSecToDate.Size = new System.Drawing.Size(102, 23);
            this.dtpSecToDate.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.label3.Location = new System.Drawing.Point(182, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "~";
            // 
            // dtpAccToDate
            // 
            this.dtpAccToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAccToDate.Location = new System.Drawing.Point(203, 20);
            this.dtpAccToDate.Name = "dtpAccToDate";
            this.dtpAccToDate.Size = new System.Drawing.Size(102, 23);
            this.dtpAccToDate.TabIndex = 5;
            // 
            // chkSecDate
            // 
            this.chkSecDate.AutoSize = true;
            this.chkSecDate.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.chkSecDate.Location = new System.Drawing.Point(6, 52);
            this.chkSecDate.Name = "chkSecDate";
            this.chkSecDate.Size = new System.Drawing.Size(62, 19);
            this.chkSecDate.TabIndex = 4;
            this.chkSecDate.Text = "탈퇴일";
            this.chkSecDate.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.label4.Location = new System.Drawing.Point(182, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "~";
            // 
            // dtpSecFromDate
            // 
            this.dtpSecFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSecFromDate.Location = new System.Drawing.Point(74, 50);
            this.dtpSecFromDate.Name = "dtpSecFromDate";
            this.dtpSecFromDate.Size = new System.Drawing.Size(102, 23);
            this.dtpSecFromDate.TabIndex = 5;
            // 
            // dtpGameFromDate
            // 
            this.dtpGameFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpGameFromDate.Location = new System.Drawing.Point(74, 20);
            this.dtpGameFromDate.Name = "dtpGameFromDate";
            this.dtpGameFromDate.Size = new System.Drawing.Size(102, 23);
            this.dtpGameFromDate.TabIndex = 5;
            // 
            // chkGameDate
            // 
            this.chkGameDate.AutoSize = true;
            this.chkGameDate.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.chkGameDate.Location = new System.Drawing.Point(6, 22);
            this.chkGameDate.Name = "chkGameDate";
            this.chkGameDate.Size = new System.Drawing.Size(62, 19);
            this.chkGameDate.TabIndex = 4;
            this.chkGameDate.Text = "참가일";
            this.chkGameDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkGameDate.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.label5.Location = new System.Drawing.Point(182, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(15, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "~";
            // 
            // chkExRegularGeme
            // 
            this.chkExRegularGeme.AutoSize = true;
            this.chkExRegularGeme.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.chkExRegularGeme.Location = new System.Drawing.Point(6, 11);
            this.chkExRegularGeme.Name = "chkExRegularGeme";
            this.chkExRegularGeme.Size = new System.Drawing.Size(90, 19);
            this.chkExRegularGeme.TabIndex = 4;
            this.chkExRegularGeme.Text = "정기전 제외";
            this.chkExRegularGeme.UseVisualStyleBackColor = true;
            // 
            // chkExEventGame
            // 
            this.chkExEventGame.AutoSize = true;
            this.chkExEventGame.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.chkExEventGame.Location = new System.Drawing.Point(6, 36);
            this.chkExEventGame.Name = "chkExEventGame";
            this.chkExEventGame.Size = new System.Drawing.Size(102, 19);
            this.chkExEventGame.TabIndex = 4;
            this.chkExEventGame.Text = "이벤트전 제외";
            this.chkExEventGame.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkExclude);
            this.groupBox1.Controls.Add(this.txtSearchWord);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmbSortType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbStatus);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 88);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // chkExclude
            // 
            this.chkExclude.AutoSize = true;
            this.chkExclude.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.chkExclude.Location = new System.Drawing.Point(144, 13);
            this.chkExclude.Name = "chkExclude";
            this.chkExclude.Size = new System.Drawing.Size(50, 19);
            this.chkExclude.TabIndex = 5;
            this.chkExclude.Text = "제외";
            this.chkExclude.UseVisualStyleBackColor = true;
            // 
            // txtSearchWord
            // 
            this.txtSearchWord.Location = new System.Drawing.Point(58, 61);
            this.txtSearchWord.Name = "txtSearchWord";
            this.txtSearchWord.Size = new System.Drawing.Size(132, 23);
            this.txtSearchWord.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.label6.Location = new System.Drawing.Point(21, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "정렬";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbSortType
            // 
            this.cmbSortType.FormattingEnabled = true;
            this.cmbSortType.Location = new System.Drawing.Point(58, 36);
            this.cmbSortType.Name = "cmbSortType";
            this.cmbSortType.Size = new System.Drawing.Size(132, 23);
            this.cmbSortType.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.label1.Location = new System.Drawing.Point(21, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "상태";
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(58, 11);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(84, 23);
            this.cmbStatus.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.label2.Location = new System.Drawing.Point(9, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "검색어";
            // 
            // chkExIrregularGame
            // 
            this.chkExIrregularGame.AutoSize = true;
            this.chkExIrregularGame.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.chkExIrregularGame.Location = new System.Drawing.Point(6, 61);
            this.chkExIrregularGame.Name = "chkExIrregularGame";
            this.chkExIrregularGame.Size = new System.Drawing.Size(102, 19);
            this.chkExIrregularGame.TabIndex = 4;
            this.chkExIrregularGame.Text = "비정기전 제외";
            this.chkExIrregularGame.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkExIrregularGame);
            this.groupBox2.Controls.Add(this.chkExRegularGeme);
            this.groupBox2.Controls.Add(this.chkExEventGame);
            this.groupBox2.Location = new System.Drawing.Point(218, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(139, 88);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.cmbInclude);
            this.groupBox4.Controls.Add(this.dtpGameToDate);
            this.groupBox4.Controls.Add(this.chkGameDate);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.dtpGameFromDate);
            this.groupBox4.Location = new System.Drawing.Point(722, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(338, 88);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.label7.Location = new System.Drawing.Point(6, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 15);
            this.label7.TabIndex = 7;
            this.label7.Text = "포함 여부";
            // 
            // cmbInclude
            // 
            this.cmbInclude.FormattingEnabled = true;
            this.cmbInclude.Location = new System.Drawing.Point(74, 48);
            this.cmbInclude.Name = "cmbInclude";
            this.cmbInclude.Size = new System.Drawing.Size(170, 23);
            this.cmbInclude.TabIndex = 6;
            // 
            // MemberListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1131, 419);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnAddMember);
            this.Controls.Add(this.pnlDataGrid);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MemberListView";
            this.Text = "MemberList";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnAddMember;
        private System.Windows.Forms.Panel pnlDataGrid;
        private System.Windows.Forms.DateTimePicker dtpGameToDate;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkAccDate;
        private System.Windows.Forms.DateTimePicker dtpAccFromDate;
        private System.Windows.Forms.DateTimePicker dtpSecToDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpGameFromDate;
        private System.Windows.Forms.DateTimePicker dtpAccToDate;
        private System.Windows.Forms.CheckBox chkGameDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkSecDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpSecFromDate;
        private System.Windows.Forms.CheckBox chkExRegularGeme;
        private System.Windows.Forms.CheckBox chkExEventGame;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkExclude;
        private System.Windows.Forms.TextBox txtSearchWord;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkExIrregularGame;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbSortType;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbInclude;
    }
}