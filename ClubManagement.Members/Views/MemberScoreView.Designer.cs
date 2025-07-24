
namespace ClubManagement.Members.Views
{
    partial class MemberScoreView
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
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.btnPre = new System.Windows.Forms.Button();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.btnPost = new System.Windows.Forms.Button();
            this.pnlDataGrid = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkMatchSearch = new System.Windows.Forms.CheckBox();
            this.txtMatchTitle = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkExclude = new System.Windows.Forms.CheckBox();
            this.txtSearchWord = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbSortType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnGradeConfig = new System.Windows.Forms.Button();
            this.btnAverageConfig = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(36, 18);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(99, 23);
            this.dtpFromDate.TabIndex = 9;
            // 
            // btnPre
            // 
            this.btnPre.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPre.Location = new System.Drawing.Point(6, 14);
            this.btnPre.Name = "btnPre";
            this.btnPre.Size = new System.Drawing.Size(24, 30);
            this.btnPre.TabIndex = 11;
            this.btnPre.Text = "◁";
            this.btnPre.UseVisualStyleBackColor = true;
            // 
            // dtpToDate
            // 
            this.dtpToDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(151, 18);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(99, 23);
            this.dtpToDate.TabIndex = 8;
            // 
            // btnPost
            // 
            this.btnPost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPost.Location = new System.Drawing.Point(256, 14);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(24, 30);
            this.btnPost.TabIndex = 10;
            this.btnPost.Text = "▷";
            this.btnPost.UseVisualStyleBackColor = true;
            // 
            // pnlDataGrid
            // 
            this.pnlDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDataGrid.Location = new System.Drawing.Point(12, 140);
            this.pnlDataGrid.Name = "pnlDataGrid";
            this.pnlDataGrid.Size = new System.Drawing.Size(1060, 409);
            this.pnlDataGrid.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkMatchSearch);
            this.groupBox1.Controls.Add(this.txtMatchTitle);
            this.groupBox1.Controls.Add(this.btnPre);
            this.groupBox1.Controls.Add(this.btnPost);
            this.groupBox1.Controls.Add(this.dtpToDate);
            this.groupBox1.Controls.Add(this.dtpFromDate);
            this.groupBox1.Location = new System.Drawing.Point(218, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 88);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            // 
            // chkMatchSearch
            // 
            this.chkMatchSearch.AutoSize = true;
            this.chkMatchSearch.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.chkMatchSearch.Location = new System.Drawing.Point(6, 61);
            this.chkMatchSearch.Name = "chkMatchSearch";
            this.chkMatchSearch.Size = new System.Drawing.Size(74, 19);
            this.chkMatchSearch.TabIndex = 27;
            this.chkMatchSearch.Text = "모임선택";
            this.chkMatchSearch.UseVisualStyleBackColor = true;
            // 
            // txtMatchTitle
            // 
            this.txtMatchTitle.Enabled = false;
            this.txtMatchTitle.Location = new System.Drawing.Point(87, 57);
            this.txtMatchTitle.Name = "txtMatchTitle";
            this.txtMatchTitle.Size = new System.Drawing.Size(193, 23);
            this.txtMatchTitle.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkExclude);
            this.groupBox3.Controls.Add(this.txtSearchWord);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.cmbSortType);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.cmbStatus);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 88);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.label2.Location = new System.Drawing.Point(21, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "상태";
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(58, 11);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(84, 23);
            this.cmbStatus.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.label3.Location = new System.Drawing.Point(9, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "회원명";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(112)))), ((int)(((byte)(247)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(12, 107);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 25);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnGradeConfig
            // 
            this.btnGradeConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(112)))), ((int)(((byte)(247)))));
            this.btnGradeConfig.FlatAppearance.BorderSize = 0;
            this.btnGradeConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGradeConfig.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnGradeConfig.ForeColor = System.Drawing.Color.White;
            this.btnGradeConfig.Location = new System.Drawing.Point(93, 107);
            this.btnGradeConfig.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGradeConfig.Name = "btnGradeConfig";
            this.btnGradeConfig.Size = new System.Drawing.Size(75, 25);
            this.btnGradeConfig.TabIndex = 27;
            this.btnGradeConfig.Text = "등급설정";
            this.btnGradeConfig.UseVisualStyleBackColor = false;
            // 
            // btnAverageConfig
            // 
            this.btnAverageConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(112)))), ((int)(((byte)(247)))));
            this.btnAverageConfig.FlatAppearance.BorderSize = 0;
            this.btnAverageConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAverageConfig.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnAverageConfig.ForeColor = System.Drawing.Color.White;
            this.btnAverageConfig.Location = new System.Drawing.Point(174, 107);
            this.btnAverageConfig.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAverageConfig.Name = "btnAverageConfig";
            this.btnAverageConfig.Size = new System.Drawing.Size(90, 25);
            this.btnAverageConfig.TabIndex = 27;
            this.btnAverageConfig.Text = "기준에버설정";
            this.btnAverageConfig.UseVisualStyleBackColor = false;
            // 
            // MemberScoreView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1084, 561);
            this.Controls.Add(this.btnAverageConfig);
            this.Controls.Add(this.btnGradeConfig);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnlDataGrid);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MemberScoreView";
            this.Text = "IMeberScoreView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Button btnPre;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.Panel pnlDataGrid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkExclude;
        private System.Windows.Forms.TextBox txtSearchWord;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbSortType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkMatchSearch;
        private System.Windows.Forms.TextBox txtMatchTitle;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnGradeConfig;
        private System.Windows.Forms.Button btnAverageConfig;
    }
}