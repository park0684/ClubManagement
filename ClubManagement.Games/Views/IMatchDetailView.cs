using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Games.Views
{
    public interface IMatchDetailView
    {
        // === 속성 ===

        /// <summary>
        /// 모임 타이틀
        /// </summary>
        string MatchTitle { get; set; }

        /// <summary>
        /// 주체자 이름
        /// </summary>
        string HostName { get; set; }
        
        /// <summary>
        /// 모임 메모
        /// </summary>
        string MatchMemo { get; set; }

        /// <summary>
        /// 모임 날짜
        /// </summary>
        DateTime MatchDate { get; set; }

        /// <summary>
        /// 모임 유형
        /// </summary>
        int MatchType { get; set; }

        // === 이벤트 ===

        /// <summary>
        /// 모임 저장 실행
        /// </summary>
        event EventHandler SaveEvent;

        /// <summary>
        /// 모임 종료 실행
        /// </summary>
        event EventHandler CloseEvenvt;


        // === 메서드 ===

        /// <summary>
        /// 모임 등록정보창 폼 종료
        /// </summary>
        void CloseForm();
        
        /// <summary>
        /// 모임 등록정보창 폼 실행
        /// </summary>
        void ShowForm();

        /// <summary>
        /// 메시지 박스 표시
        /// </summary>
        /// <param name="message"></param>
        void ShowMessage(string message);
    }
}
