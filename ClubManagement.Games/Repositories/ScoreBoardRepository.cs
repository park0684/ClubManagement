using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ClubManagement.Common.Repositories;
using ClubManagement.Games.Models;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Repositories
{
    class ScoreBoardRepository : BaseRepository, IScoreBoardRepository 
    {
        public void InsertGame(ScoreBoardModel model)
        {
            int matchCode = model.MatchCode;
            string query = $"SELECT COUNT(game_order) FROM games WHERE game_code = {matchCode} ";
            
            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    int counter = Convert.ToInt32(ScalaQuery(query));
                    if (counter > 0)
                    {
                        query = $"DELETE FROM games WHERE game_code = @code";
                        SqlParameter[] deleteParam = { new SqlParameter("@code", SqlDbType.Int) { Value = matchCode } };
                        ExecuteNonQuery(query, connection, transaction, deleteParam);
                    }
                    query = "INSERT INTO games (game_code, game_order, game_type, game_player, game_side, game_allcover) VALUES(@code, @order, @type, @player, @side, @allcover)";
                    foreach (var game in model.GameList)
                    {
                        SqlParameter[] parameters =
                        {
                            new SqlParameter("@code",SqlDbType.Int){Value = matchCode},
                            new SqlParameter("@order",SqlDbType.Int){Value = game.GameSeq},
                            new SqlParameter("@type",SqlDbType.Int){Value = game.GameType},
                            new SqlParameter("@player",SqlDbType.Int){Value = game.PlayerCount},
                            new SqlParameter("@side",SqlDbType.Int){Value = game.PersonalSideGame},
                            new SqlParameter("@allcover",SqlDbType.Int){Value = game.AllCoverGame}
                        };
                        ExecuteNonQuery(query, connection, transaction, parameters);
                    }
                    query = $"UPDATE match SET match_scoreboard = 1 WHERE match_code = @code";
                    SqlParameter[] updateParam = { new SqlParameter("@code", SqlDbType.Int) { Value = matchCode } };
                    ExecuteNonQuery(query, connection, transaction, updateParam);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }

            
        }

        public void InsertGamePlayer(ScoreBoardModel model)
        {
            int matchCode = model.MatchCode;
            int gameOrder = model.CurrentGame;
            int group = model.CurrentGroup;
            string query = $"SELECT COUNT(pl_match) FROM players WHERE pl_match = {matchCode} AND pl_game = {gameOrder} AND pl_group = {group}";
            
            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    int counter = Convert.ToInt32(ScalaQuery(query));
                    if (counter > 0)
                    {
                        query = $"DELETE FROM players WHERE pl_match = @code AND pl_game =@game AND pl_group = @group";
                        SqlParameter[] deleteParam =
                        { 
                            new SqlParameter("@code", SqlDbType.Int) { Value = matchCode },
                            new SqlParameter("@game", SqlDbType.Int) { Value = gameOrder },
                            new SqlParameter("@group",SqlDbType.Int) { Value =group}
                        };
                        ExecuteNonQuery(query, connection, transaction, deleteParam);
                    }
                    var currentGame = model.GameList.FirstOrDefault(g => g.GameSeq == gameOrder);
                    var currentGroup = currentGame.Groups.FirstOrDefault(c => c.GroupNumber == group);
                    query = "INSERT INTo players(pl_match, pl_game, pl_group, pl_member,pl_name,pl_handi,pl_pro,pl_gender, pl_side, pl_allcover,pl_score) " +
                        "VALUES(@match,@game,@group, @member, @name, @handi, @pro, @gender, @side, @allcover, 0)";
                    foreach (var player in currentGroup.players)
                    {

                        SqlParameter[] parameters =
                        {
                            new SqlParameter("@match",SqlDbType.Int){Value = matchCode},
                            new SqlParameter("@game",SqlDbType.Int){Value = gameOrder},
                            new SqlParameter("@group",SqlDbType.Int){Value = group},
                            //new SqlParameter("@seq",SqlDbType.Int){Value = player.GameType},
                            new SqlParameter("@member",SqlDbType.Int){Value = player.MemberCode},
                            new SqlParameter("@name",SqlDbType.VarChar){Value = player.PlayerName},
                            new SqlParameter("@handi",SqlDbType.Int){Value = player.Handycap},
                            new SqlParameter("@pro",SqlDbType.TinyInt){Value = player.IsPro},
                            new SqlParameter("@gender",SqlDbType.TinyInt){Value = player.Gender},
                            //new SqlParameter("@newbie",SqlDbType.TinyInt){Value = player.new},
                            new SqlParameter("@side",SqlDbType.TinyInt){Value = player.IndividualSide},
                            new SqlParameter("@allcover",SqlDbType.TinyInt){Value = player.AllCoverSide}
                        };
                        ExecuteNonQuery(query, connection, transaction, parameters);
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public DataTable LoadGameOrder(int code)
        {
            string query = $"SELECT game_order, game_type, game_player, game_side, game_allcover FROM games WHERE game_code = {code}";
            return SqlAdapterQuery(query);
        }

        public object LoadMatchTitle(int matchCode)
        {
            string query = $"SELECT match_title FROM match WHERE match_code = {matchCode}";
            return ScalaQuery(query);
        }

        public DataTable LoadAllPalyerList(int code)
        {
            string query = $"SELECT att_name, att_memcode, att_memtype, att_gender, att_pro, att_handi, att_side, att_allcover FROM attend WHERE att_code = {code}";
            return SqlAdapterQuery(query);
        }

        public void UpdatePlayerInfo(PlayerInfoDto player, int match)
        {
            string query = "UPDATE attend SET att_gender = @gender, att_pro = @pro, att_side = @side, att_allcover = @allcover, att_handi = @handi " +
                "WHERE att_code = @match AND att_name = @playerName";
            SqlParameter[] parameters =
            {
                new SqlParameter("@match", SqlDbType.Int){Value = match },
                new SqlParameter("@playerName", SqlDbType.VarChar){Value = player.PlayerName },
                new SqlParameter("@gender", SqlDbType.TinyInt){Value = player.Gender ? 1 :0 },
                new SqlParameter("@pro", SqlDbType.TinyInt){Value = player.IsPro ? 1 :0 },
                new SqlParameter("@side", SqlDbType.TinyInt){Value = player.IndividualSide ? 1 :0 },
                new SqlParameter("@allcover", SqlDbType.TinyInt){Value = player.AllCoverSide ? 1 :0 },
                new SqlParameter("@handi", SqlDbType.Int){Value = player.Handycap }
            };
            
            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    ExecuteNonQuery(query, connection, transaction, parameters);
                    
                    query = "UPDATE players SET pl_gender = att_gender, pl_pro = att_pro, pl_side = att_side, pl_allcover = att_allcover, pl_handi = att_handi " +
                        $" FROM players JOIN attend ON pl_match = att_code AND pl_name = att_name WHERE pl_match = {match} AND pl_name = '{player.PlayerName}'";
                    ExecuteNonQuery(query, connection, transaction, null);
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public DataRow LoadGamePlayer(int match, int game, string name)
        {
            string query = $"SELECT pl_group, pl_handi ,pl_score FROM players WHERE pl_match = {match} AND pl_game = {game} AND pl_name ='{name}'";
            return SqlAdapterQuery(query).Rows[0];
        }

        public void UPdatePlayerScore(PlayerInfoDto player,int match, int game)
        {
            string query = "UPDATE players SET pl_score = @score , pl_isallcover = @allcorver WHERE pl_match = @match AND pl_game = @game AND pl_name = @name";

            using(SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@score",SqlDbType.Int){ Value = player.Score},
                        new SqlParameter("@allcorver",SqlDbType.Int){ Value = player.IsAllCover == true? 1 : 0},
                        new SqlParameter("@match",SqlDbType.Int){ Value = match},
                        new SqlParameter("@game",SqlDbType.Int){ Value = game},
                        new SqlParameter("@name",SqlDbType.VarChar){ Value = player.PlayerName}
                    };
                    ExecuteNonQuery(query, connection, transaction, parameters);
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        public DataTable LoadGamePlayers(int match)
        {
            string query = $"SELECT pl_game ,pl_group, pl_member, pl_name,pl_handi,pl_pro,pl_gender,pl_side,pl_allcover,pl_score, pl_isallcover FROM players WHERE pl_match = {match} ORDER BY pl_game, pl_group";
            return SqlAdapterQuery(query);
        }
    }
}
