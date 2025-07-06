using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Members.DTOs;

namespace ClubManagement.Members.Views
{
    public interface IMemberListView
    {
        // === 속성 ===

        /// <summary>
        /// 검색어
        /// </summary>
        string SearchWord { get; set; }

        /// <summary>
        /// 상태 코드
        /// </summary>
        int? Status { get; set; }

        /// <summary>
        /// 가입일 조회 시작일
        /// </summary>
        DateTime? AccFromDate { get; set; }

        /// <summary>
        /// 가입일 조회 종료일
        /// </summary>
        DateTime? AccToDate { get; set; }

        /// <summary>
        /// 탈퇴일 조회 시작일
        /// </summary>
        DateTime? SecFromDate { get; set; }

        /// <summary>
        /// 탈퇴일 조호 ㅣ종료일
        /// </summary>
        DateTime? SecToDate { get; set; }

        /// <summary>
        /// 참가일 조회 시작일
        /// </summary>
        DateTime? GameFromDate { get; set; }

        /// <summary>
        /// 정기전 제외 여부
        /// </summary>
        bool ExcludeRegular { get; set; }

        /// <summary>
        /// 비정기전 제외 여부
        /// </summary>
        bool ExcludeIrregular { get; set; }

        /// <summary>
        /// 이벤트전 제외 여부
        /// </summary>
        bool ExcludeEvent { get; set; }

        /// <summary>
        /// 지정 회원 유형 제외 여부
        /// </summary>
        bool ExcludeMember { get; set; }

        /// <summary>
        /// 가입일 조회 체크 여부
        /// </summary>
        bool AccessCheck { get; set; }

        /// <summary>
        /// 탈퇴일 조회 체크 여부
        /// </summary>
        bool SecessCheck { get; set; }

        /// <summary>
        /// 참가일 조회 체크 여부
        /// </summary>
        bool GameCheck { get; set; }

        /// <summary>
        /// 선택 회원 코드
        /// </summary>
        int? SelectedCode { get; }

        /// <summary>
        /// 정렬 방식
        /// </summary>
        int? SortType { get; }

        /// <summary>
        /// 참가자 포함 설정
        /// </summary>
        int? AttendInclude { get; }

        // === 이벤트 ===

        /// <summary>
        /// 검색 이벤트
        /// </summary>
        event EventHandler SearchEvent;

        /// <summary>
        /// 회원 추가 이벤트
        /// </summary>
        event EventHandler AddMemberEvent;

        /// <summary>
        /// 회원 수정 이벤트
        /// </summary>
        event EventHandler EidtMemberEvent;

        // === 메서드 ===

        /// <summary>
        /// 메시지 표시
        /// </summary>
        /// <param name="message">메시지</param>
        void ShowMessage(string message);

        /// <summary>
        /// 회원 목록 바인딩
        /// </summary>
        /// <param name="members">회원 데이터</param>
        void MemberListBinding(DataTable members);
    }
}
