using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ClubManagement.Games.Views
{
    public interface ISearchMatchListView
    {
        // === 속성 ===

        /// <summary>
        /// 조회 시작일
        /// </summary>
        DateTime FromDate { get; set; }

        /// <summary>
        /// 조회 종료일
        /// </summary>
        DateTime ToDate { get; set; }

        /// <summary>
        /// 선택된 모임 코드
        /// </summary>
        int MatchCode { get; }

        /// <summary>
        /// 선택된 모임 타이틀
        /// </summary>
        string MatchTitle { get; }

        /// <summary>
        /// 기록 등록 여부
        /// </summary>
        bool IsRecodeRegisted { get; }

        // === 이벤트 ===

        /// <summary>
        /// 폼 닫기 이벤트
        /// </summary>
        event EventHandler CloseFormEvent;

        /// <summary>
        /// 모임 검색 이벤트
        /// </summary>
        event EventHandler SearchMatchEvent;

        /// <summary>
        /// 모임 선택 이벤트
        /// </summary>
        event EventHandler SelectedMatchEvent;

        // === 메서드 ===

        /// <summary>
        /// 데이터 바인딩
        /// </summary>
        /// <param name="source">바인딩할 데이터</param>
        void SetDataBinding(DataTable source);

        /// <summary>
        /// 메시지 표시
        /// </summary>
        /// <param name="message">메시지 내용</param>
        void ShowMessage(string message);

        /// <summary>
        /// 폼 표시
        /// </summary>
        void ShowForm();

        /// <summary>
        /// 폼 닫기
        /// </summary>
        void CloseForm();

    }
}
