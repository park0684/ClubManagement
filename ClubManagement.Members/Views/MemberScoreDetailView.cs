using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubManagement.Common.Controls;
using ClubManagement.Common.DTOs;
using System.Windows.Forms.DataVisualization.Charting;

namespace ClubManagement.Members.Views
{
    public partial class MemberScoreDetailView : Form,IMemberScoreDetailView
    {
        public MemberScoreDetailView()
        {
            InitializeComponent();
            InitializeLabel();
            ViewEvent();

            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        public Decimal Average 
        { 
            set 
            { 
                lblAverage.Text = value.ToString();
                lblAverage.ForeColor = value >= 200 ? Color.Red : Color.Black; 
            }  
        }
        public Decimal AverageScore 
        {
            set
            {
                lblAvergeScore.Text = value.ToString();
                lblAvergeScore.ForeColor = value >= 200 ? Color.Red : Color.Black;
            }
        }
        public int MaxScore 
        {
            set 
            { 
                lblMaxScore.Text = value.ToString();
                lblMaxScore.ForeColor = value >= 200 ? Color.Red : Color.Black;
            } 
        }
        public int MinScore 
        {
            set
            { 
                lblMinScore.Text = value.ToString();
                lblMinScore.ForeColor = value >= 200 ? Color.Red : Color.Black;
            }
        }
        public int GameCount { set => lblGameCount.Text = $"({value})게임"; }
        public string MemberName { set => lblMemberName.Text = value.ToString(); }
        public string MemberGender { set => lblGender.Text = value.ToString(); }
        public int MemberHandi { set => lblHandi.Text = value.ToString(); }
        public int MemberGrade 
        { 
            get => ((KeyValuePair<int, string>)cmbGrade.SelectedItem).Key;
            set => cmbGrade.SelectedValue = value;
        }
        
        public event EventHandler CloseFormEvent;
        public event Action<int> ScoreSearchEvent;
        public event EventHandler GradeSaveEvent;

        public void CloseForm()
        {
            this.Close();
        }

        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void SetComboBoxItems(Dictionary<int, string> items)
        {
            cmbGrade.DataSource = items.ToList();
            cmbGrade.DisplayMember = "Value";
            cmbGrade.ValueMember = "Key";
            cmbGrade.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void CreateGameScoreList(List<ScoreDto> scores)
        {
            flpMatchList.AutoScroll = true;
            flpMatchList.Controls.Clear();
            foreach(var score in scores )
            {
                var scorePnl = new CustomScorePanel();
                scorePnl.CreatePanel(score);
                scorePnl.Margin = new Padding(3);
                flpMatchList.Controls.Add(scorePnl);
            }
        }

        private void ViewEvent()
        {
            btnclose.Click += (s, e) => CloseFormEvent?.Invoke(this, EventArgs.Empty);
            btnSave.Click += (s, e) => GradeSaveEvent?.Invoke(this, EventArgs.Empty);
            foreach (var lbl in tlpInterval.Controls.OfType<Label>())
            {
                if (lbl.Tag is ValueTuple<bool, int>)
                {
                    lbl.Click += Label_Click;
                    SetLabelUnselectedStyle(lbl); // 초기 스타일
                }
            }
        }

        /// <summary>
        /// 라벨 태그값 초기화
        /// </summary>
        private void InitializeLabel()
        {
            lblOneMonth.Tag = (isSelected: false, interval: 1);
            lblThreeMonth.Tag = (isSelected: true, interval: 3);
            lblSixMonth.Tag = (isSelected: false, interval: 6);
            lblOneYear.Tag = (isSelected: false, interval: 12);
            lblAll.Tag = (isSelected: false, interval: 0);
            SetLabelSelectedStyle(lblThreeMonth);
        }

        /// <summary>
        /// 라벨 클릭 시 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_Click(object sender, EventArgs e)
        {
            if (sender is Label clicked && clicked.Tag is ValueTuple<bool, int> tag && !tag.Item1)
            {
                foreach (var lbl in tlpInterval.Controls.OfType<Label>())
                {
                    if (lbl.Tag is ValueTuple<bool, int> t)
                    {
                        bool isMatch = lbl == clicked;
                        lbl.Tag = (isMatch, t.Item2); // 유지: 대소문자 Interval

                        if (isMatch)
                        {
                            SetLabelSelectedStyle(lbl);
                        }
                            
                        else
                            SetLabelUnselectedStyle(lbl);
                    }
                }

                ScoreSearchEvent?.Invoke(tag.Item2);
            }
        }

        /// <summary>
        /// 라벨 클릭시 변경
        /// </summary>
        /// <param name="lbl"></param>
        private void SetLabelSelectedStyle(Label lbl)
        {
            lbl.Font = new Font(lbl.Font.FontFamily, 12, FontStyle.Bold);
            lbl.ForeColor = Color.Blue;
        }

        /// <summary>
        /// 다른 라벨 클릭 시 원상태로 변경
        /// </summary>
        /// <param name="lbl"></param>
        private void SetLabelUnselectedStyle(Label lbl)
        {
            lbl.Font = new Font(lbl.Font.FontFamily, 9, FontStyle.Regular);
            lbl.ForeColor = Color.Black;
        }

        private void DrowGraph(List<ScoreDto> scores)
        {
            // 차트 생성
            var chart = new Chart();
            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.WhiteSmoke; //차트 배경색

            //그래프 가로,세로축 생성
            var chartArea = new ChartArea("MainArea");
            chartArea.BackColor = Color.WhiteSmoke; // 그래프내 배경색
            chartArea.AxisX.MajorGrid.Enabled = false;  
            chart.ChartAreas.Add(chartArea);

            //데이터를 선형 그래프로 표시
            var series = new Series("Average")
            {
                ChartType = SeriesChartType.Spline,
                BorderWidth = 1,
                Color = Color.Blue,
                IsValueShownAsLabel = false 
            };

            foreach (var score in scores)
            {
                int colorThreshold = 200;
                int pointIndex = series.Points.AddXY(score.GameDate.ToString("MM/dd"), score.GameAverage);

                var point = series.Points[pointIndex];
                point.Color = score.GameAverage >= colorThreshold ? Color.FromArgb(80, Color.Red) : Color.FromArgb(80, Color.Blue);
                point.MarkerStyle = MarkerStyle.Circle;
                point.MarkerSize = 3;
            }

            chart.Series.Add(series);

            // 색상으로 면 채우기 (붉은색/파란색은 곡선 자체에 적용됨)
            //series.BackSecondaryColor = Color.LightGray;
            //series.BackGradientStyle = GradientStyle.TopBottom;
            //series.BackHatchStyle = ChartHatchStyle.None;

            // 축 설정 - 가장자리 여백 제거
            chartArea.AxisX.LabelStyle.Font = new Font("맑은 고딕", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("맑은 고딕", 8);
            chartArea.AxisX.IsMarginVisible = false;
            chartArea.AxisX.LineDashStyle = ChartDashStyle.Dash;

            // 세로축 최대, 최소 설정
            var minY = Math.Floor(scores.Min(s => (double)s.GameAverage) / 10) * 10;
            var maxY = Math.Ceiling(scores.Max(s => (double)s.GameAverage) / 10) * 10;
            chartArea.AxisY.Minimum = minY;
            chartArea.AxisY.Maximum = maxY;

            pnlGraph.Controls.Clear();
            pnlGraph.Controls.Add(chart);

        }

        public void SetGraph(List<ScoreDto> scores)
        {
            DrowGraph(scores);
        }
    }

}
