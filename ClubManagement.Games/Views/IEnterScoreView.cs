using System;

namespace ClubManagement.Games.Views
{
    public interface IEnterScoreView
    {
        /// <summary>
        /// 플레이어 점수
        /// </summary>
        int? Score { get; set; }

        /// <summary>
        /// 플레이어 핸디캡 점수
        /// </summary>
        int? Handi { get; set; }

        /// <summary>
        /// 플레이어 점수 + 핸디캡 점수
        /// </summary>
        int? TotalScore { get; set; }

        /// <summary>
        /// 플레이어 이름
        /// </summary>
        string PlayerName { get;  set; }

        /// <summary>
        /// 프로여부
        /// </summary>
        bool IsPerfect { get; set; }

        /// <summary>
        /// 올커버 여부 
        /// 사이드 게임 올카버 참석여부가 아닌 올커버 플레이
        /// </summary>
        bool IsAllcover { get; set; }

        /*이벤트*/

        /// <summary>
        /// 점수 기록 실행
        /// </summary>
        event EventHandler EnterScoreEvent;
        /// <summary>
        /// 퍼펙트 기록
        /// </summary>
        event EventHandler IsPerFectEvent;
        /// <summary>
        /// 올커버 기록
        /// </summary>
        event EventHandler IsAllcoverEvent;
        /// <summary>
        /// 폼 종료
        /// </summary>
        event EventHandler CloseFormEvent;

        /*메서드*/

        /// <summary>
        /// 폼 실행
        /// </summary>
        void ShowForm();

        /// <summary>
        /// 폼 종료
        /// </summary>
        void CloseForm();

        /// <summary>
        /// 메세지 박스 표시
        /// </summary>
        /// <param name="message"></param>
        void ShowMessage(string message);

    }
}
