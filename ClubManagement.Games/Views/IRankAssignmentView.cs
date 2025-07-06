using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public interface IRankAssignmentView
    {
        // === 속성 ===

        /// <summary>
        /// 개인전 랭커 리스트
        /// </summary>
        List<IndividualPlayerDto> IndividaulRankers { get; set; }

        /// <summary>
        /// 개인전 세트 리스트
        /// </summary>
        List<IndividaulSetDto> IndividaulSets { get; set; }

        // === 이벤트 ===

        /// <summary>
        /// 저장 실행
        /// </summary>
        event EventHandler SaveEvent;

        /// <summary>
        /// 폼 닫기 실행
        /// </summary>
        event EventHandler CloseEvent;

        /// <summary>
        /// 핸디 수정 실행
        /// </summary>
        event EventHandler<HandiEditEventArgs> EditHandiEvent;

        /// <summary>
        /// 랭크 수정 실행
        /// </summary>
        event EventHandler EidtRankEvent;

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
        /// 플레이어 패널 추가
        /// </summary>
        void AddPlayerPanel();

        /// <summary>
        /// 확인 메시지 표시
        /// </summary>
        /// <param name="message">확인할 메시지</param>
        /// <returns>Yes/No 여부</returns>
        bool ShowConfirmation(string message);

        //void EidtHendi(int handi);
    }
}
