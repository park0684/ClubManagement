using ClubManagement.Common.Controls;
using ClubManagement.Games.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClubManagement.Games.Views
{
    public partial class RecordBoardView : Form, IRecordBoardView
    {
        CustomDataGridViewControl dgvPlayer;
        CustomDataGridViewControl dgvIndividaulSide;
        CustomDataGridViewControl dgvAllCover;
        CustomDataGridViewControl dgvGameScore;
        public RecordBoardView()
        {
            InitializeComponent();
            InitializeDataGridView();
            ViewEvent();
            this.Text = "기록";
        }

        public string MatchTitle
        {
            set { lblTitle.Text = value; }
        }
        public string GameSeq 
        { 
            set { grpScore.Text = value; }
        }
        

        public event Action<int> GameButtonClick;
        public event Action<int,int> AssignPlayerClick;
        public event EventHandler SetIndividualSideEvent;
        public event EventHandler AllcoverGameSetEvent;
        public event Action<string> PlayerOptionEvent;
        public event Action<PlayerInfoDto> EnterScoreEvent;
        public event EventHandler SaveIndividualRankEvent;

        public void CloseForm()
        {
            this.Close();
        }

        public void SetAllPlayerList(List<GameOrderDto> games,List<PlayerInfoDto> players)
        {
            SetAllPlayerDataGirdView(games,players);
            LoadAllPlayers(players);
            LoadAllPlayerScore(games);
        }

        public void ShowForm()
        {
            StartPosition = FormStartPosition.CenterScreen;
            ShowDialog();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "알림");
        }
        public void SetBindingSideGame(List<PlayerInfoDto> players)
        {
            var dgv = dgvIndividaulSide.dgv;

            BindingList<PlayerInfoDto> bindingList = new BindingList<PlayerInfoDto>(players);
            dgv.DataSource = bindingList;
        }
        /// <summary>
        /// 게임버튼 생성
        /// GameOrder 리스트내 순서대로 게임 버튼을 생성
        /// </summary>
        /// <param name="games"></param>
        public void CreateGameButton(List<GameOrderDto> games)
        {
            flpGameButton.Controls.Clear();
            foreach(var game in games)
            {
                int gameSea = game.GameSeq;
                Button btnGame = new Button
                {
                    Width = 75,
                    Height = 30,
                    BackColor = Color.FromArgb(0,111,246),
                    ForeColor = Color.White,
                    Text = $"{gameSea}게임",
                    Tag = gameSea
                };
                btnGame.Click += (s, e) => GameButtonClick?.Invoke(gameSea);
                btnGame.FlatStyle = FlatStyle.Flat;
                flpGameButton.Controls.Add(btnGame);
                
            }
        }
        /// <summary>
        /// 게임 선택에 따른 flpGameGroup 패널 초기화
        /// </summary>
        public void flpGameGroupClear()
        {
            flpGameGroup.Controls.Clear();
        }

        /// <summary>
        /// 그룹내 참가 플레이어 패널 생성
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private Panel CreatePlayerPanal(PlayerInfoDto player)
        {
            Panel pnlPlayer = new Panel
            {
                Width = 250,
                Height = 45,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
                Tag = player
            };

            Label lblName = new Label
            {
                Text = player.PlayerName,
                Font = new Font("맑은 고딕", 18, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            pnlPlayer.Controls.Add(lblName);

            StringBuilder score = new StringBuilder(player.Score.ToString());
            if (player.Handycap != 0)
            {
                score.Append("/" + (player.Score + player.Handycap > 300 ? 300 : player.Score + player.Handycap).ToString());
            }
            if(player.IsAllCover)
            {
                score.Append(" ALL");
            }
            if (player.Score == 300)
                score.Append(" Perfect");

            Label lblScore = new Label
            {
                Text = score.ToString(),
                Font = new Font("맑은 고딕", 18),
                Location = new Point(90, 10),
                AutoSize = true,
                ForeColor = player.Score  + player.Handycap >= 200 ? Color.Red : Color.Black
            };
        
            pnlPlayer.Controls.Add(lblScore);
            pnlPlayer.Click += (s, e) => EnterScoreEvent((PlayerInfoDto)pnlPlayer.Tag);
            lblName.Click += (s, e) => EnterScoreEvent((PlayerInfoDto)pnlPlayer.Tag);
            lblScore.Click += (s, e) => EnterScoreEvent((PlayerInfoDto)pnlPlayer.Tag);
            return pnlPlayer;
        }
        /// <summary>
        /// 개인전 플레이어 패널 생성
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayerPanal(PlayerInfoDto player)
        {
            var pnlPlayer =  CreatePlayerPanal(player);
            flpGameGroup.Controls.Add(pnlPlayer);
        }

        /// <summary>
        /// 그룹 패널 생성
        /// </summary>
        /// <param name="group"></param>
        /// <param name="gameSeq"></param>
        private Panel CreateGroupPanal(GroupDto group, int gameSeq)
        {
            Panel pnlGroup = new Panel
            {
                Width = 280,
                Height = 350,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(10),
            };

            // 팀 번호 Label
            Label lblGroupName = new Label
            {
                Text = $"{group.GroupNumber}팀",
                Font = new Font("맑은 고딕", 18, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            pnlGroup.Controls.Add(lblGroupName);

            group.Score = group.players.Sum(s => Math.Min(s.Score + s.Handycap, 300));

            // 합계 점수 Label
            Label lblScore = new Label
            {
                Text = $"합계: {group.Score}",
                Font = new Font("맑은 고딕", 18),
                Location = new Point(60, 10),
                AutoSize = true
            };
            pnlGroup.Controls.Add(lblScore);

            // 참가자 추가 버튼
            Button btnAddPlayer = new Button
            {
                Text = "추가",
                Width = 70,
                Height = 35,
                Location = new Point(200, 10),
                BackColor = Color.FromArgb(0, 111, 246),
                ForeColor = Color.White

            };
            btnAddPlayer.FlatStyle = FlatStyle.Flat;
            btnAddPlayer.FlatAppearance.BorderSize = 0;
            btnAddPlayer.Click += (s, e) => AssignPlayerClick(gameSeq, group.GroupNumber);
            pnlGroup.Controls.Add(btnAddPlayer);
            int startY = 50; // 플레이어 패널 시작 위치

            foreach (var player in group.players)
            {
                var pnlPlayer = CreatePlayerPanal(player); // 기존 CreatePlayerPanal 메소드 사용
                pnlPlayer.Location = new Point(10, startY); // 위치 지정 (패널 내)
                pnlPlayer.Width = pnlGroup.Width - 20;      // 그룹 패널 너비에 맞춤
                pnlGroup.Controls.Add(pnlPlayer);

                startY += pnlPlayer.Height + 5; // 다음 플레이어 패널 위치
            }

            return pnlGroup;
            // 그룹 패널에 추가 -> 반복 생성 메소드에서 처리로 이관
            //flpGameGroup.Controls.Add(pnlGroup);
        }

        public void RenderIndividualGameGroups(List<GroupDto> groups)
        {
            flpGameGroup.SuspendLayout();
            flpGameGroup.Controls.Clear();
            foreach (var group in groups)
            {
                foreach (var player in group.players)
                {
                    var pnl = CreatePlayerPanal(player);
                    flpGameGroup.Controls.Add(pnl);
                }
            }
            flpGameGroup.AutoScroll = true;
            flpGameGroup.ResumeLayout();
        }

        public void RenderTeamGameGroups(List<GroupDto> groups, int gameSeq)
        {
            flpGameGroup.SuspendLayout();
            flpGameGroup.Controls.Clear();
            foreach (var group in groups)
            {
                var pnl = CreateGroupPanal(group, gameSeq);
                flpGameGroup.Controls.Add(pnl);
            }
            flpGameGroup.ResumeLayout();
        }

        /// <summary>
        /// 이벤트 등록
        /// </summary>
        private void ViewEvent()
        {
            btnSideGameSet.Click += (s, e) => SetIndividualSideEvent?.Invoke(this, EventArgs.Empty);
            btnAllcoverGame.Click += (s, e) => AllcoverGameSetEvent?.Invoke(this, EventArgs.Empty);
            btnSaveIndividualSide.Click += (s, e) => SaveIndividualRankEvent?.Invoke(this, EventArgs.Empty);
            dgvPlayer.dgv.CellClick += dgvPlayer_CellClick;
        }

        /// <summary>
        /// DataGridView 기본 설정 설정
        /// dgvPlayerList는 게임 횟수 정보가 필요하므로 나중에 등록
        /// </summary>
        private void InitializeDataGridView()
        {
            //전체 플레이어 리스트
            dgvPlayer = new CustomDataGridViewControl();
            pnlPlayerDataGird.Controls.Add(dgvPlayer.dgv);
            dgvPlayer.dgv.Dock = DockStyle.Fill;
            dgvPlayer.dgv.RowTemplate.Height = 35;
            dgvPlayer.dgv.DefaultCellStyle.Font = new Font("맑은 고딕", 12);
            //개인 사인드 플레이어 리스트
            dgvIndividaulSide = new CustomDataGridViewControl();
            var sideGame = dgvIndividaulSide.dgv;
            pnlSideGame.Controls.Add(sideGame);
            sideGame.Dock = DockStyle.Fill;
            sideGame.Columns.Add("player", "이름");
            sideGame.Columns.Add("handycap", "핸디");
            sideGame.Columns.Add("score", "점수");
            sideGame.Columns.Add("rank", "순위");
            sideGame.RowTemplate.Height = 45;
            dgvIndividaulSide.ApplyDefaultColumnSettings();
            sideGame.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            sideGame.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            sideGame.ReadOnly = true;
            sideGame.AutoGenerateColumns = false;
            sideGame.Columns["player"].DataPropertyName = "PlayerName";
            sideGame.Columns["handycap"].DataPropertyName = "Handycap";
            sideGame.DefaultCellStyle.SelectionBackColor = Color.White;
            sideGame.DefaultCellStyle.Font = new Font("맑은 고딕", 12);

            //올커버 사이드 플레이어 리스트
            dgvAllCover = new CustomDataGridViewControl();
            var allcoverGame = dgvAllCover.dgv;
            pnlAllcoverGame.Controls.Add(allcoverGame);
            allcoverGame.Dock = DockStyle.Fill;
            allcoverGame.Columns.Add("player", "이름");
            allcoverGame.Columns.Add("whether", "올커버");
            allcoverGame.RowTemplate.Height = 45;
            dgvAllCover.ApplyDefaultColumnSettings();
            allcoverGame.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            allcoverGame.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            allcoverGame.ReadOnly = true;
            allcoverGame.AutoGenerateColumns = false;
            allcoverGame.Columns["player"].DataPropertyName = "PlayerName";
            allcoverGame.DefaultCellStyle.SelectionBackColor = Color.White;
            allcoverGame.DefaultCellStyle.Font = new Font("맑은 고딕", 12);

            //게임 그룹별 스코어 리스트
            dgvGameScore = new CustomDataGridViewControl();
            var gameScore = dgvGameScore.dgv;
            pnlGameRecord.Controls.Add(gameScore);
            gameScore.Dock = DockStyle.Fill;
            gameScore.Columns.Add("group", "그룹번호");
            gameScore.Columns.Add("name", "참가자/팀");
            gameScore.Columns.Add("score", "점수");
            gameScore.Columns.Add("rank", "순위");
            gameScore.RowTemplate.Height = 45;
            gameScore.Columns.Remove("No");
            gameScore.Columns["group"].Visible = false;
            gameScore.ReadOnly = true;
            gameScore.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gameScore.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvGameScore.ApplyDefaultColumnSettings();
            gameScore.DefaultCellStyle.SelectionBackColor = Color.White;
            gameScore.DefaultCellStyle.Font = new Font("맑은 고딕", 16);
        }
        /// <summary>
        /// dgvPlayer의 경우 등록된 게임수 만큼 게임별 점수 칼럼이 생성되어야 하므로 
        /// 기본 설정 후 GameOrderDto 리스트와 PlayerInfoDto리스트를 수신 받은 후 칼럼 추가 및 플레이어를 등록
        /// </summary>
        /// <param name="games"></param>
        /// <param name="players"></param>
        private void SetAllPlayerDataGirdView(List<GameOrderDto> games, List<PlayerInfoDto> players)
        {
            var dgv = dgvPlayer.dgv;
            dgv.Columns.Clear();
            dgv.Columns.Add("memCode","회원코드");
            dgv.Columns.Add("playerName", "이름");
            dgv.Columns.Add("handycap", "핸디캡");
            foreach(var game in games)
            {
                string columnName = $"game{game.GameSeq}";
                dgv.Columns.Add($"{columnName}", $"{game.GameSeq}게임");
            }
            dgv.Columns.Add("average", "에버");
            dgvPlayer.ApplyDefaultColumnSettings();
            dgv.AutoGenerateColumns = false;
            dgv.Columns["memCode"].DataPropertyName = "MemberCode";
            dgv.Columns["playerName"].DataPropertyName = "PlayerName";
            dgv.Columns["handycap"].DataPropertyName = "Handycap";
            dgv.Columns["memCode"].Visible = false;
            
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.SelectionBackColor = Color.White;
        }

        /// <summary>
        /// 데이터 그리드 셀을 클릭 할 경우 플레이어의 옵션 설정 할 수 있게 이벤트 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPlayer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                //클릭 한 플레이어의 이름을 가지고와 이벤트 실행
                string playerName = dgvPlayer.dgv.CurrentRow.Cells["playerName"].Value.ToString();
                if(!string.IsNullOrEmpty(playerName))
                {
                    PlayerOptionEvent?.Invoke(playerName);
                }

            }
        }

        /// <summary>
        /// 사이드 게임 리스트 DataGridView 데이터 바인딩
        /// </summary>
        /// <param name="players"></param>
        public void SetSideGamePlayerList(List<PlayerInfoDto> players)
        {
            var dgv = dgvIndividaulSide.dgv;
            dgv.Rows.Clear();
            foreach(var player in players)
            {
                int newRow = dgv.Rows.Add();
                dgv.Rows[newRow].Cells["No"].Value = newRow + 1;
                dgv.Rows[newRow].Cells["player"].Value = player.PlayerName;
                dgv.Rows[newRow].Cells["handycap"].Value = player.Handycap;
                dgv.Rows[newRow].Cells["score"].Value = 0;
            }
        }
        public void SetAllcoverGamePlayers(List<PlayerInfoDto> players)
        {
            var dgv = dgvAllCover.dgv;
            dgv.Rows.Clear();
            foreach(var player in players)
            {
                int newRow = dgv.Rows.Add();
                dgv.Rows[newRow].Cells["No"].Value = newRow + 1;
                dgv.Rows[newRow].Cells["player"].Value = player.PlayerName;
            }
        }
       
        /// <summary>
        /// 전체 플레이어 리스트 데이터 바인딩
        /// </summary>
        /// <param name="players"></param>
        private void LoadAllPlayers(List<PlayerInfoDto> players)
        {
            var dgv = dgvPlayer.dgv;
            foreach (var player in players)
            {
                int newRow = dgv.Rows.Add();
                dgv.Rows[newRow].Cells["memCode"].Value = player.MemberCode;
                dgv.Rows[newRow].Cells["playerName"].Value = player.PlayerName;
                dgv.Rows[newRow].Cells["handycap"].Value = player.Handycap;
                
            }

        }
        private void LoadAllPlayerScore(List<GameOrderDto> games)
        {
            var dgv = dgvPlayer.dgv;
            
            foreach (var game in games)
            {
                string gameColumnName = $"game{game.GameSeq}";

                foreach (var group in game.Groups)
                {
                    foreach (var player in group.players)
                    {
                        // 플레이어 이름 기준으로 Row 찾기
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            if (row.Cells["playerName"].Value?.ToString() == player.PlayerName)
                            {
                                // 점수 제한: 300점 초과 불가
                                int gameScore = player.Score == 0 ? 0 : player.Score + player.Handycap;
                                if (gameScore > 300)
                                    gameScore = 300;

                                row.Cells[gameColumnName].Value = gameScore;
                                break;
                            }
                        }
                    }
                }
            }
            int gameCount = games.Count;
            int playCount = 0;
            int tscore = 0;
            int average = 0;
            foreach(DataGridViewRow row in dgv.Rows)
            {
                for(int i=1; i <= gameCount; i++)
                {
                    int score = Convert.ToInt32(row.Cells[$"game{i}"].Value);
                    tscore += score;
                    if (score != 0)
                        playCount++;
                }
                average = tscore != 0 && playCount != 0 ? tscore / playCount : 0;
                row.Cells["average"].Value = average;
                tscore = 0;
                playCount = 0;
            }
        }
        public void SetGroupScoreList(GameOrderDto groups)
        {
            var dgv = dgvGameScore.dgv;
            dgv.Rows.Clear();
            var scoreList = groups.Groups.Select(g =>
            {
                int totalScore = groups.GameType == 1
                    ? Math.Min(g.players.FirstOrDefault()?.Score ?? 0,300)
                    : g.players.Sum(p => Math.Min(p.Score + p.Handycap , 300));

                return new { Group = g, Score = totalScore };
            }).OrderByDescending(x => x.Score).ToList();

            // 순위 부여
            int currentRank = 1;
            int sameScoreCount = 1;
            int? prevScore = null;

            foreach (var item in scoreList)
            {
                if (prevScore.HasValue && item.Score == prevScore.Value)
                {
                    item.Group.Rank = currentRank; // 동점이면 현재 순위 그대로
                    sameScoreCount++;
                }
                else
                {
                    if (prevScore.HasValue)
                    {
                        currentRank += sameScoreCount;
                    }

                    item.Group.Rank = currentRank;
                    sameScoreCount = 1; // 현재 아이템 포함
                }

                prevScore = item.Score;
            }

            string name = string.Empty;
            int score = 0;
            foreach(var group in groups.Groups )
            {
                if(groups.GameType == 1)
                {
                    var player = group.players.FirstOrDefault(); // 안전하게 가져오기
                    if (player != null)
                    {
                        name = player.PlayerName;
                        score = player.Score + player.Handycap > 300 ? 300 : player.Score + player.Handycap;
                    }
                }
                else
                {
                    score = 0;
                    name = group.GroupNumber.ToString() + "팀";
                    foreach(var player in group.players)
                    {
                        score += player.Score + player.Handycap > 300 ? 300 : player.Score + player.Handycap;
                    }
                }
                int newRow = dgv.Rows.Add();
                dgv.Rows[newRow].Cells["group"].Value = group.GroupNumber;
                dgv.Rows[newRow].Cells["name"].Value = name;
                dgv.Rows[newRow].Cells["score"].Value = score;
                dgv.Rows[newRow].Cells["rank"].Value = group.Rank;
            }
            ;
            //SideScoreList(groups);
        }
        
        /// <summary>
        /// 올커버 플레이어 표시
        /// 점수입력시 클릭된 올커버 여부로 확인
        /// </summary>
        /// <param name="selectedGame"></param>
        public void LoadAllcoverGamePlayers(GameOrderDto selectedGame)
        {
            
            var players = selectedGame.Groups.SelectMany(g => g.players).Where(p => p.AllCoverSide && p.IsAllCover).ToList();
            var dgv = dgvAllCover.dgv;
            foreach(DataGridViewRow row in dgv.Rows)
            {
                row.Cells["whether"].Value = "";
            }
            foreach(var player in players)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Cells["player"].Value?.ToString() == player.PlayerName)
                    {
                        row.Cells["whether"].Value = player.IsAllCover ? "★ All" : "";
                        break;
                    }
                }
            }
            
        }
        /// <summary>
        /// 개인 사이드 랭크 등록을 위한 플레이어 리스트
        /// </summary>
        /// <param name="players"></param>
        public List<IndividualPlayerDto> SetIndividualSideRank(int rank)
        {
            List<IndividualPlayerDto> players = new List<IndividualPlayerDto>();
            var dav = dgvIndividaulSide.dgv;
            int i = 1;
            foreach(DataGridViewRow row in dav.Rows)
            {
                IndividualPlayerDto player = new IndividualPlayerDto
                {
                    seq = i,
                    Player = row.Cells["player"].Value.ToString(),
                    Score = Convert.ToInt32(row.Cells["score"].Value),
                    Handi = Convert.ToInt32(row.Cells["handycap"].Value),
                    Rank = Convert.ToInt32(row.Cells["rank"].Value)
                };
                players.Add(player);
                i++;
            }
            return players;
        }

        public void BindingIndividualScore(DataTable players)
        {
            foreach(DataRow player in players.Rows)
            {
                foreach (DataGridViewRow row in dgvIndividaulSide.dgv.Rows)
                {
                    if (row.Cells["player"].Value.ToString() == player["playerName"].ToString())
                    {
                        row.Cells["handycap"].Value = player["handi"];
                        row.Cells["score"].Value = player["totalScore"];
                        row.Cells["rank"].Value = player["rank"];
                    }
                }
            }
        }
        public bool ShowConfirmation(string message)
        {
            return MessageBox.Show(message, "확인", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }
    }
}
