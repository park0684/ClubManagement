using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public interface IRecordBoardRegistView
    {
        // === 속성 ===

        /// <summary>
        /// 모임 타이틀
        /// </summary>
        string MatchTitle { get; set; }

        /// <summary>
        /// 순서 수
        /// </summary>
        int OrderCount { get; set; }

        /// <summary>
        /// 게임 리스트
        /// </summary>
        List<GameOrderDto> GameOrderList { get; set; }

        /// <summary>
        /// 플레이어 리스트
        /// </summary>
        List<PlayerInfoDto> PlayerList { get; set; }

        // === 이벤트 ===

        /// <summary>
        /// 게임 추가 실행
        /// </summary>
        event EventHandler AddOrderEvent;

        /// <summary>
        /// 게임 저장 실행
        /// </summary>
        event EventHandler SaveOrderEvent;

        /// <summary>
        /// 폼 닫기 실행
        /// </summary>
        event EventHandler CloseFormEvent;

        /// <summary>
        /// 게임 선택 실행
        /// </summary>
        event EventHandler SelectGameEvent;

        /// <summary>
        /// 플레이어 편집 실행
        /// </summary>
        event EventHandler EditPlayerEvent;

        // === 메서드 ===

        /// <summary>
        /// 게임 로드
        /// </summary>
        /// <param name="orders">게임 순서 리스트</param>
        void LoadOrder(List<GameOrderDto> orders);

        /// <summary>
        /// 플레이어 로드
        /// </summary>
        /// <param name="players">플레이어 리스트</param>
        void LoadPlayer(List<PlayerInfoDto> players);

        /// <summary>
        /// 새 게임 추가
        /// </summary>
        /// <param name="newOrder">새 게임 순서</param>
        void AddNewOrder(GameOrderDto newOrder);

        /// <summary>
        /// 게임 검색
        /// </summary>
        void SearchGame();

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
        /// 모임 버튼 상태 설정
        /// </summary>
        void SetMatchButton();


    }
}
