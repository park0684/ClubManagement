using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public interface IGuestAddView
    {
        /// <summary>
        /// 게스트 이름
        /// </summary>
        string GuestName { get; set; }

        /// <summary>
        /// 게스트 정보 Dto
        /// </summary>
        /// <returns></returns>
        List<PlayerInfoDto> GusetData();

        /*이벤트*/
        /// <summary>
        /// 게스트 등록창에 새로운 게스트 패널 추가
        /// </summary>
        event EventHandler AddGuestEvent;
        /// <summary>
        /// 게스트 등록창 정보 저장
        /// </summary>
        event EventHandler SaveGuestEvent;
        /// <summary>
        /// 폼 종료
        /// </summary>
        event EventHandler CloseFormEvevnt;

        /// <summary>
        /// 폼 종료
        /// </summary>
        void CloseForm();
        /// <summary>
        /// 게스트 패널 추가 등록
        /// </summary>
        void AddGuestPanel();
        /// <summary>
        /// 게스트 추가 후 이름 입력 텍스트 박스 초기화
        /// </summary>
        void TextBoxClear();

        /// <summary>
        /// 폼 실행
        /// </summary>
        void ShowForm();

        /// <summary>
        /// 메세지 박스 표시
        /// </summary>
        /// <param name="message">오류메시지</param>
        void ShowMessage(string message);

    }
}
