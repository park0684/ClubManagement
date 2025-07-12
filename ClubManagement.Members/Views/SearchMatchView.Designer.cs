
namespace ClubManagement.Members.Views
{
    partial class SearchMatchView
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
            this.btnPost = new System.Windows.Forms.Button();
            this.btnPre = new System.Windows.Forms.Button();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlDataGrid = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnPost
            // 
            this.btnPost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPost.Location = new System.Drawing.Point(262, 12);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(24, 30);
            this.btnPost.TabIndex = 23;
            this.btnPost.Text = "▷";
            this.btnPost.UseVisualStyleBackColor = true;
            // 
            // btnPre
            // 
            this.btnPre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPre.Location = new System.Drawing.Point(12, 12);
            this.btnPre.Name = "btnPre";
            this.btnPre.Size = new System.Drawing.Size(24, 30);
            this.btnPre.TabIndex = 24;
            this.btnPre.Text = "◁";
            this.btnPre.UseVisualStyleBackColor = true;
            // 
            // dtpToDate
            // 
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(157, 15);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(99, 23);
            this.dtpToDate.TabIndex = 21;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(42, 15);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(99, 23);
            this.dtpFromDate.TabIndex = 22;
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.BackColor = System.Drawing.Color.Black;
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelect.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnSelect.ForeColor = System.Drawing.Color.White;
            this.btnSelect.Location = new System.Drawing.Point(485, 12);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 30);
            this.btnSelect.TabIndex = 19;
            this.btnSelect.Text = "선택";
            this.btnSelect.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Black;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(566, 12);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // pnlDataGrid
            // 
            this.pnlDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDataGrid.Location = new System.Drawing.Point(12, 50);
            this.pnlDataGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlDataGrid.Name = "pnlDataGrid";
            this.pnlDataGrid.Size = new System.Drawing.Size(629, 236);
            this.pnlDataGrid.TabIndex = 18;
            // 
            // SearchMatchView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 299);
            this.Controls.Add(this.btnPost);
            this.Controls.Add(this.btnPre);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlDataGrid);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SearchMatchView";
            this.Text = "SearchMatchView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.Button btnPre;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlDataGrid;
    }
}