using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Members.Views
{
    public interface IMemberScoreView
    {
        /// <summary>
        /// 조회 기간 시작일
        /// </summary>
        DateTime FromDate { get; }

        /// <summary>
        /// 조회 기간 종료일
        /// </summary>
        DateTime ToDate { get; }

        /// <summary>
        /// 회원 상태
        /// </summary>
        int Status { get;  }

        /// <summary>
        /// 정렬 유형
        /// </summary>
        int SortType { get; }

        /// <summary>
        /// 검색어
        /// </summary>
        string SearchWord { get; }

        /// <summary>
        /// 모임 타이틀
        /// </summary>
        string MatchTitel { set; }

        /// <summary>
        /// 모임 조회 여부
        /// </summary>
        bool IsMatch { get; }

        /// <summary>
        /// 지정 회원 제외 여부
        /// </summary>
        bool ExculuedMember { get; }
        //=== 이벤트 ===
        /// <summary>
        /// 검색 실행
        /// </summary>
        event EventHandler SearchScoreEvent;

        /// <summary>
        /// 회원 선택
        /// </summary>
        event Action<int> MemberSelectedEvent;
        event EventHandler MatachSearchEvent;
        event EventHandler CheckedEvent;
        event EventHandler<List<int>> GradeUpdateEvet;
        // === 메서드 ===
        /// <summary>
        /// 메시지 박스 표시
        /// </summary>
        /// <param name="messaga"></param>
        void ShowMessga(string messaga);

        /// <summary>
        /// 데이터 그리드 뷰 칼럼 설정
        /// </summary>
        /// <param name="columns">칼럼 항목</param>
        void SetDatagirColumns(Dictionary<string,  string> columns);

        /// <summary>
        /// 콤보박스 항목 설정
        /// </summary>
        /// <param name="items">컬럼 항목</param>
        void SetComboboxItem(Dictionary<string, Dictionary<int, string>> items);

        /// <summary>
        /// 회원 점수 정보 조회
        /// </summary>
        /// <param name="result">회원 점수 정보</param>
        void MemberScoreBinding(DataTable result);


    }
}
