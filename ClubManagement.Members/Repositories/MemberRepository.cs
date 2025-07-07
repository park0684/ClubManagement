using ClubManagement.Common.Repositories;
using ClubManagement.Members.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ClubManagement.Members.Repositories
{
    class MemberRepository : BaseRepository, IMemberRepository
    {
        public DataTable LodaMemberList(MemberSearchModel model)
        {
            //회원검색 query는 회원, 정기전 참석, 비정기전 참석, 이벤트전 참석, 회비납부 현황을 각각 FROM에서 구한 후 LEFT OUTER JOIN으로 연결
            //
            
            
            StringBuilder query = new StringBuilder();

            query.Append("SELECT m.mem_code, mem_name ,mem_birth, mem_gender, mem_position,");
            query.Append("\nISNULL(match_regular.att_count,0) regular_count, ");
            query.Append("\nCASE WHEN ISNULL(match_regular.att_count,0) =0 THEN 0 ELSE CAST(ISNULL(match_regular.att_count,0) as decimal)/(cast(ISNULL(regular_game.game_count, 0) as decimal)) END regular_rate, ");
            query.Append("ISNULL(match_irregular.att_count, 0) irregular_count,");
            query.Append(" \nISNULL(match_event.att_count, 0) event_count,");
            query.Append(" \nISNULL(regular_game.game_count, 0) game_count, \nmem_access, mem_secess, mem_status, ");
            query.Append("ISNULL((SELECT MAX(match_date) FROM attend, match WHERE att_code = match_code AND att_name = mem_name),NULL) match_last,");
            query.Append($"CASE WHEN mem_position = 5 THEN 0 ELSE CAST(DATEDIFF(MONTH,CASE WHEN mem_access < '{model.StartDate}' THEN '{model.StartDate}'  ELSE mem_access END ,GETDATE()) as INT) + 1 END mem_dues,ISNULL(payment, 0) payment, mem_memo \nFROM ");
            //회원 검색 구간
            query.Append("(SELECT mem_code, mem_name ,mem_birth, mem_gender, mem_position, mem_access, mem_secess, mem_status,");
            query.Append(" CAST(DATEDIFF(MONTH,CASE WHEN mem_access < '2025-02-01' THEN '2025-02-01'  ELSE mem_access END,");
            query.Append($"'{DateTime.Now.ToString("d")}') as INT) + 1 [mem_duse], mem_memo FROM member \n");

            List<string> memberConditio = SetWhereCondition(model);
           
            if (memberConditio.Count > 0)
            {
                query.Append(" WHERE ");
                query.Append(string.Join(" AND ", memberConditio));
            }
            query.Append(") as m LEFT OUTER JOIN \n");

            
            //참가일 설정은 기본적으로 config에 등록된 시작일부터 모든 모임을 검색하는 것으로 설정
            string gameDate = $"match_date > '{model.StartDate}' AND match_date < '{DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")}' AND "; ; 
            query.Append($"(SELECT mem_code, COUNT(match_code) game_count FROM member, match");
            if (model.GameCheck) // 참석일 설정이 되어 있다면 해당 기간내 정기전만으로 게임 카운트 설정
            {
                gameDate = $" match_date >= '{model.GameFromDate.Value.ToString("yyyy-MM-dd")}' AND match_date < '{model.GameTodate.Value.AddDays(1).ToString("d")}' AND ";
                query.Append($" WHERE {gameDate} match_type = 1 AND match_date >= mem_access GROUP BY mem_code) as regular_game ON m.mem_code = regular_game.mem_code LEFT OUTER JOIN \n");
            }
            else // 체크되지 않았다면 참가일 설정은 기존 유지 && 정기전 대상 모임의 수를 가입 후 모든 정기 모임으로 카운트 설정 
            {
                query.Append(" WHERE match_type = 1 AND match_date >= mem_access GROUP BY mem_code) as regular_game ON m.mem_code = regular_game.mem_code LEFT OUTER JOIN \n");
            }
            
            // 정기전 참석 현황 집계
            if (!model.ExcludeRegular)
                query.Append(GetGameQuery(1, gameDate));
            else
                query.Append(GetGameQuery(1));
            query.Append("as match_regular ON m.mem_code = match_regular.att_memcode LEFT OUTER JOIN \n");

            // 비정기전 참석 현황 집계
            if (!model.ExcludeIrregular)
                query.Append(GetGameQuery(2, gameDate));
            else
                query.Append(GetGameQuery(2));
            query.Append("as match_irregular ON m.mem_code = match_irregular.att_memcode LEFT OUTER JOIN \n");

            // 이벤트전 참석 현황 집계
            if (!model.ExcludeEvent)
                query.Append(GetGameQuery(3, gameDate));
            else
                query.Append(GetGameQuery(3));
            query.Append("as match_event ON m.mem_code = match_event.att_memcode LEFT OUTER JOIN \n");

            //회비 납부 현황 집계
            query.Append("(select du_memcode, sum(du_apply) as payment FROM dues WHERE du_status = 1 AND du_type = 1 GROUP BY du_memcode) as dues ON m.mem_code = du_memcode");

            //ORDER BY 설정
            //2025-07-05 정렬 콤보 박스 추가
            switch(model.SortType)
            {
                case 0:
                    query.Append(" ORDER BY mem_position, mem_status, m.mem_code");
                    break;
                case 1:
                    query.Append(" ORDER BY regular_rate DESC, mem_position, mem_status, m.mem_code");
                    break;
                case 2:
                    query.Append(" ORDER BY regular_rate, mem_position, mem_status, m.mem_code");
                    break;
                case 3:
                    query.Append(" ORDER BY regular_count DESC, mem_position, mem_status, m.mem_code");
                    break;
                case 4:
                    query.Append(" ORDER BY regular_count, mem_position, mem_status, m.mem_code");
                    break;
                case 5:
                    query.Append(" ORDER BY regular_count DESC, regular_rate DESC, mem_position, mem_status, m.mem_code");
                    break;
            }
            return SqlAdapterQuery(query.ToString());
        }


        private List<string> SetWhereCondition(MemberSearchModel model)
        {
            //반환 값이 적용될 변수 설정
            var condition = new List<string>();

            //회원 검색어가 있다면 조건문에 회원 검색 부분 추가
            if (!string.IsNullOrEmpty(model.SearchWord))
                condition.Add($"mem_name LIKE '%{model.SearchWord}%'");

            //상태값이 0이 아니라면 특정 상태값 혹은 특정 상태값만 제외 해야함
            if (model.Status != 0)
                if (model.ExcludeMember) // 제외 체크박스가 체크되어 True값을 받아 왔다면 지정된 상태값을 제외로
                {
                    condition.Add($"mem_status != {model.Status}");
                }
                else // false 값이라면 지정된 상태값으로 설정
                { condition.Add($"mem_status = {model.Status}"); }

            //가입일을 지정했다면 해당 가입일내 회원만 검색하도록 조건문 추가
            if (model.AccessCheck)
                condition.Add($"mem_access >=  '{ model.AccFromDate.Value.ToString("yyyy-MM-dd")}' AND mem_access < '{model.AccToDate.Value.AddDays(1).ToString("yyyy-MM-dd")}'");

            //탈퇴일을 지정했다면 해당 탈퇴일내 회원만 검색하도록 조건문 추가
            if (model.SecessCheck)
                condition.Add($"mem_secess >=  '{ model.SecFromDate.Value.ToString("yyyy-MM-dd")}' AND mem_secess < '{model.SecToDate.Value.AddDays(1).ToString("yyyy-MM-dd")}'");

            //2025년 7월 5일 참가기간 설정 조회 쿼리 수정
            //기간내 참석자만 조회하던 부분에서 전체회원 대상 기간내 참석내역만 조회 또는 미참석자 조회 기능 추가
            if (model.GameCheck)
                switch (model.AttendInclude)
                {
                    case 0: // 전체회원 조회
                        break;
                    case 1: // attend 테블이에 회원코드가 있는 회원만 
                        condition.Add($"mem_code IN ( SELECT DISTINCT(att_memcode) FROM attend, match WHERE match_code = att_code AND match_date >='{model.GameFromDate.Value.ToString("yyyy-MM-dd")}' AND match_date < '{model.GameTodate.Value.AddDays(1).ToString("yyyy-MM-dd")}' AND att_memtype = 1 AND att_memcode IS NOT NULL)");
                        break;
                    case 2: // attend 테이블에 회원코드가 없는 회원
                        condition.Add($"mem_code NOT IN (SELECT DISTINCT(att_memcode) FROM attend, match WHERE match_code = att_code AND match_date >='{model.GameFromDate.Value.ToString("yyyy-MM-dd")}' AND match_date < '{model.GameTodate.Value.AddDays(1).ToString("yyyy-MM-dd")}' AND att_memtype = 1 AND att_memcode IS NOT NULL)");
                        break;

                }
            return condition;
        }
        /// <summary>
        /// 게임
        /// </summary>
        /// <param name="type"></param>
        /// <param name="gameDate"></param>
        /// <returns></returns>
        public string GetGameQuery(int type, string gameDate = null)
        {
            string query;
            if (gameDate != null)
            {
                query = "(SELECT att_memcode, COUNT(att_name) att_count FROM attend, match WHERE ";
                query += gameDate;
            }
            else
                query = "(SELECT att_memcode, 0 att_count FROM attend, match WHERE ";
            query += $" match_type = {type} AND att_code = match_code GROUP BY att_memcode)";
            return query;

        }

        /// <summary>
        /// 회원 상세 정보 조회
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataRow LoadMemberInfo(int value)
        {
            string query = $"SELECT mem_name, mem_birth, mem_gender, mem_status, mem_position, mem_access, mem_secess, mem_memo FROM member WHERE mem_code = {value}";
            DataTable result = SqlAdapterQuery(query);

            return result.Rows[0];
        }

        /// <summary>
        /// 회원 지정을 위한 회원검색
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataTable LoadSearchMember(MemberSearchModel model)
        {
            StringBuilder query = new StringBuilder();
            List<string> whereCondition = new List<string>();
            query.Append($"SELECT mem_code, mem_name FROM member ");
            if (!string.IsNullOrEmpty(model.SearchWord))
                whereCondition.Add($"mem_name LIKE '%{model.SearchWord}%'");
            if (!model.StatusInclude)
                whereCondition.Add("mem_status != 2");
            if (whereCondition.Count > 0)
                query.Append(" WHERE ");
            query.Append(string.Join(" AND ", whereCondition));
            query.Append(" ORDER BY mem_name");

            return SqlAdapterQuery(query.ToString());
        }

        /// <summary>
        /// 새 회원 등록
        /// </summary>
        /// <param name="model"></param>
        public void InsertMember(MemberModel model)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@name",SqlDbType.VarChar){Value = model.MemberName},
                new SqlParameter("@status",SqlDbType.Int){Value=model.Status },
                new SqlParameter("@birth",SqlDbType.VarChar){Value=model.Birth },
                new SqlParameter("@gender",SqlDbType.Int){Value = model.Gender},
                new SqlParameter("@position", SqlDbType.Int){ Value = model.Position },
                new SqlParameter("@memo", SqlDbType.VarChar){ Value = model.Memo },
                new SqlParameter("@access", SqlDbType.Date){Value = model.AccessDate.Value.ToString("yyyy-MM-dd") },
                new SqlParameter("@pro",SqlDbType.Int){ Value = model.IsPro == true ? 1:0 } 
            };
            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    ExecuteNoneQuery(StoredProcedures.InsertMember, connection, transaction, parameters);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("회원 등록 오류 : "+ex.Message);
                }
            }
        }

        /// <summary>
        /// 회원 정보 수정
        /// </summary>
        /// <param name="model"></param>
        public void UpdateMember(MemberModel model)
        {
            
            SqlParameter[] parameters =
            {
                new SqlParameter("@memcode",SqlDbType.Int){Value = model.Code },
                new SqlParameter("@name",SqlDbType.NVarChar){Value = model.MemberName},
                new SqlParameter("@status",SqlDbType.Int){Value=model.Status },
                new SqlParameter("@birth",SqlDbType.NVarChar){Value=model.Birth },
                new SqlParameter("@gender",SqlDbType.Int){Value = model.Gender},
                new SqlParameter("@position", SqlDbType.Int){ Value = model.Position },
                new SqlParameter("@memo", SqlDbType.NVarChar){ Value = model.Memo },
                new SqlParameter("@access", SqlDbType.Date){Value = model.AccessDate.Value.ToString("yyyy-MM-dd")},
                new SqlParameter("@secess", SqlDbType.Date){Value = DBNull.Value  } //2025-03-31 탈퇴 회원이 아님에도 탈퇴일이 기록되어 기본Null로 수정 
            };
            if (model.Status == 2) // 2025-03-31 --> 회원 변경시 탈퇴이라면 탈퇴일자 적용
                parameters[8].Value = model.SecessDate.Value.ToString("yyyy-MM-dd");


            using (SqlConnection connection = OpenSql())
            {
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    ExecuteNoneQuery(StoredProcedures.UpdateMember, connection, transaction, parameters);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("회원 수정 오류 : " + ex.Message);
                }
            }
        }

        /// <summary>
        /// 사용 시작일 조회
        /// </summary>
        /// <returns></returns>
        public string LoadStartDate()
        {
            string query = "SELECT cf_strval FROM config WHERE cf_code = 12";
            return ScalaQuery(query).ToString();
            
        }
    }
}
