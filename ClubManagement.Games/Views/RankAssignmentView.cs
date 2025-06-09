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

        /// <summary>
        /// 닫기 버튼 이벤트 바인딩
        /// </summary>
        private void ViewEvent()
        {
            btnClose.Click += (s, e) => CloseEvent?.Invoke(this, EventArgs.Empty);
            btnSave.Click += (s, e) => SaveEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 개별 플레이어 패널 생성 및 flpPlayerList에 추가
        /// </summary>
        /// <param name="player">플레이어 정보</param>
        private void CreatePlayerPanel(IndividualPlayerDto player)
        {
            // 패널 생성
            Panel pnlPlayer = new Panel
            {
                Width = 360,
                Height = 60,
                BorderStyle = BorderStyle.None,
                Margin = new Padding(10)
            };

            // 순위 버튼 생성
            Button btnRank = new Button
            {
                Name = $"btnRank_{player.seq}",
                Font = new Font("맑은 고딕", 12),
                Size = new Size(50, 50),
                Location = new Point(5, 5),
                Margin = new Padding(5),
                Text = player.Rank >= 1 && player.Rank <= 3 ? player.Rank.ToString() : "",
                Tag = player,
                BackColor = Color.White
            };
            player.Rank = string.IsNullOrEmpty(btnRank.Text) ? 0 : Convert.ToInt32(btnRank.Text);
            btnRank.Click += BtnRank_Click;
            ApplyRankColor(btnRank, player.Rank);
            pnlPlayer.Controls.Add(btnRank);

            // 이름 라벨
            Label lblPlayer = new Label
            {
                Text = player.Player,
                Font = new Font("맑은 고딕", 14, FontStyle.Bold),
                Size = new Size(69, 25),
                Location = new Point(63, 17),
                AutoSize = true
            };
            pnlPlayer.Controls.Add(lblPlayer);

            // 점수 + 핸디 표시
            StringBuilder score = new StringBuilder();
            if (player.Handi != 0)
                score.Append($"{player.Score + player.Handi} / {player.Score}");
            else
                score.Append(player.Score.ToString());

            Label lblScore = new Label
            {
                Text = score.ToString(),
                Font = new Font("맑은 고딕", 14, FontStyle.Bold),
                Location = new Point(159, 17),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleRight,
                ForeColor = player.Score > 199 ? Color.Red : Color.Black
            };
            pnlPlayer.Controls.Add(lblScore);

            // 기본 핸디
            int defaultHandi = IndividaulSets.FirstOrDefault(x => x.Rank == player.Rank)?.Handi ?? 0;
            player.AddHandi = defaultHandi;

            // 핸디 버튼
            Button btnHandi = new Button
            {
                Name = $"btnHandi_{player.seq}",
                Text = defaultHandi.ToString(),
                Tag = player,
                Font = new Font("맑은 고딕", 12),
                Location = new Point(266, 5),
                Size = new Size(89, 50),
                Margin = new Padding(5)
            };
            btnHandi.Click += (s, e) => EditHandiEvent?.Invoke(this, new HandiEditEventArgs { SenderButton = (Button)s });
            pnlPlayer.Controls.Add(btnHandi);

            // 패널 추가
            flpPlayerList.Controls.Add(pnlPlayer);
        }

        /// <summary>
        /// 전체 플레이어 패널 생성
        /// </summary>
        public void AddPlayerPanel()
        {
            flpPlayerList.Controls.Clear();
            foreach (var player in IndividaulRankers)
                CreatePlayerPanel(player);
        }

        /// <summary>
        /// 순위에 따라 버튼 색상 지정
        /// </summary>
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

        /// <summary>
        /// 순위 버튼 클릭 시 컨텍스트 메뉴 표시
        /// </summary>
        private void BtnRank_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var player = btn.Tag as IndividualPlayerDto;
            if (player == null || IndividaulSets == null || IndividaulRankers == null)
                return;

            int maxRank = IndividaulSets.Max(x => x.Rank);
            Font menuFont = new Font("맑은 고딕", 14);
            ContextMenuStrip menu = new ContextMenuStrip();

            // 선택 해제 메뉴
            ToolStripMenuItem clearItem = new ToolStripMenuItem("선택 해제") { Font = menuFont };
            clearItem.Click += (s, ev) =>
            {
                player.Rank = 0;
                btn.Text = "";
                ApplyRankColor(btn, 0);
                EidtPlyaerRank(player.seq, player.Rank);
            };
            menu.Items.Add(clearItem);
            menu.Items.Add(new ToolStripSeparator());

            // 순위 메뉴 항목 생성
            for (int i = 1; i <= maxRank; i++)
            {
                bool isLastRank = i == maxRank;
                bool alreadyTaken = IndividaulRankers.Any(p => p != player && p.Rank == i);
                if (alreadyTaken && !isLastRank)
                    continue;

                int selectedRank = i;
                ToolStripMenuItem item = new ToolStripMenuItem($"{selectedRank}등") { Font = menuFont };
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
        /// IndividaulRankers에서 플레이어 순위, 핸디 갱신
        /// </summary>
        private void EidtPlyaerRank(int seq, int rank)
        {
            var player = IndividaulRankers.FirstOrDefault(p => p.seq == seq);
            int maxRank = IndividaulSets.Max(s => s.Rank);
            player.Rank = rank;
            player.AddHandi = rank == 0 || rank > maxRank ? 0 : IndividaulSets.FirstOrDefault(s => s.Rank == rank).Handi;
            UpdateHandiButton(player);
        }

        /// <summary>
        /// 핸디 버튼의 핸디 값을 갱신
        /// </summary>
        private void UpdateHandiButton(IndividualPlayerDto player)
        {
            string handiName = $"btnHandi_{player.seq}";
            Control[] handiControls = flpPlayerList.Controls.Find(handiName, true);
            if (handiControls.Length > 0 && handiControls[0] is Button btnHandi)
            {
                int defaultHandi = IndividaulSets.FirstOrDefault(x => x.Rank == player.Rank)?.Handi ?? 0;
                player.AddHandi = defaultHandi;
                btnHandi.Text = defaultHandi.ToString();
                btnHandi.Tag = player;
            }
        }

        /// <summary>
        /// 폼 닫기
        /// </summary>
        public void CloseForm()
        {
            this.Close();
        }

        /// <summary>
        /// 폼 중앙 표시
        /// </summary>
        public void ShowForm()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowDialog();
        }

        /// <summary>
        /// 알림 메시지 표시
        /// </summary>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        /// <summary>
        /// 확인 메시지 (Yes/No)
        /// </summary>
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
