using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Members.Views
{
    public interface IMemberDetailView
    {
        // === 속성 ===

        /// <summary>
        /// 회원명
        /// </summary>
        string MemberName { get; set; }

        /// <summary>
        /// 생년
        /// </summary>
        string Birth { get; set; }

        /// <summary>
        /// 메모
        /// </summary>
        string Memo { get; set; }

        /// <summary>
        /// 상태
        /// </summary>
        int? Status { get; set; }

        /// <summary>
        /// 직위
        /// </summary>
        int? Position { get; set; }

        /// <summary>
        /// 성별
        /// </summary>
        int? Gender { get; set; }

        /// <summary>
        /// 가입일
        /// </summary>
        DateTime? AccessDate { get; set; }

        /// <summary>
        /// 탈퇴일
        /// </summary>
        DateTime? SecessDate { get; set; }

        /// <summary>
        /// 프로 여부
        /// </summary>
        bool IsPro { get; set; }

        // === 이벤트 ===

        /// <summary>
        /// 저장 실행
        /// </summary>
        event EventHandler SaveEvent;

        /// <summary>
        /// 폼 종료 이벤트
        /// </summary>
        event EventHandler CloseFormEvent;

        // === 메서드 ===

        /// <summary>
        /// 폼 종료
        /// </summary>
        void CloseForm();

        /// <summary>
        /// 메시지 표시
        /// </summary>
        /// <param name="message">메시지</param>
        void ShowMessage(string message);

        /// <summary>
        /// 폼 실행
        /// </summary>
        void ShowForm();

    }
}
