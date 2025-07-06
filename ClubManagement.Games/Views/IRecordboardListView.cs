using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace ClubManagement.Games.Views
{
    public interface IRecordboardListView
    {

        /// <summary>
        /// 조회 기간 시작일
        /// </summary>
        DateTime FromDate { get; set; }

        /// <summary>
        /// 조회 기간 종료일
        /// </summary>
        DateTime ToDate { get; set; }

        /// <summary>
        /// 선택된 모임 코드
        /// </summary>
        int? GetMatchCode { get; }


        /// <summary>
        /// 게임 기록 등록 이벤트
        /// </summary>
        event EventHandler RecordBoardRegistEvent;

        /// <summary>
        /// 게임 기록 설정 수정 이벤트
        /// </summary>
        event EventHandler RecordBoardEditEvent;

        /// <summary>
        /// 게임 기록 선택 이벤트
        /// </summary>
        event EventHandler RecordBoarSelectedEvent;

        /// <summary>
        /// 모임 검색창 실행 이벤트
        /// </summary>
        event EventHandler SearchRecordBoardEvnt;

        void SetDataBinding(DataTable resource);
        void ShowMessage(string message);
    }
}
