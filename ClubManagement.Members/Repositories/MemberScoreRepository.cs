using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ClubManagement.Common.Repositories;
using ClubManagement.Members.DTOs;

namespace ClubManagement.Members.Repositories
{
    public class MemberScoreRepository : BaseRepository, IMemberScoreRepository
    {
        public int GetGameCount(int matchCode)
        {
            string query = $"SELECT MAX(game_order) FROM games WHERE game_match = {matchCode}";
            return Convert.ToInt32(ScalaQuery(query));
        }

        public DataTable GetGradeInfo()
        {
            string query = "SELECT grd_code, grd_name FROM grade";
            return SqlAdapterQuery(query);
        }

        public DataTable GetMatchScore(SearchScoreDto search, int gameCount)
        {
            int matchCode = search.MatchCode;
            //게임 칼럼 생성
            List<string> columnList = new List<string>();
            for (int i = 1;  i <= gameCount; i++)
            {
                columnList.Add("[Game" + i.ToString() + "]");
            }
            string gamecolumn = string.Join(",", columnList);

            //정렬기준
            string sortCondition = "";
            switch (search.SortType)
            {
                case 0:
                    sortCondition = "average_score DESC";
                    break;
                case 1:
                    sortCondition = "reference_average DESC";
                    break;
                case 2:
                    sortCondition = "reference_gap DESC";
                    break;
                case 3:
                    sortCondition = "game_gap DESC";
                    break;
            }

            //기준 에버 조회
            string query = "DECLARE @Average TABLE ( mem_code INT PRIMARY KEY,reference_average DECIMAL(5,1));	\n" +
                "DECLARE @fromdate date, @todate date , @interval int;\n" +
                "SET @interval = (SELECT cf_value FROM config WHERE cf_code = 14)\n" +
                $"SET @todate = (SELECT match_date FROM match WHERE match_code = {matchCode})\n" +
                "SET @fromdate = DATEADD(MONTH,@interval*-1,@todate)\n" +

                "INSERT INTO @Average (mem_code, reference_average)	\n" +
                "SELECT mem_code, CAST(CASE WHEN COUNT(pl.pl_score) > 0 THEN 1.0 * SUM(a.att_handi + pl.pl_score)  / COUNT(pl.pl_score)ELSE 0 END AS DECIMAL(5,1)) AS average_score	\n" +
                $"FROM member m JOIN (SELECT att_memcode FROM attend  WHERE att_code = {matchCode}) as ea ON m.mem_code = ea.att_memcode	\n" +
                "JOIN attend a ON m.mem_code = a.att_memcode	\n" +
                "JOIN (SELECT match_code FROM match WHERE match_type = 1 AND match_date >= @fromdate AND match_date <  @todate) mt ON a.att_code = mt.match_code	\n" +
                "JOIN players pl ON mt.match_code = pl.pl_match AND pl.pl_member = a.att_memcode AND pl.pl_score != 0	\n" +
                "GROUP BY mem_code;\n\n"+

                $"SELECT mem_code, mem_name, grd_name, reference_average,ISNULL(att_handi, 0) AS mem_handi,ROUND(AVG_SCORE, 2) AS average_score, SUM_SCORE AS total_score,\n" +
                gamecolumn + ",ROUND(AVG_SCORE, 2) -reference_average as reference_gap,MAX_SCORE,MIN_SCORE, MAX_SCORE - MIN_SCORE AS game_gap\n" +
                "FROM \n" +
                "( SELECT m.mem_code, m.mem_name, grd_name, a.att_handi, 'Game' + CAST(g.game_order AS NVARCHAR) AS GameOrder, CASE WHEN a.att_code = p.pl_match AND a.att_memcode = p.pl_member THEN p.pl_score + ISNULL(a.att_handi, 0) ELSE p.pl_score END AS Score,\n" +
                "CAST(AVG(CASE WHEN a.att_code = p.pl_match AND a.att_memcode = p.pl_member THEN 1.0 * p.pl_score + ISNULL(a.att_handi, 0) ELSE 1.0 *p.pl_score END) OVER (PARTITION BY m.mem_code) AS DECIMAL(5,1)) AS AVG_SCORE,\n" +
                "SUM(CASE WHEN a.att_code = p.pl_match AND a.att_memcode = p.pl_member THEN p.pl_score + ISNULL(a.att_handi, 0) ELSE p.pl_score END) OVER (PARTITION BY m.mem_code) as SUM_SCORE,\n" +
                "ref.reference_average, Max(pl_score + att_handi) OVER (PARTITION BY m.mem_code) AS MAX_SCORE, MIN(pl_score + att_handi) OVER (PARTITION BY m.mem_code) AS MIN_SCORE\n" +
                $"FROM member m JOIN attend a ON m.mem_code = a.att_memcode AND a.att_code = {matchCode} \n" +
                $"JOIN players p ON p.pl_match = {matchCode} AND p.pl_member = m.mem_code \n" +
                $"JOIN games g ON g.game_match = p.pl_match AND g.game_order = p.pl_game\n" +
                $"JOIN @Average as ref ON m.mem_code = ref.mem_code	\n" +
                $"JOIN grade gr ON m.mem_grade = gr.grd_code) AS SourceTable\n" +
                $"PIVOT (MAX(Score) FOR GameOrder IN ({gamecolumn})) AS PivotTable\nORDER BY " + sortCondition;
            return SqlAdapterQuery(query);
        }

