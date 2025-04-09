
namespace ClubManagement.Games.Views
{
    partial class ScoreBoardRegistView
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.flpOrderList = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOrderAdd = new System.Windows.Forms.Button();
            this.btnEditPlayer = new System.Windows.Forms.Button();
            this.lblCounter = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMatchTitle = new System.Windows.Forms.Label();
            this.btnGameSearch = new System.Windows.Forms.Button();
            this.flpPlayerList = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(112)))), ((int)(((byte)(247)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(598, 557);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 30);
            this.btnSave.TabIndex = 29;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(112)))), ((int)(((byte)(247)))));
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(688, 557);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(84, 30);
            this.btnClose.TabIndex = 30;
            this.btnClose.Text = "닫기";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // flpOrderList
            // 
            this.flpOrderList.Location = new System.Drawing.Point(9, 54);
            this.flpOrderList.Name = "flpOrderList";
            this.flpOrderList.Size = new System.Drawing.Size(444, 432);
            this.flpOrderList.TabIndex = 28;
            // 
            // btnOrderAdd
            // 
            this.btnOrderAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(112)))), ((int)(((byte)(247)))));
            this.btnOrderAdd.FlatAppearance.BorderSize = 0;
            this.btnOrderAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOrderAdd.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnOrderAdd.ForeColor = System.Drawing.Color.White;
            this.btnOrderAdd.Location = new System.Drawing.Point(378, 16);
            this.btnOrderAdd.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnOrderAdd.Name = "btnOrderAdd";
            this.btnOrderAdd.Size = new System.Drawing.Size(75, 30);
            this.btnOrderAdd.TabIndex = 27;
            this.btnOrderAdd.Text = "게임추가";
            this.btnOrderAdd.UseVisualStyleBackColor = false;
            // 
            // btnEditPlayer
            // 
            this.btnEditPlayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(112)))), ((int)(((byte)(247)))));
            this.btnEditPlayer.FlatAppearance.BorderSize = 0;
            this.btnEditPlayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditPlayer.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnEditPlayer.ForeColor = System.Drawing.Color.White;
            this.btnEditPlayer.Location = new System.Drawing.Point(203, 16);
            this.btnEditPlayer.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnEditPlayer.Name = "btnEditPlayer";
            this.btnEditPlayer.Size = new System.Drawing.Size(85, 30);
            this.btnEditPlayer.TabIndex = 26;
            this.btnEditPlayer.Text = "참가자 수정";
            this.btnEditPlayer.UseVisualStyleBackColor = false;
            // 
            // lblCounter
            // 
            this.lblCounter.AutoSize = true;
            this.lblCounter.Location = new System.Drawing.Point(66, 24);
            this.lblCounter.Name = "lblCounter";
            this.lblCounter.Size = new System.Drawing.Size(14, 15);
            this.lblCounter.TabIndex = 23;
            this.lblCounter.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 15);
            this.label1.TabIndex = 24;
            this.label1.Text = "게임 수 :";
            // 
            // lblMatchTitle
            // 
            this.lblMatchTitle.AutoSize = true;
            this.lblMatchTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMatchTitle.Location = new System.Drawing.Point(94, 21);
            this.lblMatchTitle.Name = "lblMatchTitle";
            this.lblMatchTitle.Size = new System.Drawing.Size(74, 21);
            this.lblMatchTitle.TabIndex = 22;
            this.lblMatchTitle.Text = "모임이름";
            // 
            // btnGameSearch
            // 
            this.btnGameSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(126)))), ((int)(((byte)(112)))), ((int)(((byte)(247)))));
            this.btnGameSearch.FlatAppearance.BorderSize = 0;
            this.btnGameSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGameSearch.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.btnGameSearch.ForeColor = System.Drawing.Color.White;
            this.btnGameSearch.Location = new System.Drawing.Point(13, 16);
            this.btnGameSearch.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnGameSearch.Name = "btnGameSearch";
            this.btnGameSearch.Size = new System.Drawing.Size(75, 30);
            this.btnGameSearch.TabIndex = 21;
            this.btnGameSearch.Text = "모임 선택";
            this.btnGameSearch.UseVisualStyleBackColor = false;
            // 
            // flpPlayerList
            // 
            this.flpPlayerList.Location = new System.Drawing.Point(7, 54);
            this.flpPlayerList.Name = "flpPlayerList";
            this.flpPlayerList.Size = new System.Drawing.Size(281, 432);
            this.flpPlayerList.TabIndex = 31;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnEditPlayer);
            this.groupBox1.Controls.Add(this.flpPlayerList);
            this.groupBox1.Location = new System.Drawing.Point(478, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 492);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.lblCounter);
            this.groupBox2.Controls.Add(this.flpOrderList);
            this.groupBox2.Controls.Add(this.btnOrderAdd);
            this.groupBox2.Location = new System.Drawing.Point(13, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(459, 492);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            // 
            // ScoreBoardRegistView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 601);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblMatchTitle);
            this.Controls.Add(this.btnGameSearch);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ScoreBoardRegistView";
            this.Text = "ScoreBoardRegistView";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.FlowLayoutPanel flpOrderList;
        private System.Windows.Forms.Button btnOrderAdd;
        private System.Windows.Forms.Button btnEditPlayer;
        private System.Windows.Forms.Label lblCounter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMatchTitle;
        private System.Windows.Forms.Button btnGameSearch;
        private System.Windows.Forms.FlowLayoutPanel flpPlayerList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}