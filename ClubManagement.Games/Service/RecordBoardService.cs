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
                int handicap = (isFemale ? 15 : 0) + (isPro ? -5 : 0);

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
                    PlayerName = row["pl_name"].ToString(),
                    MemberCode = Convert.ToInt32(row["pl_member"]),
                    Handycap = Convert.ToInt32(row["pl_handi"]),
                    IsPro = row["pl_pro"].ToString() == "1",
                    Gender = row["pl_gender"].ToString() == "1",
                    IndividualSide = row["pl_side"].ToString() == "1",
                    AllCoverSide = row["pl_allcover"].ToString() == "1",
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



        private List<IndividualPlayerDto> LoadIndividualRak(int match, int game)
        {
            var result = new List<IndividualPlayerDto>();
            var dataTable = _repository.LoadIndividualSideRank(match, game);
            foreach(DataRow row in dataTable.Rows)
            {
                var individualRank = new IndividualPlayerDto
                {
                    Player = row["is_name"].ToString(),
                    Rank = Convert.ToInt32(row["is_rank"]),
                    AddHandi = Convert.ToInt32(row["is_handi"])
                };
                result.Add(individualRank);
            }
            return result;
        }

        public void UpdatePlayerOption(PlayerInfoDto player, int match)
        {
            _repository.UpdatePlayerInfo(player, match);
        }
        
        public List<IndividualPlayerDto> SetIndividualHandi(List<GameOrderDto> games, int currentGame)
        {
            List<IndividualPlayerDto> players = new List<IndividualPlayerDto>();

            foreach(var game in games)
            {
                if (game.GameSeq >= currentGame) continue;

                foreach (var player in game.IndividualPlayers)
                {
                    var exist = players.FirstOrDefault(p => p.Player == player.Player);
                    if (exist == null)
                    {
                        players.Add( new IndividualPlayerDto
                        {
                            Player = player.Player,
                            AddHandi = player.AddHandi
                        });
                    }
                    else
                    {
                        exist.AddHandi += player.AddHandi;
                    }
                }
            }
            return players.ToList();
        }

        public DataTable IndividualScore(List<GameOrderDto> games, int currentGame)
        {
            DataTable result = new DataTable();
            result.Columns.Add("playerName", typeof(string));
            result.Columns.Add("score", typeof(int));
            result.Columns.Add("handi", typeof(int));
            result.Columns.Add("totalScore", typeof(int));
            result.Columns.Add("rank", typeof(int));

            var rankerHandi = SetIndividualHandi(games, currentGame);
            var selectedGame = games.First(g => g.GameSeq == currentGame);
            var players = selectedGame.Groups
                .SelectMany(g => g.players)
                .Where(p => p.IndividualSide)
                .ToList();

            var sortPlayers = players.Select(p =>
            {
                int addHandi = 0;
                var exist = rankerHandi.FirstOrDefault(x => x.Player == p.PlayerName);
                if ( rankerHandi.Count != 0 && exist != null)
                {
                    
                    addHandi = exist.AddHandi;
                }
                    

                int total = Math.Min(300, p.Score + p.Handycap - addHandi);

                return new
                {
                    Player = p.PlayerName,
                    Score = p.Score,
                    AddHandi = addHandi,
                    TotalScore = total
                };
            }).OrderByDescending(p => p.TotalScore).ToList();

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
        public void InsertGames(RecordBoardModel model)
        {
            _repository.InsertGame(model);
        }
        public void InsertGamePlayer(RecordBoardModel model)
        {
            _repository.InsertGamePlayer(model);
        }
        
    }
}