        public string GetMatchTitle(int matchCode)
        {
            string query = $"SELECT match_title FROM match WHERE match_code = {matchCode}";
            return ScalaQuery(query).ToString();
        }

        public DataRow GetMemberBaseInfo( int member)
        {
            string query = "DECLARE @fromdate date, @todate date , @interval int;\n" +                                                     //기준 에버 계산기간 변수
                "SET @interval = (SELECT cf_value FROM config WHERE cf_code = 14)\n" +                                          //에버 기준 기간 변수값 설정
                $"SET @todate = GETDATE()\n" +                                                 //기준 에버 생성 종료일
                "SET @fromdate = DATEADD(MONTH,@interval*-1,@todate)\n\n" + 
                "SELECT m.mem_name, m.mem_gender,ISNULL(m.mem_grade,-1) mem_grade,\n" +
                "CASE WHEN m.mem_gender = 1 THEN 15 ELSE 0 END + CASE WHEN m.mem_pro = 1 THEN -5 ELSE 0 END as mem_handi,\n" +
                "CAST(AVG(CASE WHEN a.att_code = pl.pl_match AND a.att_memcode = pl.pl_member THEN 1.0 * pl.pl_score + ISNULL(a.att_handi, 0)ELSE 1.0 * pl.pl_score END) AS DECIMAL(5,1)) AS AVG_SCORE\n" +
                $"FROM member m JOIN attend a ON m.mem_code = a.att_memcode AND m.mem_code = {member}\n" +
                $"JOIN (SELECT match_code FROM match WHERE match_type = 1 AND match_date >= @fromdate AND match_date < @todate) mt ON a.att_code = mt.match_code\n" +
                $"JOIN players pl ON mt.match_code = pl.pl_match AND pl.pl_member = a.att_memcode AND pl.pl_score != 0\n" +
                $"GROUP BY m.mem_code, m.mem_name, m.mem_gender,mem_grade, m.mem_pro";
            
            return SqlAdapterQuery(query).Rows[0];
        }

        public DataTable GetMemberScoreList(SearchScoreDto search, int member)
        {
            string fromDate = search.FromDate.ToString("yyyy-MM-dd");
            string toDate = search.ToDate.ToString("yyyy-MM-dd");
            string query = $"SELECT max(game_order)   FROM games WHERE game_match IN (SELECT match_code FROM match, attend WHERE match_date > '{fromDate}' AND match_date < '{toDate}' AND match_record = 1 AND match_code = att_code AND att_memcode = {member})";
            int maxCount = Convert.ToInt32(ScalaQuery(query));

            //게임 칼럼 생성
            List<string> columnList = new List<string>();
            for (int i = 1; i <= maxCount; i++)
            {
                columnList.Add("[Game" + i.ToString() + "]");
            }
            string gamecolumn = string.Join(",", columnList);

            query = $"SELECT match_date, match_title, {gamecolumn}, game_totalscore, game_average\n" +
                "FROM\n" +
                "(SELECT match_code, match_date, match_title, att_handi,  'Game' + CAST(g.game_order AS NVARCHAR) AS GameOrder,\n" +
                "CAST(CASE WHEN a.att_code = p.pl_match AND a.att_memcode = p.pl_member THEN 1.0 * p.pl_score + ISNULL(a.att_handi, 0) ELSE 1.0 * p.pl_score END AS DECIMAL(5,1)) AS Score,\n" +
                "AVG(p.pl_score + ISNULL(a.att_handi, 0)) OVER (PARTITION BY p.pl_member, pl_match) as game_average,\n" +
                "SUM(p.Pl_score + ISNULL(a.att_handi, 0)) OVER (PARTITION BY p.pl_member, pl_match) as game_totalscore\n" +
                "FROM member as m JOIN attend as a ON mem_code = 8 AND m.mem_code = a.att_memcode \n" +
                $"JOIN match as mt ON a.att_code = mt.match_code AND match_date >= '{fromDate}' AND match_date < '{toDate}' AND match_record = 1 AND match_type = 1 \n" +
                "JOIN players as p ON mt.match_code = p.pl_match AND p.pl_score != 0 \n" +
                $"JOIN games g ON g.game_match = p.pl_match AND g.game_order = p.pl_game AND p.pl_member = {member}) as source\n" +
                $"PIVOT(MAX(Score) FOR GameOrder IN ({gamecolumn})) AS PivotTable\n" +
                $"ORDER BY match_date";
            return SqlAdapterQuery(query);
        }

