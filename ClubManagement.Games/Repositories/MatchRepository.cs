using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ClubManagement.Games.Models;
using ClubManagement.Common.Repositories;

namespace ClubManagement.Games.Repositories
{
    class MatchRepository : BaseRepository, IMatchRepository
    {
        /// <summary>
        /// 모임 리스트 조회
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataTable LoadMatchList(MatchSearchModel model)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT match_code, match_date, match_type, match_title, match_host, ");
            query.Append("COUNT(att_code) as totalplayer, COUNT(CASE WHEN att_memtype = 1 THEN 1 END) as member, COUNT( CASE WHEN att_memtype = 2 THEN 1 END) as guest, match_record ");
            query.Append("FROM Match LEFT OUTER JOIN attend ON Match_code =att_code WHERE");
            query.Append($" Match_date >= '{model.FromDate.ToString("yyyy-MM-dd")}' AND Match_date < '{model.ToDate.AddDays(1).ToString("yyyy-MM-dd")}'");

            if (model.MatchType != 0)
            {
                if (model.ExcludeType)
                {
                    query.Append($" AND Match_type != {model.MatchType}");
                }
                else
                {
                    query.Append($" AND Match_type = {model.MatchType}");
                }
            }
            if (model.IsRecordBoardRegisted == true)
            {
                query.Append(" AND match_record = 1");
            }
            query.Append(" GROUP BY match_code, match_date, match_type, match_title, match_host, match_record ORDER BY Match_date, Match_code");
            try
            {
                return SqlAdapterQuery(query.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 모임 선택 시 참석자 리스트 조회
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable LoadPlayerList(int code)
        {
            string query = ($"SELECT att_name,  ISNULL(RTRIM(mem_birth),'') mem_birth, att_gender, att_memtype FROM attend LEFT JOIN member ON att_memcode = mem_code  WHERE att_code = {code}");
            try
            {
                return SqlAdapterQuery(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 회원정보 조회
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataTable LoadMember(MatchSearchModel model)
        {
            StringBuilder query = new StringBuilder();
            List<string> whereCondition = new List<string>();
            query.Append("SELECT mem_code, mem_name, mem_gender, mem_pro FROM member ");
            if (!string.IsNullOrEmpty(model.SearchWord))
            {
                whereCondition.Add($"mem_name LIKE '%{model.SearchWord}%'");
            }
            if (!model.IncludeSecessMember)
            {
                whereCondition.Add($"mem_status != 2");
            }

            if (whereCondition.Count > 0)
            {
                query.Append(" WHERE ");
                query.Append(string.Join(" AND ", whereCondition));
            }
            query.Append(" ORDER BY mem_name ");

            return SqlAdapterQuery(query.ToString());
        }

        /// <summary>
        /// 모임 참석자 리스트 조회
        /// </summary>
        /// <param name="mactchCode"></param>
        /// <returns></returns>
        public DataTable LoadAttendPlayer(int mactchCode)
        {
            string query = $"SELECT att_name, ISNULL(att_memcode,0) att_memcode, att_gender, att_pro, att_handi FROM attend WHERE att_code = {mactchCode} ";
            return SqlAdapterQuery(query);
        }

        /// <summary>
        /// 모임 상세정보 조회
        /// </summary>
        /// <param name="matchCode"></param>
        /// <returns></returns>
        public DataRow LoadMatchInfo(int matchCode)
        {
            string query = $"SELECT match_title, match_host, match_date, match_type, match_memo FROM match WHERE match_code = {matchCode}";
            DataTable result = SqlAdapterQuery(query);
            return result.Rows[0];
        }

        /// <summary>
        /// 모임 참석자 수정
        /// </summary>
        /// <param name="model"></param>
        public void UpdateMatchPlayer(MatchModel model)
        {
            int matchCode = (int)model.MatchCode;
            DataTable parmaTable = new DataTable();
            parmaTable.Columns.Add("player_memcode", typeof(int));
            parmaTable.Columns.Add("player_name", typeof(string));
            parmaTable.Columns.Add("player_gender", typeof(byte));
            parmaTable.Columns.Add("player_handicap", typeof(int));
            parmaTable.Columns.Add("player_isPro", typeof(byte));
            parmaTable.Columns.Add("player_individual", typeof(byte));
            parmaTable.Columns.Add("player_allcover", typeof(byte));
            parmaTable.Columns.Add("player_isAllcover", typeof(byte));
            parmaTable.Columns.Add("player_score", typeof(int));

            foreach (var player in model.PlayerList)
            {
                parmaTable.Rows.Add(
                player.MemberCode,
                player.PlayerName,
                player.Gender ? 1 : 0,
                player.Handycap,
                player.IsPro ? 1 : 0
                );
            }

                using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    SqlParameter[] parameters =
                    {
                        new SqlParameter("@match", SqlDbType.Int){ Value = model.MatchCode},
                        new SqlParameter{
                            ParameterName = "@PlayerList",
                            SqlDbType = SqlDbType.Structured,
                            TypeName = "dbo.PlayerInfo",
                            Value = parmaTable
                        }
                    };
                    ExecuteNoneQuery(StoredProcedures.UpdateMatchPlayer, connection, transaction, parameters);
                    transaction.Commit();
                }

                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }

        }

        /// <summary>
        /// 모임 정보 수정
        /// </summary>
        /// <param name="model"></param>
        public void UpdateMatch(MatchModel model)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter ("@match",SqlDbType.Int){Value = model.MatchCode},
                new SqlParameter ("@title",SqlDbType.NVarChar){Value = model.MatchTitle},
                new SqlParameter ("@host",SqlDbType.NVarChar){Value = model.MatchHost},
                new SqlParameter ("@date",SqlDbType.Date){Value = model.MatchDate.Value.ToString("yyyy-MM-dd")},
                new SqlParameter ("@type",SqlDbType.Int){Value = model.MatchType},
                new SqlParameter ("@memo",SqlDbType.NVarChar){Value = model.MatchMemo},
            };
            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    ExecuteNoneQuery(StoredProcedures.UpdateMatch, connection, transaction, parameters);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// 모임 정보 등록
        /// </summary>
        /// <param name="model"></param>
        public void InsertMatch(MatchModel model)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter ("@title",SqlDbType.NVarChar){Value = model.MatchTitle},
                new SqlParameter ("@host",SqlDbType.NVarChar){Value = model.MatchHost},
                new SqlParameter ("@date",SqlDbType.Date){Value = model.MatchDate.Value.ToString("yyyy-MM-dd")},
                new SqlParameter ("@type",SqlDbType.Int){Value = model.MatchType},
                new SqlParameter ("@memo",SqlDbType.NVarChar){Value = model.MatchMemo},
            };
            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    ExecuteNoneQuery(StoredProcedures.InsertMatch, connection, transaction, parameters);
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
