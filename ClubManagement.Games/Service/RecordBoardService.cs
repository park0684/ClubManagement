using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;

namespace ClubManagement.Games.Service
{
    public class RecordBoardService
    {
        IRecordBoardRepository _repository;

        public RecordBoardService(IRecordBoardRepository repositories)
        {
            _repository = repositories;
            
        }
        public RecordBoardModel LoadRecodeDate(int match, int game)
        {
            var model = new RecordBoardModel
            {
                MatchCode = match,
                MatchTitle = _repository.LoadMatchTitle(match).ToString(),
                CurrentGame = game,
                CurrentGroup = 0,
                GameList = LoadGames(match),
                PlayerList = LoadAttendees(match),
                IndividaulSideSet = LoadIndividualSideSet(match)
            };
            LoadGamePlayer(match, model);
            return model;
        }
        /// <summary>
        /// games 테이블에서 정보 수신
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public List<GameOrderDto> LoadGames(int match)
        {
            var result = new List<GameOrderDto>();
            var dataTable = _repository.LoadGameOrder(match);
            foreach(DataRow row in dataTable.Rows)
            {
                var gameOrder = new GameOrderDto
                {
                    GameSeq = Convert.ToInt32(row["game_order"]),
                    GameType = Convert.ToInt32(row["game_type"]),
                    PlayerCount = Convert.ToInt32(row["game_player"]),
                    Groups = Enumerable.Range(1, Convert.ToInt32(row["game_player"])).Select(i => new GroupDto { GroupNumber = i }).ToList()
                };
                gameOrder.IndividualPlayers = LoadIndividualRak(match, gameOrder.GameSeq);
                result.Add(gameOrder);
            }
            return result;
        }

        /// <summary>
        /// attend 테이블에 모임 참석자 조회
        /// 각 게임별이 아닌 전체 참석자로 핸디 및 사이드 게임 설정 적용
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public List<PlayerInfoDto> LoadAttendees(int match)
        {
            var result = new List<PlayerInfoDto>();
            var dataTable = _repository.LoadAllPalyerList(match);

            foreach(DataRow row in dataTable.Rows)
            {
                bool isFemale = Convert.ToInt32(row["att_gender"]) == 1;
                bool isPro = Convert.ToInt32(row["att_pro"]) == 1;
                int handicap = Convert.ToInt32(row["att_handi"]);

                PlayerInfoDto player = new PlayerInfoDto
                {
                    PlayerName = row["att_name"].ToString(),
                    MemberCode = Convert.ToInt32(row["att_memcode"]),
                    Gender = isFemale,
                    IsPro = isPro,
                    Handycap = handicap,
                    IndividualSide = Convert.ToInt32(row["att_side"]) == 1,
                    AllCoverSide = Convert.ToInt32(row["att_allcover"]) == 1
                };
                result.Add(player);
            }

            return result;
        }

        /// <summary>
        /// 각 게임별 플레이어정보 수신
        /// </summary>
        /// <param name="match"></param>
        /// <param name="model"></param>
        private void LoadGamePlayer(int match, RecordBoardModel model)
        {
            var dataTable = _repository.LoadGamePlayers(match);

            foreach(DataRow row in dataTable.Rows)
            {
                var player = new PlayerInfoDto
                {
                    PlayerName = row["att_name"].ToString(),
                    MemberCode = Convert.ToInt32(row["att_memcode"]),
                    Handycap = Convert.ToInt32(row["att_handi"]),
                    IsPro = row["att_pro"].ToString() == "1",
                    Gender = row["att_gender"].ToString() == "1",
                    IndividualSide = row["att_side"].ToString() == "1",
                    AllCoverSide = row["att_allcover"].ToString() == "1",
                    Score = Convert.ToInt32(row["pl_score"]),
                    IsSelected = true,
                    IsAllCover = row["pl_isallcover"].ToString() == "1"
                };

                int gameSeq = Convert.ToInt32(row["pl_game"]);
                int groupNum = Convert.ToInt32(row["pl_group"]);
                var game = model.GameList.FirstOrDefault(g => g.GameSeq == gameSeq);
                var group = game?.Groups.FirstOrDefault(g => g.GroupNumber == groupNum);
                group?.players.Add(player);
            }
        }

