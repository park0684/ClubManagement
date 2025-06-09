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

        /// <summary>
        /// 폼 종료
        /// </summary>
        public void CloseForm()
        {
            this.Close();
        }

        /// <summary>
        /// 전체 플레이어 리스트와 게임 데이터를 바인딩 및 초기화
        /// </summary>
        public void SetAllPlayerList(List<GameOrderDto> games, List<PlayerInfoDto> players)
        {
            SetAllPlayerDataGirdView(games, players);
            LoadAllPlayers(players);
            LoadAllPlayerScore(games);
        }

        /// <summary>
        /// 폼을 화면 중앙에 모달로 표시
        /// </summary>
        public void ShowForm()
        {
            StartPosition = FormStartPosition.CenterScreen;
            ShowDialog();
        }

        /// <summary>
        /// 메시지 박스 표시
        /// </summary>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "알림");
        }

        /// <summary>
        /// 개인 사이드 게임 DataGridView 바인딩
        /// </summary>
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

            // 이름 라벨
            Label lblName = new Label
            {
                Text = player.PlayerName,
                Font = new Font("맑은 고딕", 18, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            pnlPlayer.Controls.Add(lblName);

            //점수 표시
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
            //클릭 시 점수 입력 이벤트 호출
            pnlPlayer.Controls.Add(lblScore);
            pnlPlayer.Click += (s, e) => EnterScoreEvent((PlayerInfoDto)pnlPlayer.Tag);
            lblName.Click += (s, e) => EnterScoreEvent((PlayerInfoDto)pnlPlayer.Tag);
            lblScore.Click += (s, e) => EnterScoreEvent((PlayerInfoDto)pnlPlayer.Tag);
            return pnlPlayer;
        }
        /// <summary>
        /// 개인전 플레이어 패널 생성
        /// </summary>
        /// <param name="player">플레이어 이름</param>
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
            //패널 생성
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

        /// <summary>
        /// 개인전 그룹별 플레이어 패널을 렌더링
        /// 그룹의 모든 플레이어를 패널로 생성하여 flpGameGroup에 추가
        /// </summary>
        /// <param name="groups">그룹 리스트</param>
        public void RenderIndividualGameGroups(List<GroupDto> groups)
        {
            // 레이아웃 중지 (성능 최적화)
            flpGameGroup.SuspendLayout();

            // 기존 컨트롤 제거
            flpGameGroup.Controls.Clear();

            // 그룹 내 모든 플레이어 패널 생성 및 추가
            foreach (var group in groups)
            {
                foreach (var player in group.players)
                {
                    var pnl = CreatePlayerPanal(player);  // 플레이어 패널 생성
                    flpGameGroup.Controls.Add(pnl);      // FlowLayoutPanel에 추가
                }
            }

            // 자동 스크롤 활성화
            flpGameGroup.AutoScroll = true;

            // 레이아웃 다시 시작
            flpGameGroup.ResumeLayout();
        }

        /// <summary>
        /// 팀전 그룹별 그룹 패널을 렌더링
        /// 각 그룹 패널을 flpGameGroup에 추가
        /// </summary>
        /// <param name="groups">그룹 리스트</param>
        /// <param name="gameSeq">게임 순번</param>
        public void RenderTeamGameGroups(List<GroupDto> groups, int gameSeq)
        {
            // 레이아웃 중지 (성능 최적화)
            flpGameGroup.SuspendLayout();

            // 기존 컨트롤 제거
            flpGameGroup.Controls.Clear();

            // 그룹 패널 생성 및 추가
            foreach (var group in groups)
            {
                var pnl = CreateGroupPanal(group, gameSeq);  // 그룹 패널 생성
                flpGameGroup.Controls.Add(pnl);              // FlowLayoutPanel에 추가
            }

            // 레이아웃 다시 시작
            flpGameGroup.ResumeLayout();
        }


        /// <summary>
        /// 이벤트 등록
        /// </summary>
        private void ViewEvent()
        {
            // 개인 사이드 게임 설정 버튼 클릭 시 이벤트 발생
            btnSideGameSet.Click += (s, e) => SetIndividualSideEvent?.Invoke(this, EventArgs.Empty);

            // 올커버 사이드 게임 설정 버튼 클릭 시 이벤트 발생
            btnAllcoverGame.Click += (s, e) => AllcoverGameSetEvent?.Invoke(this, EventArgs.Empty);

            // 개인 사이드 랭크 저장 버튼 클릭 시 이벤트 발생
            btnSaveIndividualSide.Click += (s, e) => SaveIndividualRankEvent?.Invoke(this, EventArgs.Empty);

            // 플레이어 DataGridView 셀 클릭 시 이벤트 발생
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
            dgvPlayer.dgv.Margin = new Padding(5, 0, 5, 5);
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
        /// dgvPlayer의 DataGridView에 게임별 점수 칼럼과 기본 칼럼을 생성하여 초기화.
        /// GameOrderDto 리스트를 기반으로 게임 횟수만큼 점수 칼럼을 생성.
        /// 플레이어 정보 등록을 위한 칼럼 및 설정 초기화 수행.
        /// </summary>
        /// <param name="games">게임 순서 리스트 (GameOrderDto)</param>
        /// <param name="players">플레이어 리스트 (PlayerInfoDto)</param>
        private void SetAllPlayerDataGirdView(List<GameOrderDto> games, List<PlayerInfoDto> players)
        {
            var dgv = dgvPlayer.dgv;

            // 기존 컬럼 제거 (초기화)
            dgv.Columns.Clear();

            // 기본 컬럼 추가: 회원코드, 이름, 핸디캡
            dgv.Columns.Add("memCode", "회원코드");
            dgv.Columns.Add("playerName", "이름");
            dgv.Columns.Add("handycap", "핸디캡");

            // 게임 순서에 따라 게임별 점수 컬럼 생성
            foreach (var game in games)
            {
                string columnName = $"game{game.GameSeq}";
                dgv.Columns.Add(columnName, $"{game.GameSeq}게임");
            }

            // 평균 점수 컬럼 추가
            dgv.Columns.Add("average", "에버");

            // 공통 DataGridView 설정 적용 (폰트, 스타일, 정렬 등)
            dgvPlayer.ApplyDefaultColumnSettings();

            // 데이터 바인딩 속성 설정 (AutoGenerateColumns 비활성)
            dgv.AutoGenerateColumns = false;
            dgv.Columns["memCode"].DataPropertyName = "MemberCode";
            dgv.Columns["playerName"].DataPropertyName = "PlayerName";
            dgv.Columns["handycap"].DataPropertyName = "Handycap";

            // 회원코드는 화면에 표시하지 않음
            dgv.Columns["memCode"].Visible = false;

            // 셀 가운데 정렬 및 선택 색상 지정
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.SelectionBackColor = Color.White;
        }


        /// <summary>
        /// DataGridView 셀 클릭 시 해당 플레이어의 이름을 가져와 옵션 설정 이벤트를 호출.
        /// 셀 클릭 시 유효한 행에서만 동작.
        /// </summary>
        /// <param name="sender">DataGridView 컨트롤</param>
        /// <param name="e">셀 클릭 이벤트 인자 (행/열 인덱스 정보 포함)</param>
        private void dgvPlayer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 클릭한 셀이 유효한 행(데이터가 있는 행)인지 확인
            if (e.RowIndex >= 0)
            {
                // 현재 클릭된 행에서 플레이어 이름 가져오기
                string playerName = dgvPlayer.dgv.CurrentRow.Cells["playerName"].Value.ToString();

                // 플레이어 이름이 유효하면 옵션 설정 이벤트 호출
                if (!string.IsNullOrEmpty(playerName))
                {
                    PlayerOptionEvent?.Invoke(playerName);
                }
            }
        }

        /// <summary>
        /// 개인 사이드 게임 플레이어 리스트를 DataGridView에 바인딩.
        /// 각 플레이어를 행 단위로 추가하며 초기 점수는 0으로 설정.
        /// </summary>
        /// <param name="players">사이드 게임 참가 플레이어 리스트</param>
        public void SetSideGamePlayerList(List<PlayerInfoDto> players)
        {
            var dgv = dgvIndividaulSide.dgv;

            // 기존 행 제거 (초기화)
            dgv.Rows.Clear();

            // 플레이어 정보 바인딩
            foreach (var player in players)
            {
                int newRow = dgv.Rows.Add();
                dgv.Rows[newRow].Cells["No"].Value = newRow + 1;                  // 순번
                dgv.Rows[newRow].Cells["player"].Value = player.PlayerName;       // 이름
                dgv.Rows[newRow].Cells["handycap"].Value = player.Handycap;       // 핸디캡
                dgv.Rows[newRow].Cells["score"].Value = 0;                        // 점수 초기값
            }
        }

        /// <summary>
        /// 올커버 게임 플레이어 리스트를 DataGridView에 바인딩.
        /// 이름과 순번만 표시하며 점수 및 상태는 별도 처리.
        /// </summary>
        /// <param name="players">올커버 게임 참가 플레이어 리스트</param>
        public void SetAllcoverGamePlayers(List<PlayerInfoDto> players)
        {
            var dgv = dgvAllCover.dgv;

            // 기존 행 제거 (초기화)
            dgv.Rows.Clear();

            // 플레이어 정보 바인딩
            foreach (var player in players)
            {
                int newRow = dgv.Rows.Add();
                dgv.Rows[newRow].Cells["No"].Value = newRow + 1;                  // 순번
                dgv.Rows[newRow].Cells["player"].Value = player.PlayerName;       // 이름
            }
        }

        /// <summary>
        /// 전체 플레이어 리스트를 DataGridView에 바인딩.
        /// 각 플레이어 정보를 행 단위로 추가하며, 기본 정보 (회원 코드, 이름, 핸디캡)를 표시.
        /// </summary>
        /// <param name="players">DataGridView에 바인딩할 플레이어 리스트</param>
        private void LoadAllPlayers(List<PlayerInfoDto> players)
        {
            var dgv = dgvPlayer.dgv;

            // 전달받은 플레이어 리스트를 순회하며 DataGridView에 행 추가
            foreach (var player in players)
            {
                int newRow = dgv.Rows.Add();  // 새 행 추가

                // 행의 셀 값 설정
                dgv.Rows[newRow].Cells["memCode"].Value = player.MemberCode;    // 회원 코드
                dgv.Rows[newRow].Cells["playerName"].Value = player.PlayerName; // 이름
                dgv.Rows[newRow].Cells["handycap"].Value = player.Handycap;     // 핸디캡
            }
        }

        /// <summary>
        /// 전체 플레이어의 게임별 점수를 DataGridView에 바인딩하고,
        /// 각 플레이어의 평균 점수를 계산하여 표시.
        /// </summary>
        /// <param name="games">게임 및 그룹/플레이어 점수 정보를 포함한 게임 리스트</param>
        private void LoadAllPlayerScore(List<GameOrderDto> games)
        {
            var dgv = dgvPlayer.dgv;

            // 각 게임별로 DataGridView에 점수 채우기
            foreach (var game in games)
            {
                string gameColumnName = $"game{game.GameSeq}";

                // 게임 내 그룹 순회
                foreach (var group in game.Groups)
                {
                    // 그룹 내 플레이어 순회
                    foreach (var player in group.players)
                    {
                        // DataGridView의 행 중 이름이 일치하는 플레이어 찾기
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            if (row.Cells["playerName"].Value?.ToString() == player.PlayerName)
                            {
                                // 점수 + 핸디캡 합계 (300점 제한)
                                int gameScore = player.Score == 0 ? 0 : player.Score + player.Handycap;
                                if (gameScore > 300)
                                    gameScore = 300;

                                // 해당 게임 컬럼에 점수 설정
                                row.Cells[gameColumnName].Value = gameScore;
                                break; // 플레이어 찾았으면 다음 플레이어로
                            }
                        }
                    }
                }
            }

            // 각 플레이어의 평균 점수 계산 및 설정
            int gameCount = games.Count;
            foreach (DataGridViewRow row in dgv.Rows)
            {
                int tscore = 0;    // 총 점수
                int playCount = 0; // 플레이한 게임 수

                // 각 게임 점수를 합산
                for (int i = 1; i <= gameCount; i++)
                {
                    int score = Convert.ToInt32(row.Cells[$"game{i}"].Value);
                    tscore += score;
                    if (score != 0)
                        playCount++;
                }

                // 평균 계산 (플레이한 게임이 없으면 0)
                int average = (tscore != 0 && playCount != 0) ? tscore / playCount : 0;
                row.Cells["average"].Value = average;
            }
        }
        /// <summary>
        /// 그룹별 점수를 계산하여 DataGridView에 바인딩하고,
        /// 순위를 부여하여 표시.
        /// </summary>
        /// <param name="groups">게임 및 그룹, 플레이어 점수 정보를 포함한 GameOrderDto</param>
        public void SetGroupScoreList(GameOrderDto groups)
        {
            var dgv = dgvGameScore.dgv;
            dgv.Rows.Clear(); // 기존 데이터 초기화

            // 그룹별 점수 계산 및 내림차순 정렬
            var scoreList = groups.Groups.Select(g =>
            {
                // 점수 계산: 개인전은 첫 플레이어 점수, 팀전은 합산 점수 (300점 초과 제한)
                int totalScore = groups.GameType == 1
                    ? Math.Min(g.players.FirstOrDefault()?.Score ?? 0, 300)
                    : g.players.Sum(p => Math.Min(p.Score + p.Handycap, 300));

                return new { Group = g, Score = totalScore };
            })
            .OrderByDescending(x => x.Score)
            .ToList();

            // 순위 부여: 동점 처리 포함
            int currentRank = 1;
            int sameScoreCount = 1;
            int? prevScore = null;

            foreach (var item in scoreList)
            {
                if (prevScore.HasValue && item.Score == prevScore.Value)
                {
                    // 동점: 같은 순위 유지
                    item.Group.Rank = currentRank;
                    sameScoreCount++;
                }
                else
                {
                    if (prevScore.HasValue)
                    {
                        // 점수가 다르면 이전 동점 수 만큼 순위 증가
                        currentRank += sameScoreCount;
                    }

                    item.Group.Rank = currentRank;
                    sameScoreCount = 1; // 현재 그룹 포함
                }

                prevScore = item.Score;
            }

            // DataGridView에 그룹 정보 및 순위 표시
            foreach (var group in groups.Groups)
            {
                string name;
                int score;

                if (groups.GameType == 1) // 개인전
                {
                    var player = group.players.FirstOrDefault();
                    if (player != null)
                    {
                        name = player.PlayerName;
                        score = Math.Min(player.Score + player.Handycap, 300);
                    }
                    else
                    {
                        name = "";
                        score = 0;
                    }
                }
                else // 팀전
                {
                    name = $"{group.GroupNumber}팀";
                    score = group.players.Sum(p => Math.Min(p.Score + p.Handycap, 300));
                }

                int newRow = dgv.Rows.Add();
                dgv.Rows[newRow].Cells["group"].Value = group.GroupNumber;
                dgv.Rows[newRow].Cells["name"].Value = name;
                dgv.Rows[newRow].Cells["score"].Value = score;
                dgv.Rows[newRow].Cells["rank"].Value = group.Rank;
            }
        }


        /// <summary>
        /// 올커버 플레이어를 DataGridView에 표시.
        /// - 점수 300 또는 올커버 사이드 + 올커버 여부를 만족하는 플레이어 대상.
        /// - 해당 플레이어의 "whether" 컬럼에 ★ All 표기.
        /// </summary>
        /// <param name="selectedGame">현재 게임 정보(GameOrderDto)</param>
        public void LoadAllcoverGamePlayers(GameOrderDto selectedGame)
        {
            var dgv = dgvAllCover.dgv;

            // 올커버 대상 플레이어: 300점이거나 올커버 사이드 + 올커버 체크된 경우
            var players = selectedGame.Groups
                .SelectMany(g => g.players)
                .Where(p => (p.AllCoverSide && p.IsAllCover) || p.Score == 300)
                .ToList();

            // 기존 표시 초기화
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells["whether"].Value = "";
            }

            // 올커버 플레이어 표기
            foreach (var player in players)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    // 이름 매칭 시 "whether" 컬럼 업데이트
                    if (row.Cells["player"].Value?.ToString() == player.PlayerName)
                    {
                        row.Cells["whether"].Value = player.IsAllCover ? "★ All" : "";
                        break; // 매칭된 경우 루프 종료
                    }
                }
            }
        }

        /// <summary>
        /// 개인 사이드 랭크 등록을 위한 플레이어 리스트 생성.
        /// DataGridView(dgvIndividaulSide)의 현재 표시된 데이터를 기반으로 IndividualPlayerDto 리스트 반환.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns>개인 사이드 랭크 정보가 포함된 IndividualPlayerDto 리스트</returns>
        public List<IndividualPlayerDto> SetIndividualSideRank(int rank)
        {
            List<IndividualPlayerDto> players = new List<IndividualPlayerDto>();
            var dgv = dgvIndividaulSide.dgv;
            int i = 1;

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue; // 새 행 제외

                IndividualPlayerDto player = new IndividualPlayerDto
                {
                    seq = i, // 순번
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

        /// <summary>
        /// 개인 사이드 게임 점수를 DataGridView(dgvIndividaulSide)에 바인딩.
        /// DataTable 데이터의 playerName과 DataGridView의 player 컬럼을 매칭하여 값 갱신.
        /// </summary>
        /// <param name="players">개인 점수/핸디/랭크 데이터</param>
        public void BindingIndividualScore(DataTable players)
        {
            foreach (DataRow player in players.Rows)
            {
                string playerName = player["playerName"].ToString();

                foreach (DataGridViewRow row in dgvIndividaulSide.dgv.Rows)
                {
                    if (row.Cells["player"].Value.ToString() == playerName)
                    {
                        row.Cells["handycap"].Value = player["handi"];
                        row.Cells["score"].Value = player["totalScore"];
                        row.Cells["rank"].Value = player["rank"];
                        break; // 매칭 완료되면 루프 종료
                    }
                }
            }
        }

        /// <summary>
        /// 사용자에게 확인 메시지 표시 후 Yes/No 여부 반환.
        /// </summary>
        /// <param name="message">표시할 메시지</param>
        /// <returns>Yes 선택 시 true, No 선택 시 false</returns>
        public bool ShowConfirmation(string message)
        {
            return MessageBox.Show(message, "확인", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }
    }
}
