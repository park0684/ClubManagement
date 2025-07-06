using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ClubManagement.Members.Views
{
    public interface IMemberSearchView
    {
        // === 속성 ===

        /// <summary>
        /// 검색어
        /// </summary>
        string SearchWord { get; set; }

        /// <summary>
        /// 선택한 회원 코드
        /// </summary>
        int MemberCode { get; }

        /// <summary>
        /// 탈퇴회원 포함 여부
        /// </summary>
        bool IsInculde { get; set; }

        /// <summary>
        /// 선택한 회원 이름
        /// </summary>
        string MemberName { get; }

        // === 이벤트 ===

        /// <summary>
        /// 회원 검색 실행
        /// </summary>
        event EventHandler MemberSeachEvent;

        /// <summary>
        /// 회원 선택 실행
        /// </summary>
        event EventHandler SelectMemberEvent;

        /// <summary>
        /// 폼 종료 실행
        /// </summary>
        event EventHandler CloseFormEvent;

        // === 메서드 ===

        /// <summary>
        /// 폼 종료
        /// </summary>
        void CloseForm();

        /// <summary>
        /// 회원 리스트 바인딩
        /// </summary>
        /// <param name="members">회원 데이터</param>
        void SetMemberList(DataTable members);

        /// <summary>
        /// 폼 실행
        /// </summary>
        void ShowForm();

        /// <summary>
        /// 메시지 표시
        /// </summary>
        /// <param name="message">메시지 내용</param>
        void ShowMessage(string message);


    }
}

