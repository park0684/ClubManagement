
namespace ClubManagement.Games.Views
{
    partial class ScoreboardListView
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
            this.btnGameListEdit = new System.Windows.Forms.Button();
            this.btnAddGame = new System.Windows.Forms.Button();
            this.pnlDataGrid = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnPost
            // 
            this.btnPost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPost.Location = new System.Drawing.Point(262, 12);
            this.btnPost.Name = "btnPost";
            this.btnPost.Size = new System.Drawing.Size(24, 30);
            this.btnPost.TabIndex = 9;
            this.btnPost.Text = "▷";
            this.btnPost.UseVisualStyleBackColor = true;
            // 
            // btnPre
            // 
            this.btnPre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPre.Location = new System.Drawing.Point(12, 12);
            this.btnPre.Name = "btnPre";
            this.btnPre.Size = new System.Drawing.Size(24, 30);
            this.btnPre.TabIndex = 10;
            this.btnPre.Text = "◁";
            this.btnPre.UseVisualStyleBackColor = true;
            // 
            // dtpToDate
            // 
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(157, 15);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(99, 23);
            this.dtpToDate.TabIndex = 7;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(42, 15);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(99, 23);
            this.dtpFromDate.TabIndex = 8;
            // 
            // btnGameListEdit
            // 
            this.btnGameListEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGameListEdit.BackColor = System.Drawing.Color.Black;
            this.btnGameListEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGameListEdit.ForeColor = System.Drawing.Color.White;
            this.btnGameListEdit.Location = new System.Drawing.Point(551, 12);
            this.btnGameListEdit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGameListEdit.Name = "btnGameListEdit";
            this.btnGameListEdit.Size = new System.Drawing.Size(75, 30);
            this.btnGameListEdit.TabIndex = 5;
            this.btnGameListEdit.Text = "수정";
            this.btnGameListEdit.UseVisualStyleBackColor = false;
            // 
            // btnAddGame
            // 
            this.btnAddGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddGame.BackColor = System.Drawing.Color.Black;
            this.btnAddGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddGame.ForeColor = System.Drawing.Color.White;
            this.btnAddGame.Location = new System.Drawing.Point(632, 12);
            this.btnAddGame.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAddGame.Name = "btnAddGame";
            this.btnAddGame.Size = new System.Drawing.Size(75, 30);
            this.btnAddGame.TabIndex = 6;
            this.btnAddGame.Text = "게임 등록";
            this.btnAddGame.UseVisualStyleBackColor = false;
            // 
            // pnlDataGrid
            // 
            this.pnlDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDataGrid.Location = new System.Drawing.Point(12, 50);
            this.pnlDataGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlDataGrid.Name = "pnlDataGrid";
            this.pnlDataGrid.Size = new System.Drawing.Size(695, 464);
            this.pnlDataGrid.TabIndex = 4;
            // 
            // ScoreboardListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(719, 527);
            this.Controls.Add(this.btnPost);
            this.Controls.Add(this.btnPre);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.btnGameListEdit);
            this.Controls.Add(this.btnAddGame);
            this.Controls.Add(this.pnlDataGrid);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ScoreboardListView";
            this.Text = "ScoreboardListView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPost;
        private System.Windows.Forms.Button btnPre;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Button btnGameListEdit;
        private System.Windows.Forms.Button btnAddGame;
        private System.Windows.Forms.Panel pnlDataGrid;
    }
}