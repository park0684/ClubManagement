using ClubManagement.Common.DTOs;
using System.Drawing;
using System.Windows.Forms;

namespace ClubManagement.Common.Controls
{
    public partial class CustomScorePanel : UserControl
    {
        public CustomScorePanel()
        {
            InitializeComponent();
        }

        public void CreatePanel(ScoreDto score)
        {
            lblTitel.Text = score.GameTitle;
            lblDate.Text = score.GameDate.ToString("M");
            lblTotalScore.Text = score.TotalScore.ToString();
            lblAverage.Text = score.GameAverage.ToString("0##.#");
            lblAverage.ForeColor = score.GameAverage >= 200 ? Color.Red : Color.Black;
            int row = 0;
            int column = 0;
            foreach(var gScore in score.GameScore)
            {
                
                
                if (column >= 6)
                {
                    column = 0;
                    row++;
                    tlpScore.RowCount++;
                    tlpScore.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
                    this.Height = this.Height + 25;
                }
                Label lbl = new Label
                {
                    Text = gScore.ToString(),
                    Font = new Font("맑은 고딕", 12),
                    ForeColor = gScore >= 200 ? Color.Red : Color.Black,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Dock = DockStyle.Fill
                };

                tlpScore.Controls.Add(lbl, column, row);
                column++;
            };
        }
    }
}
