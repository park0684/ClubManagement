using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public interface IRecordBoardPlayerManageView
    {

        // === 이벤트 ===

        /// <summary>
        /// 저장 실행
        /// </summary>
        event EventHandler SaveEvent;

        /// <summary>
        /// 폼 닫기 실행
        /// </summary>
        event EventHandler CloaseEvent;

        /// <summary>
        /// 플레이어 추가 실행
        /// </summary>
        event EventHandler<participantButtonEventArgs> PlayerAddEvent;

        /// <summary>
        /// 플레이어 제거 실행
        /// </summary>
        event EventHandler<participantButtonEventArgs> PlayerRemoveEvent;

        // === 메서드 ===

        /// <summary>
        /// 폼 닫기
        /// </summary>
        void CloseForm();

        /// <summary>
        /// 폼 표시
        /// </summary>
        void ShowForm();

        /// <summary>
        /// 메시지 박스 표시
        /// </summary>
        /// <param name="message">표시할 메시지</param>
        void ShowMessage(string message);

        /// <summary>
        /// 플레이어 버튼 생성
        /// </summary>
        /// <param name="gameOrder">게임 순서 정보</param>
        /// <param name="groupNumber">그룹 번호</param>
        /// <param name="players">플레이어 리스트</param>
        void CreatePlayerButton(GameOrderDto gameOrder, int groupNumber, List<PlayerInfoDto> players);

        /// <summary>
        /// 참가자 버튼 생성
        /// </summary>
        /// <param name="participant">참가자 리스트</param>
        void CreateAttendButton(List<PlayerInfoDto> participant);

        /// <summary>
        /// 플레이어 버튼 색상 업데이트
        /// </summary>
        /// <param name="player">플레이어 이름</param>
        /// <param name="isCurrentGroup">현재 그룹 여부</param>
        /// <param name="isSelected">

    }
}
