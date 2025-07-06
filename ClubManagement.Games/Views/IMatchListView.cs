using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ClubManagement.Games.Views
{
    public interface IMatchListView
    {
        // === 속성 ===

        /// <summary>
        /// 조회 기간 시작일
        /// </summary>
        DateTime MatchFromDate { get; set; }

        /// <summary>
        /// 조회 기간 종료일
        /// </summary>
        DateTime MatchToDate { get; set; }

        /// <summary>
        /// 모임 유형
        /// </summary>
        int? MatchType { get; set; }

        /// <summary>
        /// 선택 유형 제외
        /// </summary>
        bool ExcludeType { get; set; }

        /// <summary>
        /// 모임 코드
        /// </summary>
        int? GetMatchCode { get; }

        /// <summary>
        /// 모임 타이틀
        /// </summary>
        string MatchTile { get; }

        // ===이벤트===

        /// <summary>
        /// 모임 검색 실행
        /// </summary>
        event EventHandler SearchMatchEvent;

        /// <summary>
        /// 새 모임등록 실행
        /// </summary>
        event EventHandler AddMatchEvent;

        /// <summary>
        /// 모임 설정 정보 수정 실행
        /// </summary>
        event EventHandler EditMatchEvent;

        /// <summary>
        /// 참가자 조회 
        /// </summary>
        event EventHandler SearchPlayerEvent;

        /// <summary>
        /// 참가자 수정
        /// </summary>
        event EventHandler EditPlayerEvent;

        // === 메서드 ===

        /// <summary>
        /// 모임리스트 정보 바인딩
        /// </summary>
        /// <param name="source"></param>
        void SetMatchListBinding(DataTable source);

        /// <summary>
        /// 참가자 리스트 정보 바인딩
        /// </summary>
        /// <param name="source"></param>
        void SetPlayerListBinding(DataTable source);

        /// <summary>
        /// 메시지 박스 표시
        /// </summary>
        /// <param name="message"></param>
        void ShowMessage(string message);

        /// <summary>
        /// 폼 실행
        /// </summary>
        void ShowForm();

        /// <summary>
        /// 폼 종료
        /// </summary>
        void CloseForm();
    }
}
