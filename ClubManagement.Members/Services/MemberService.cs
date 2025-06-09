using ClubManagement.Common.Hlepers;
using ClubManagement.Members.Models;
using ClubManagement.Members.Repositories;
using System;
using System.Data;

namespace ClubManagement.Members.Services
{
    public class MemberService : IMemberService
    {
        IMemberRepository _reposotory;
        public MemberService(IMemberRepository repository)
        {
            _reposotory = repository;
        }

        /// <summary>
        /// 프로그램 사용 시작일 조회
        /// 해당 정보로 회원의 회비 및 참석율의 계산 범위 지정
        /// </summary>
        /// <returns></returns>
        public string LoadStartDate()
        {
            return _reposotory.LoadStartDate();
        }

        /// <summary>
        /// 회원 정보 조회
        /// MemberListView에서 검색한 회원 정보 조회 및 데이터 가공
        /// </summary>
        /// <param name="model">회원 검색 조건</param>
        /// <returns>가공된 회원 목록 DataTable</returns>
        public DataTable LoadMemberList(MemberSearchModel model)
        {
            // DB에서 회원 목록을 조회
            var result = _reposotory.LodaMemberList(model);

            int i = 1; //출력용 No(순번)

            // 출력용 추가 컬럼
            result.Columns.Add("No");
            result.Columns.Add("status");
            result.Columns.Add("position");
            result.Columns.Add("gender");
            result.Columns.Add("regularRate", typeof(double));
            result.Columns.Add("nonPayment");

            // 각 행 데이터 가공
            foreach (DataRow row in result.Rows)
            {
                row["No"] = i++; // 순번 지정

                // 상태 코드 -> 상태명 변환
                row["status"] = MemberHelper.GetMemberStatus(Convert.ToInt32(row["mem_status"]));

                // 성별 코드 -> 성별명 변환
                row["gender"] = MemberHelper.GetMemberGenger(Convert.ToInt32(row["mem_gender"]));

                // 직책 코드 -> 직책명 변환
                row["position"] = MemberHelper.GetMemberPositon(Convert.ToInt32(row["mem_position"]));

                // 미납 계산 (납부해야 할 금액 - 납부한 금액)
                int nonPayment = Convert.ToInt32(row["mem_dues"]) - Convert.ToInt32(row["Payment"]);
                row["nonPayment"] = nonPayment < 0 ? 0 : nonPayment;

                // 참석율 (반올림 처리)
                double rate = Math.Round(Convert.ToDouble(row["regular_rate"])*100);
                row["regularRate"] = rate;
            }

            // 화면에 불필요한 원본 코드 컬럼 제거
            result.Columns.Remove("mem_status");
            result.Columns.Remove("mem_gender");
            result.Columns.Remove("mem_position");
            result.Columns.Remove("game_count");
            result.Columns.Remove("mem_dues");

            // 데이터 변경 사항 확정 (AcceptChanges)
            result.AcceptChanges();

            return result;
        }

        /// <summary>
        /// 회원 검색 조건에 따라 DB에서 회원 목록을 조회.
        /// 별도 가공 없이 DataTable 그대로 반환.
        /// </summary>
        /// <param name="model">회원 검색 조건 모델</param>
        /// <returns>조회된 회원 목록 DataTable</returns>
        public DataTable LoadSearchMember(MemberSearchModel model)
        {
            var resutl = _reposotory.LoadSearchMember(model);
            return resutl;
        }

        /// <summary>
        /// 회원 코드로 단일 회원 정보를 조회하고 MemberModel로 매핑.
        /// </summary>
        /// <param name="memberCode">회원 코드 (PK)</param>
        /// <returns>조회된 회원 정보를 담은 MemberModel</returns>
        public MemberModel LoadMemberInfo(int memberCode)
        {
            // DB에서 단일 회원 정보 (DataRow) 조회
            var result = _reposotory.LoadMemberInfo(memberCode);

            // MemberModel 인스턴스로 변환 및 값 설정
            return new MemberModel
            {
                IsNew = false, // 기존 회원 데이터임을 표시
                Code = memberCode, // 회원 코드
                MemberName = result["mem_name"].ToString().Trim(), // 이름 (공백 제거)
                Status = Convert.ToInt32(result["mem_status"]), // 상태 코드
                Gender = Convert.ToInt32(result["mem_gender"]), // 성별 코드
                Position = Convert.ToInt32(result["mem_position"]), // 직책 코드
                Birth = result["mem_birth"].ToString().Trim(), // 생년 (문자열, 공백 제거)
                Memo = result["mem_memo"].ToString().Trim(), // 메모 (공백 제거)
                AccessDate = (DateTime)result["mem_access"],  // 가입일
                SecessDate = result["mem_secess"] == DBNull.Value ? DateTime.Now : (DateTime)result["mem_secess"] // 탈퇴일: NULL이면 현재 시각 반환 (또는 다른 기본값 지정 가능)
            };
        }

        /// <summary>
        /// MemberModel 데이터를 저장 (신규 등록 또는 기존 회원 정보 수정).
        /// </summary>
        /// <param name="model">저장할 회원 정보 모델</param>
        public void SaveMember(MemberModel model)
        {
            // 모델의 IsNew 값에 따라 Insert 또는 Update 실행
            if (model.IsNew)
                // 신규 회원인 경우: DB에 새 회원 등록
                _reposotory.InsertMember(model);
            else
                // 기존 회원인 경우: DB에 회원 정보 수정
                _reposotory.UpdateMember(model);
        }
    }
}
