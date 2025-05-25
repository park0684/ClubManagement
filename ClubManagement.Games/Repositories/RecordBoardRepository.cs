using ClubManagement.Common.Repositories;
using ClubManagement.Games.DTOs;
using ClubManagement.Games.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ClubManagement.Games.Repositories
{
    class RecordBoardRepository : BaseRepository, IRecordBoardRepository 
    {
        public DataTable LoadGameOrder(int code)
        {
            string query = $"SELECT game_order, game_type, game_player, game_side, game_allcover FROM games WHERE game_code = {code}";
            return SqlAdapterQuery(query);
        }
        public string LoadMatchTitle(int matchCode)
        {
            string query = $"SELECT match_title FROM match WHERE match_code = {matchCode}";
            return ScalaQuery(query).ToString();
        }
        public DataTable LoadAllPalyerList(int code)
        {
            string query = $"SELECT att_name, att_memcode, att_memtype, att_gender, att_pro, att_handi, att_side, att_allcover FROM attend WHERE att_code = {code}";
            return SqlAdapterQuery(query);
        }
        public DataRow LoadGamePlayer(int match, int game, string name)
        {
            string query = $"SELECT pl_group, pl_handi ,pl_score FROM players WHERE pl_match = {match} AND pl_game = {game} AND pl_name ='{name}'";
            return SqlAdapterQuery(query).Rows[0];
        }
        public DataTable LoadGamePlayers(int match)
        {
            string query = $"SELECT pl_game ,pl_group, pl_member, pl_name,pl_handi,pl_pro,pl_gender,pl_side,pl_allcover,pl_score, pl_isallcover FROM players WHERE pl_match = {match} ORDER BY pl_game, pl_group";
            return SqlAdapterQuery(query);
        }
        public DataTable LoadIndividualSideSet(int match)
        {
            string query = $"SELECT ind_rank, ind_prize, ind_handi FROM individualset WHERE ind_match = {match}";
            return SqlAdapterQuery(query);
        }
        public DataTable LoadIndividualSideRank(int match, int game)
        {
            string query = $"SELECT is_name, is_rank, is_handi FROM individualside WHERE is_match = {match} AND is_game = {game}";
            return SqlAdapterQuery(query);
        }
        public int CheckIndividualSideRank(int match, int game)
        {
            string query = $"SELECT COUNT(*) FROM individualside WHERE is_match = {match} AND is_game = {game}";
            return Convert.ToInt32(ScalaQuery(query));
        }
        public void InsertGame(RecordBoardModel model)
        {
            DataTable paramDate = new DataTable();
            paramDate.Columns.Add("game_order", typeof(int));
            paramDate.Columns.Add("game_type", typeof(int));
            paramDate.Columns.Add("game_player", typeof(int));
            paramDate.Columns.Add("game_side", typeof(int));
            paramDate.Columns.Add("game_allcover", typeof(int));

            foreach(var game in model.GameList)
            {
                paramDate.Rows.Add(
                    game.GameSeq,
                    game.GameType,
                    game.PlayerCount,
                    game.PersonalSideGame = false,
                    game.AllCoverGame = false
                    );
            }

            using (SqlConnection connection = OpenSql())
            {
               
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlParameter[] parameters = {
                        new SqlParameter ("@match", SqlDbType.Int) { Value = model.MatchCode},
                        new SqlParameter
                        {
                            ParameterName = "@GameList",
                            SqlDbType = SqlDbType.Structured,
                            TypeName = "dbo.GameInfo",
                            Value = paramDate
                        }
                    };
                    ExecuteNoneQuery(StoredProcedures.InsertGames, connection, transaction, parameters);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
        public void InsertGamePlayer(RecordBoardModel model)
        {
            int matchCode = model.MatchCode;
            int gameOrder = model.CurrentGame;
            int group = model.CurrentGroup;

            DataTable paramTable = new DataTable();
            paramTable.Columns.Add("player_memcode", typeof(int));
            paramTable.Columns.Add("player_name", typeof(string));
            paramTable.Columns.Add("player_gender", typeof(byte));
            paramTable.Columns.Add("player_handicap", typeof(int));
            paramTable.Columns.Add("player_isPro", typeof(byte));
            paramTable.Columns.Add("player_individual", typeof(byte));
            paramTable.Columns.Add("player_allcover", typeof(byte));
            paramTable.Columns.Add("player_isAllcover", typeof(byte));
            paramTable.Columns.Add("player_score", typeof(int));

            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {  
                    var currentGame = model.GameList.FirstOrDefault(g => g.GameSeq == gameOrder);
                    var currentGroup = currentGame.Groups.FirstOrDefault(c => c.GroupNumber == group);
                    foreach (var player in currentGroup.players)
                    {
                        paramTable.Rows.Add(
                            player.MemberCode,
                            player.PlayerName,
                            player.Gender,
                            player.Handycap,
                            player.IsPro,
                            player.IndividualSide,
                            player.AllCoverSide,
                            player.IsAllCover,
                            player.Score
                            );
                    }
                    SqlParameter[] parameters =
                        {
                            new SqlParameter("@match",SqlDbType.Int){Value = matchCode},
                            new SqlParameter("@game",SqlDbType.Int){Value = gameOrder},
                            new SqlParameter("@group",SqlDbType.Int){Value = group},
                            new SqlParameter
                            {
                                ParameterName = "@PlayerList",
                                SqlDbType = SqlDbType.Structured,
                                TypeName = "dbo.PlayerInfo",
                                Value = paramTable
                            }
                           
                        };
                    ExecuteNoneQuery(StoredProcedures.InsertGamePlayer, connection, transaction, parameters);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
        public void UpdatePlayerInfo(PlayerInfoDto player, int match)
        {
            
            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@match", SqlDbType.Int){Value = match },
                        new SqlParameter("@name", SqlDbType.VarChar){Value = player.PlayerName },
                        new SqlParameter("@gender", SqlDbType.Int){Value = player.Gender },
                        new SqlParameter("@isPro", SqlDbType.Int){Value = player.IsPro },
                        new SqlParameter("@individual", SqlDbType.Int){Value = player.IndividualSide },
                        new SqlParameter("@allcover", SqlDbType.Int){Value = player.AllCoverSide },
                        new SqlParameter("@handi", SqlDbType.Int){Value = player.Handycap },

                    };
                    ExecuteNoneQuery(StoredProcedures.UpdatePlayerOption, connection, transaction, parameters);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
        public void UPdatePlayerScore(PlayerInfoDto player, int match, int game)
        {
            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@score",SqlDbType.Int){ Value = player.Score},
                        new SqlParameter("@allcover",SqlDbType.Int){ Value = player.IsAllCover == true? 1 : 0},
                        new SqlParameter("@match",SqlDbType.Int){ Value = match},
                        new SqlParameter("@game",SqlDbType.Int){ Value = game},
                        new SqlParameter("@name",SqlDbType.VarChar){ Value = player.PlayerName}
                    };
                    ExecuteNoneQuery(StoredProcedures.UpdatePlayerScore, connection, transaction, parameters);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
        public void UpdateIndividualRank(List<IndividualPlayerDto> players, int match, int game, bool reRecord)
        {
            using (SqlConnection connection = OpenSql())
            {
                DataTable paramTable = new DataTable();
                paramTable.Columns.Add("indp_name", typeof(string)); 
                paramTable.Columns.Add("indp_handi", typeof(int));
                paramTable.Columns.Add("indp_rank", typeof(int));

                foreach (var player in players)
                {
                    if(player.Rank != 0)
                        paramTable.Rows.Add(
                            player.Player,
                            player.AddHandi,
                            player.Rank);
                }
                    SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlParameter[] parameters =
                       {
                            new SqlParameter("@match", SqlDbType.Int){ Value = match},
                            new SqlParameter("@game", SqlDbType.Int){ Value = game},
                            new SqlParameter
                            {
                                ParameterName = "@IndividualRanks",
                                SqlDbType = SqlDbType.Structured,
                                TypeName = "dbo.IndividualPlayer",
                                Value = paramTable
                            }
                        };
                    ExecuteNoneQuery(StoredProcedures.InsertIndividaulRank, connection, transaction, parameters);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }

        }
        public void UpdateIndividualSet(RecordBoardModel model)
        {
            DataTable paramTable = new DataTable();
            paramTable.Columns.Add("indo_rank", typeof(int));
            paramTable.Columns.Add("indo_prize", typeof(int));
            paramTable.Columns.Add("indo_handi", typeof(int));
            foreach (var set in model.IndividaulSideSet)
            {
                DataRow row = paramTable.NewRow();
                row["indo_rank"] = set.Rank;
                row["indo_prize"] = set.Prize;
                row["indo_handi"] = set.Handi;
                paramTable.Rows.Add(row);

            }
                using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlParameter[] parameters =
                        {
                            new SqlParameter("@match", SqlDbType.Int){Value = model.MatchCode},
                            new SqlParameter
                            {
                                ParameterName = "@individualList",
                                SqlDbType = SqlDbType.Structured,
                                TypeName = "dbo.IndividualOption",
                                Value = paramTable
                            }
                        };
                    ExecuteNoneQuery(StoredProcedures.SetIndividualOption, connection, transaction, parameters);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }

            }

        }

    }
}