        public string GetStartDate()
        {
            string query = "SELECT cf_strval  FROM config where cf_code =  12";
            return ScalaQuery(query).ToString();
        }

        public DataTable GetTotalScore(SearchScoreDto search)
        {
            // 회원 상태 조회 조건 생성
            string statusCondition;
            if (search.Status == -1)
                statusCondition = $"";
            else
                statusCondition = search.IsExcludedMember ? $"WHERE mem_status != {search.Status}" : $"WHERE mem_status = {search.Status}\n";

            // 정렬 조건 생성
            string sortCondition = "";
            switch(search.SortType)
            {
                case 0:
                    sortCondition = "average_score DESC";
                    break;
                case 1:
                    sortCondition = "reference_average DESC";
                    break;
                case 2:
                    sortCondition = "reference_gap DESC";
                    break;
                case 3:
                    sortCondition = "game_gap DESC";
                    break;
            }

            StringBuilder query = new StringBuilder();
            string referenceAver = "DECLARE @Average TABLE ( mem_code INT PRIMARY KEY, reference_average DECIMAL(5,1));\n" +    //기준 에버리지 계산용 임시 테이블
                "DECLARE @fromdate date, @todate date , @interval int;\n" +                                                     //기준 에버 계산기간 변수
                "SET @interval = (SELECT cf_value FROM config WHERE cf_code = 14)\n" +                                          //에버 기준 기간 변수값 설정
                $"SET @todate = '{search.FromDate.ToString("yyyy-MM-dd")}'\n" +                                                 //기준 에버 생성 종료일
                "SET @fromdate = DATEADD(MONTH,@interval*-1,@todate)\n\n" +                                                     //기준 에버 생성 시작일(종료일로 부터 기준 기간으로)
                //기준 에버 테이블 등록
                "INSERT INTO @Average (mem_code, reference_average)\n" +
                "SELECT mem_code, CAST(CASE WHEN COUNT(pl.pl_score) > 0 THEN 1.0 * SUM(a.att_handi + pl.pl_score) / COUNT(pl.pl_score)ELSE 0 END AS DECIMAL(5, 1)) AS average_score\n" +
                "FROM member m\n" +
                "LEFT JOIN attend a ON m.mem_code = a.att_memcode\n" +
                "LEFT JOIN(SELECT match_code FROM match WHERE match_type = 1 AND match_date >= @fromdate AND match_date<@todate) mt ON a.att_code = mt.match_code\n" +
                "LEFT JOIN players pl ON mt.match_code = pl.pl_match AND pl.pl_member = a.att_memcode AND pl.pl_score != 0\n" +
                statusCondition +"\n" +
                "GROUP BY mem_code;\n";
            query.Append(referenceAver);

            string searchQyuer = "SELECT mem_code, mem_name, grd_name, mem_handi, game_count, total_score, average_score,reference_average,\n" +
                 "CASE WHEN average_score > 0 THEN average_score - reference_average ELSE 0 END AS reference_gap, \n" +
                 "max_score, min_score, max_score - min_score as game_gap\n" +
                 "FROM\n" +
                 "( SELECT m.mem_code, m.mem_name, grd_name,\n" +
                 "CASE WHEN m.mem_gender = 1 THEN 15 ELSE 0 END + CASE WHEN m.mem_pro = 1 THEN -5 ELSE 0 END as mem_handi, \n" +
                 "COUNT(p.pl_score) AS game_count,aver.reference_average,ISNULL(SUM(a.att_handi + p.pl_score), 0) AS total_score,\n" +
                 "CAST(CASE WHEN COUNT(p.pl_score) > 0 THEN 1.0 * SUM(a.att_handi + p.pl_score) / COUNT(p.pl_score) ELSE 0 END as decimal(5,1)) AS average_score,\n" +
                 "MAX(ISNULL(a.att_handi + p.pl_score,0)) AS max_score,\n" +
                 "ISNULL(MIN(a.att_handi + p.pl_score),0) AS min_score,\n" +
                 "ISNULL(MIN(a.att_handi + p.pl_score),0) as game_gap\n" +
                 "FROM member m JOIN @Average as aver ON m.mem_code = aver.mem_code\n" +
                 "LEFT JOIN attend a ON m.mem_code = a.att_memcode\n" +
                 $"LEFT JOIN (SELECT match_code FROM match WHERE match_type = 1 AND match_date >= '{search.FromDate.ToString("yyyy-MM-dd")}' AND match_date <  '{search.ToDate.ToString("yyyy-MM-dd")}') mt ON a.att_code = mt.match_code\n" +
                 "LEFT JOIN players p ON mt.match_code = p.pl_match AND p.pl_member = a.att_memcode AND p.pl_score != 0\n" +
                 "JOIN grade gr ON m.mem_grade = gr.grd_code	\n" +
                 statusCondition + "\n" +
                 "GROUP BY m.mem_code, m.mem_name, grd_name, m.mem_gender, m.mem_pro,reference_average) as source\n" +
                 "ORDER BY " ;
;
            query.Append(searchQyuer);
            query.Append(sortCondition);
            return SqlAdapterQuery(query.ToString());
        }