        /// <summary>
        /// 사이드게임 개인전 설정값 조회
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private List<IndividaulSetDto> LoadIndividualSideSet(int match)
        {
            var dataTable = _repository.LoadIndividualSideSet(match);
            var result = new List<IndividaulSetDto>();

            foreach(DataRow row in dataTable.Rows)
            {
                var individualset = new IndividaulSetDto
                {
                    Rank = Convert.ToInt32(row["ind_rank"]),
                    Prize = Convert.ToInt32(row["ind_prize"]),
                    Handi = Convert.ToInt32(row["ind_handi"])
                };
                result.Add(individualset);
            }
            return result;
        }


        /// <summary>
        /// 지정된 모임과 게임 코드에 대한 개인 사이드 랭크 데이터를 로드.
        /// DataTable을 IndividualPlayerDto 리스트로 변환하여 반환.
        /// </summary>
        /// <param name="match">모임 코드</param>
        /// <param name="game">게임 순번</param>
        /// <returns>IndividualPlayerDto 리스트</returns>
        private List<IndividualPlayerDto> LoadIndividualRak(int match, int game)
        {
            var result = new List<IndividualPlayerDto>();

            // DB에서 개인 사이드 랭크 데이터를 DataTable 형태로 로드
            var dataTable = _repository.LoadIndividualSideRank(match, game);

            // DataTable의 각 Row를 DTO로 변환하여 리스트에 추가
            foreach (DataRow row in dataTable.Rows)
            {
                var individualRank = new IndividualPlayerDto
                {
                    Player = row["is_name"].ToString(),          // 플레이어 이름
                    Rank = Convert.ToInt32(row["is_rank"]),     // 순위
                    AddHandi = Convert.ToInt32(row["is_handi"]) // 추가 핸디캡
                };

                result.Add(individualRank);
            }

            return result;
        }


        /// <summary>
        /// 지정된 모임 코드의 플레이어 옵션 정보를 DB에 업데이트.
        /// </summary>
        /// <param name="player">업데이트할 플레이어 정보 DTO</param>
        /// <param name="match">모임 코드</param>
        public void UpdatePlayerOption(PlayerInfoDto player, int match)
        {
            // 리포지토리 메서드를 호출하여 플레이어 정보 DB 업데이트
            _repository.UpdatePlayerInfo(player, match);
        }


        /// <summary>
        /// 현재 게임 이전까지의 게임 데이터를 기반으로  
        /// 플레이어별 누적 추가 핸디캡을 계산하여 반환.
        /// </summary>
        /// <param name="games">게임 순서 리스트</param>
        /// <param name="currentGame">현재 게임 순번</param>
        /// <returns>플레이어별 누적 핸디캡 리스트</returns>
        public List<IndividualPlayerDto> SetIndividualHandi(List<GameOrderDto> games, int currentGame)
        {
            List<IndividualPlayerDto> players = new List<IndividualPlayerDto>();

            // 현재 게임 이전 게임들만 순회
            foreach (var game in games)
            {
                if (game.GameSeq >= currentGame) continue;

                // 각 게임의 개별 플레이어 데이터 순회
                foreach (var player in game.IndividualPlayers)
                {
                    // 이미 리스트에 있는 플레이어인지 확인
                    var exist = players.FirstOrDefault(p => p.Player == player.Player);

                    if (exist == null)
                    {
                        // 최초 추가 시 새 DTO 생성
                        players.Add(new IndividualPlayerDto
                        {
                            Player = player.Player,
                            AddHandi = player.AddHandi
                        });
                    }
                    else
                    {
                        // 이미 존재하면 누적 핸디캡 가산
                        exist.AddHandi += player.AddHandi;
                    }
                }
            }

            // 최종 플레이어 누적 핸디 리스트 반환
            return players.ToList();
        }


