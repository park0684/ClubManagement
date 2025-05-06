using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public partial class RankAssignmentView : Form,IRankAssignmentView
    {
   
        public RankAssignmentView()
        {
            InitializeComponent();
            ViewEvent();
        }

        public List<IndividualPlayerDto> IndividaulRankers { get; set; } = new List<IndividualPlayerDto>();

        public List<IndividaulSetDto> IndividaulSets { get; set; } = new List<IndividaulSetDto>();


        public event EventHandler SaveEvent;
        public event EventHandler CloseEvent;
        public event EventHandler<HandiEditEventArgs> EditHandiEvent;
        public event EventHandler EidtRankEvent;

        private void ViewEvent()
        {
            btnClose.Click += (s, e) => CloseEvent?.Invoke(this, EventArgs.Empty);
            btnSave.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
        }
        private void CreatePlayerPanel(IndividualPlayerDto player)
        {
            Panel pnlPlayer = new Panel 
            {
                Width = 360,
                Height = 60,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10)
            };

            Button btnRank = new Button
            {
                Name = $"btnRank_{player.seq}",
                Font = new Font("맑은 고딕", 12),
                Size = new Size(50,50),
                Location = new Point(5, 5),
                Margin = new Padding(5),
                Text = player.Rank >= 1 && player.Rank <= 3 ? player.Rank.ToString() : "",
                Tag = player,
                BackColor = Color.White
            };
            player.Rank = btnRank.Text == "" ? 0 : Convert.ToInt32(btnRank.Text);
            btnRank.Tag = player;
            btnRank.Click += BtnRank_Click;
            pnlPlayer.Controls.Add(btnRank);
            ApplyRankColor(btnRank, player.Rank);
            Label lblPlayer = new Label
            {
                Text = player.Player,
                Font = new Font("맑은 고딕", 14, FontStyle.Bold),
                Size = new Size(69, 25),
                Location = new Point(63, 17),
                AutoSize = true
            };
            pnlPlayer.Controls.Add(lblPlayer);

            StringBuilder score = new StringBuilder(player.Score.ToString());
            if (player.Handi != 0)
                score.Append(" / " + (player.Score - player.Handi));
            string _score = score.ToString();
            Label lblScore = new Label
            {
                Text = score.ToString(),
                Font = new Font("맑은 고딕", 14, FontStyle.Bold),
                Location = new Point(159, 17),
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = true,
                ForeColor = Convert.ToInt32(score.ToString()) > 199 ? Color.Red : Color.Black
            };
            pnlPlayer.Controls.Add(lblScore);

            int defaultHandi = IndividaulSets.FirstOrDefault(x => x.Rank == player.Rank)?.Handi ?? 0;
            player.AddHandi = defaultHandi;

            Button btnHandi = new Button
            {
                Name = $"btnHandi_{player.seq}",
                Text = defaultHandi.ToString(),
                Tag = player,
                Font = new Font("맑은 고딕", 12),
                Location = new Point(266, 5),
                Margin = new Padding(5),
                Size = new Size(89,50)
            };
            btnHandi.Click += (s, e) => EditHandiEvent?.Invoke(this, new HandiEditEventArgs
            {
                SenderButton = (Button)s
            });


            pnlPlayer.Controls.Add(btnHandi);
            flpPlayerList.Controls.Add(pnlPlayer);
        }

        public void AddPlayerPanel( )
        {
            //IndividaulPlayers = players;
            //IndividaulSets = individaulSet;

            flpPlayerList.Controls.Clear();
            foreach (var player in IndividaulRankers)
                CreatePlayerPanel(player);
        }
        private void ApplyRankColor(Button btn, int rank)
        {
            switch (rank)
            {
                case 1:
                    btn.BackColor = Color.Gold;
                    break;
                case 2:
                    btn.BackColor = Color.Silver;
                    break;
                case 3:
                    btn.BackColor = Color.SaddleBrown;
                    break;
                default:
                    btn.BackColor = Color.White;
                    break;
            }
        }
        private void BtnRank_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var player = btn.Tag as IndividualPlayerDto;

            if (player == null || IndividaulSets == null || IndividaulRankers == null)
                return;

            int maxRank = IndividaulSets.Max(x => x.Rank); // 가장 마지막 순위 = 중복 허용 대상
            Font menuFont = new Font("맑은 고딕", 14);      // 메뉴 항목 글꼴

            ContextMenuStrip menu = new ContextMenuStrip();

            // 선택 해제 항목
            ToolStripMenuItem clearItem = new ToolStripMenuItem("선택 해제");
            clearItem.Font = menuFont;
            clearItem.Click += (s, ev) =>
            {
                player.Rank = 0;
                btn.Text = "";
                ApplyRankColor(btn, 0);
                EidtPlyaerRank(player.seq, player.Rank);
            };
            menu.Items.Add(clearItem);
            menu.Items.Add(new ToolStripSeparator());

            // 순위 항목 생성
            for (int i = 1; i <= maxRank; i++)
            {
                bool isLastRank = i == maxRank;
                bool alreadyTaken = IndividaulRankers.Any(p => p != player && p.Rank == i);

                // 마지막 순위만 중복 허용
                if (alreadyTaken && !isLastRank)
                    continue;

                int selectedRank = i; // 클로저 고정용 지역 변수
                ToolStripMenuItem item = new ToolStripMenuItem($"{selectedRank}등");
                item.Font = menuFont;
                item.Click += (s, ev) =>
                {
                    player.Rank = selectedRank;
                    btn.Text = selectedRank.ToString();
                    ApplyRankColor(btn, selectedRank);
                    EidtPlyaerRank(player.seq, player.Rank);
                };
                menu.Items.Add(item);
            }

            menu.Show(btn, new Point(0, btn.Height));
            
            
        }
        /// <summary>
        /// IndividaulRankers에 수정된 랭크 정보 등록
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rank"></param>
        private void EidtPlyaerRank(int seq, int rank)
        {
            var player = IndividaulRankers.FirstOrDefault(p => p.seq == seq);
            int maxRank = IndividaulSets.Max(s => s.Rank);
            player.Rank = rank;
            player.AddHandi = rank == 0 || rank > maxRank ? 0 : IndividaulSets.FirstOrDefault(s => s.Rank == rank).Handi;
            UpdateHandiButton(player);
        }
        private void UpdateHandiButton(IndividualPlayerDto player)
        {
            
            string handiName = $"btnHandi_{player.seq}";


            // 핸디 텍스트 갱신
            Control[] handiControls = flpPlayerList.Controls.Find(handiName, true);
            if (handiControls.Length > 0 && handiControls[0] is Button btnHandi)
            {
                int defaultHandi = IndividaulSets.FirstOrDefault(x => x.Rank == player.Rank)?.Handi ?? 0;
                player.AddHandi = defaultHandi;

                btnHandi.Text = defaultHandi.ToString();
                btnHandi.Tag = player;
            }
        }
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
        public bool ShowConfirmation(string message)
        {
            return MessageBox.Show(message, "확인", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }
    }

    public class HandiEditEventArgs : EventArgs
    {
        public Button SenderButton { get; set; }
    }
}