        public DataTable LoadMatchList(SearchMatchDto search)
        {
            StringBuilder query = new StringBuilder();
            query.Append("SELECT match_code, match_date, match_type, match_title, match_host, ");
            query.Append("COUNT(att_code) as totalplayer, COUNT(CASE WHEN att_memtype = 1 THEN 1 END) as member, COUNT( CASE WHEN att_memtype = 2 THEN 1 END) as guest, match_record ");
            query.Append("FROM Match LEFT OUTER JOIN attend ON Match_code =att_code WHERE");
            query.Append($" Match_date >= '{search.FromDate.ToString("yyyy-MM-dd")}' AND Match_date < '{search.ToDate.AddDays(1).ToString("yyyy-MM-dd")}'");

            if (search.MatchType != 0)
            {
                if (search.ExcludeType)
                {
                    query.Append($" AND Match_type != {search.MatchType}");
                }
                else
                {
                    query.Append($" AND Match_type = {search.MatchType}");
                }
            }
            if (search.IsRecordBoardRegisted == true)
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

        public void UpdateMemberGrade(int member, int grade)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@member", SqlDbType.Int){Value = member},
                new SqlParameter("@grade", SqlDbType.Int){Value = grade}
            };
            
            using(SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    ExecuteNoneQuery(StoredProcedures.UpdateMemberGrade, connection, transaction, parameters);
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("회원 등급 수정 오류\n " + ex.Message);
                }
            }
        }

        public void UpdateMemberGradeBulk(List<int> members, int grade)
        {
            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    foreach(var member in members)
                    {
                        SqlParameter[] parameters =
                        {
                        new SqlParameter("@member", SqlDbType.Int){Value = member},
                        new SqlParameter("@grade", SqlDbType.Int){Value = grade}
                        };

                        ExecuteNoneQuery(StoredProcedures.UpdateMemberGrade, connection, transaction, parameters);
                        
                    };
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("회원 등급 수정 오류\n" + ex.Message);
                }
            }
        }
        public void UpdateGradeInfo(DataTable gradeItems, int? deleteCode = null)
        {
            using(SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    foreach(DataRow item in gradeItems.Rows)
                    {
                        SqlParameter[] parameters =
                        {
                            new SqlParameter("@code", SqlDbType.Int){Value=item["grd_code"]},
                            new SqlParameter("@name", SqlDbType.NVarChar){Value = item["grd_name"]}
                        };
                        ExecuteNoneQuery(StoredProcedures.UpdateGradeInfo, connection, transaction, parameters);
                    }

                    if(deleteCode.HasValue)
                    {
                        SqlParameter[] deleteParam =
                        {
                            new SqlParameter("@code", SqlDbType.Int){Value = deleteCode}
                        };
                        ExecuteNoneQuery(StoredProcedures.DeleteGradeInfo, connection, transaction, deleteParam);
                    }
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("회원 등급 정보 수정 오류\n" + ex.Message);
                }
            }
        }

        public int GetAverageInterval()
        {
            string query = "SELECT cf_value FROM config WHERE cf_code = 14";
            return Convert.ToInt32(ScalaQuery(query));
        }
        public void UpdateAverageInter(int interval)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@code", SqlDbType.Int){Value = 14},
                new SqlParameter("@value", SqlDbType.Int){Value = interval},
                new SqlParameter("@str", SqlDbType.VarChar){Value = ""}
            };
            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    ExecuteNoneQuery(StoredProcedures.UpdateConfig, connection, transaction, parameters);
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("기준 에버 설정 실패" + ex.Message);
                }
            }
        }
    }
}