        /// <summary>
        /// 현재 게임의 개인 사이드 점수표를 생성.
        /// 누적 핸디캡을 고려한 총점 및 순위를 포함한 DataTable 반환.
        /// </summary>
        /// <param name="games">게임 순서 목록</param>
        /// <param name="currentGame">현재 게임 순번</param>
        /// <returns>개인 점수표 DataTable</returns>
        public DataTable IndividualScore(List<GameOrderDto> games, int currentGame)
        {
            DataTable result = new DataTable();
            result.Columns.Add("playerName", typeof(string));  // 플레이어 이름
            result.Columns.Add("score", typeof(int));         // 실제 점수
            result.Columns.Add("handi", typeof(int));         // 누적 핸디캡
            result.Columns.Add("totalScore", typeof(int));    // 총점 (핸디 반영)
            result.Columns.Add("rank", typeof(int));          // 순위

            // 현재 게임 이전까지 누적된 핸디캡 계산
            var rankerHandi = SetIndividualHandi(games, currentGame);

            // 현재 게임 데이터 가져오기
            var selectedGame = games.First(g => g.GameSeq == currentGame);

            // 현재 게임의 개인 사이드 참가자 추출
            var players = selectedGame.Groups
                .SelectMany(g => g.players)
                .Where(p => p.IndividualSide)
                .ToList();

            // 각 플레이어에 대해 누적 핸디캡, 총점 계산
            var sortPlayers = players.Select(p =>
            {
                int addHandi = 0;
                var exist = rankerHandi.FirstOrDefault(x => x.Player == p.PlayerName);
                if (rankerHandi.Count != 0 && exist != null)
                {
                    addHandi = exist.AddHandi;
                }

                // 총점 = 실제점수 + 게임 핸디 - 누적핸디 (최대 300 제한)
                int total = Math.Min(300, p.Score + p.Handycap - addHandi);

                return new
                {
                    Player = p.PlayerName,
                    Score = p.Score,
                    AddHandi = addHandi,
                    TotalScore = total
                };
            })
            .OrderByDescending(p => p.TotalScore)
            .ToList();

            // 순위 계산 (동점자 처리 포함)
            int rank = 1, sameScoreCount = 1;
            int? prevScore = null;

            foreach (var player in sortPlayers)
            {
                DataRow row = result.NewRow();
                row["playerName"] = player.Player;
                row["score"] = player.Score;
                row["handi"] = player.AddHandi;
                row["totalScore"] = player.TotalScore;

                if (prevScore.HasValue && player.TotalScore == prevScore.Value)
                {
                    // 이전 점수와 같으면 같은 순위
                    row["rank"] = rank;
                    sameScoreCount++;
                }
                else
                {
                    if (prevScore.HasValue)
                        rank += sameScoreCount;

                    row["rank"] = rank;
                    sameScoreCount = 1;
                }

                prevScore = player.TotalScore;
                result.Rows.Add(row);
            }

            return result;
        }
        /// <summary>
        /// RecordBoardModel을 기반으로 게임 데이터를 DB에 저장.
        /// </summary>
        /// <param name="model">기록판 데이터 모델</param>
        public void InsertGames(RecordBoardModel model)
        {
            // 리포지토리를 통해 게임 데이터 삽입
            _repository.InsertGame(model);
        }

        /// <summary>
        /// RecordBoardModel을 기반으로 게임 플레이어 데이터를 DB에 저장.
        /// </summary>
        /// <param name="model">기록판 데이터 모델</param>
        public void InsertGamePlayer(RecordBoardModel model)
        {
            // 리포지토리를 통해 게임 플레이어 데이터 삽입
            _repository.InsertGamePlayer(model);
        }

    }
}
