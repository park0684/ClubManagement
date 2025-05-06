using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace ClubManagement.Common.Controls
{
    public class CustomRounButton : Button
    {
        public int CornerRadius { get; set; } = 8;
        public Color BorderColor { get; set; } = Color.Black;
        public int BorderThickness { get; set; } = 1;

        public CustomRounButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.FromArgb(47,107,214);
            ForeColor = Color.White;
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            Rectangle rect = ClientRectangle;

            using (GraphicsPath path = GetRoundPath(rect, CornerRadius))
            using (Brush brush = new SolidBrush(BackColor))
            {
                g.FillPath(brush, path);
                // 테두리 제거: DrawPath 생략
                TextRenderer.DrawText(
                    g,
                    Text = "Button",
                    Font,
                    rect,
                    ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );
                this.Region = new Region(path);
            }
        }

        private GraphicsPath GetRoundPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
