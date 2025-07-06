using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;
namespace ClubManagement.Games.Views
{
    public interface IPlayerOptionView
    {
        // === 속성 ===

        /// <summary>
        /// 업데이트된 플레이어 정보
        /// </summary>
        PlayerInfoDto UpdatePlayer { get; }

        // === 이벤트 ===

        /// <summary>
        /// 저장 실행
        /// </summary>
        event EventHandler SaveEvent;

        /// <summary>
        /// 폼 닫기 실행
        /// </summary>
        event EventHandler CloseEvent;

        // === 메서드 ===

        /// <summary>
        /// 폼 표시
        /// </summary>
        void ShowForm();

        /// <summary>
        /// 폼 닫기
        /// </summary>
        void CloseForm();

        /// <summary>
        /// 메시지 박스 표시
        /// </summary>
        /// <param name="message">표시할 메시지</param>
        void ShowMessage(string message);

        /// <summary>
        /// 플레이어 옵션 설정
        /// </summary>
        /// <param name="player">플레이어 정보</param>
        void SetPlayerOption(PlayerInfoDto player);

    }
}
