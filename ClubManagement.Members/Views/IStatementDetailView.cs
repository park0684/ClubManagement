using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Members.Views
{
    public interface IStatementDetailView
    {
        // === 속성 ===

        /// <summary>
        /// 전표 날짜
        /// </summary>
        DateTime StatementDate { get; set; }

        /// <summary>
        /// 전표 유형
        /// </summary>
        int? StatementType { get; set; }

        /// <summary>
        /// 회비 적용 개월 수
        /// </summary>
        int DueCount { get; set; }

        /// <summary>
        /// 회비 금액
        /// </summary>
        int? DueAmount { get; set; }

        /// <summary>
        /// 출금 금액
        /// </summary>
        int? Withdrawal { get; set; }

        /// <summary>
        /// 적용 회비
        /// </summary>
        int? Apply { get; set; }

        /// <summary>
        /// 회원 이름
        /// </summary>
        string MemberName { get; set; }

        /// <summary>
        /// 출금 상세 내역
        /// </summary>
        string WithdrawalDetail { get; set; }

        /// <summary>
        /// 메모
        /// </summary>
        string Memo { get; set; }

        /// <summary>
        /// 출금 여부
        /// </summary>
        bool IsWithdrawal { get; }

        // === 이벤트 ===

        /// <summary>
        /// 저장 실행
        /// </summary>
        event EventHandler SaveEvent;

        /// <summary>
        /// 종료 실행
        /// </summary>
        event EventHandler CloseEvent;

        /// <summary>
        /// 회원 선택 실행
        /// </summary>
        event EventHandler SelectMemberEvent;

        /// <summary>
        /// 전표 삭제 실행
        /// </summary>
        event EventHandler DeleteEvent;

        /// <summary>
        /// 유형 변경 실행
        /// </summary>
        event EventHandler TypeChaingedEvnet;

        /// <summary>
        /// 회비 금액 변경 실행
        /// </summary>
        event EventHandler DuesAmountChaingedEvent;

        // === 메서드 ===

        /// <summary>
        /// 폼 종료
        /// </summary>
        void CloseForm();

        /// <summary>
        /// 폼 실행
        /// </summary>
        void ShowForm();

        /// <summary>
        /// 삭제 버튼 표시
        /// </summary>
        void SetDeleteButtonVisivle();

        /// <summary>
        /// 유형 변경 UI 처리
        /// </summary>
        void TypeChaingedSet();

        /// <summary>
        /// 적용 개월 수 표시
        /// </summary>
        /// <param name="counter">적용 개월 수</param>
        void SetApplyCounter(int counter);

        /// <summary>
        /// 메시지 표시
        /// </summary>
        /// <param name="message">메시지 내용</param>
        void ShowMessage(string message);

    }
}
