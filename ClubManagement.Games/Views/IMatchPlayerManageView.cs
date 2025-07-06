using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public interface IMatchPlayerManageView
    {
        // === 속성 ===

        /// <summary>
        /// 검색어
        /// </summary>
        string SearchWord { get; set; }

        /// <summary>
        /// 탈퇴회원 포함 여부
        /// </summary>
        bool SecessMember { get; set; }

        // === 이벤트 ===

        /// <summary>
        /// 회원 검색
        /// </summary>
        event EventHandler SearchMemberEvent;

        /// <summary>
        /// 플레이어 추가
        /// </summary>
        event EventHandler<PlayerButtonEventArgs> PlayerAddEvent;

        /// <summary>
        /// 플레이어 제거
        /// </summary>
        event EventHandler<PlayerButtonEventArgs> PlayerRemoveEvent;

        /// <summary>
        /// 플레이어 리스트 저장
        /// </summary>
        event EventHandler SavePlayerListEvent;

        /// <summary>
        /// 폼 종료
        /// </summary>
        event EventHandler CloseEvent;

        /// <summary>
        /// 게스트 추가
        /// </summary>
        event EventHandler AddGuestEvent;

        // === 메서드 ===

        /// <summary>
        /// 버튼 색상 갱신
        /// </summary>
        void UpdateButtonColor(int memberCode, bool isAdded);

        /// <summary>
        /// 회원 리스트 세팅
        /// </summary>
        void SetMemberList(List<PlayerInfoDto> members);

        /// <summary>
        /// 플레이어 리스트 세팅
        /// </summary>
        void SetPlayerList(List<PlayerInfoDto> players);

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
        void ShowMessage(string message);

    }
}
