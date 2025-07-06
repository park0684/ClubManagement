using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ClubManagement.Members.Views
{
    public interface IDuesManageView
    {
        // === 속성 ===

        /// <summary>
        /// 검색어
        /// </summary>
        string SearchWord { get; set; }

        /// <summary>
        /// 조회 시작일
        /// </summary>
        DateTime FromDate { get; set; }

        /// <summary>
        /// 조회 종료일
        /// </summary>
        DateTime ToDate { get; set; }

        /// <summary>
        /// 탈퇴 회원 포함 여부
        /// </summary>
        bool SecessInclude { get; set; }

        /// <summary>
        /// 선택된 회원 코드
        /// </summary>
        int? GetMemberCode { get; }

        /// <summary>
        /// 선택된 전표 코드
        /// </summary>
        int? GetStateMentCode { get; }

        /// <summary>
        /// 전표 유형
        /// </summary>
        int? StateType { get; set; }

        // === 이벤트 ===

        /// <summary>
        /// 회원 검색 이벤트
        /// </summary>
        event EventHandler MemberSearchEvent;

        /// <summary>
        /// 전표 검색 이벤트
        /// </summary>
        event EventHandler StatementSearchEvent;

        /// <summary>
        /// 전표 추가 이벤트
        /// </summary>
        event EventHandler StatementAddEvent;

        /// <summary>
        /// 전표 수정 이벤트
        /// </summary>
        event EventHandler StatementEditEvent;

        // === 메서드 ===

        /// <summary>
        /// 회원 리스트 데이터 바인딩
        /// </summary>
        /// <param name="members">회원 데이터</param>
        void SetMemberListBinding(DataTable members);

        /// <summary>
        /// 전표 리스트 데이터 바인딩
        /// </summary>
        /// <param name="States">전표 데이터</param>
        void SetStateListBinding(DataTable States);

    }
}

