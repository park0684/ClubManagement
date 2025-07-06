using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Games.Views
{
    public interface INumericPadView
    {
        // === 속성 ===

        /// <summary>
        /// 입력 숫자
        /// </summary>
        int Nember { get; set; }

        // === 이벤트 ===

        /// <summary>
        /// 폼 닫기
        /// </summary>
        event EventHandler CloseFormEvent;

        /// <summary>
        /// 숫자 입력 완료
        /// </summary>
        event EventHandler InsertNumberEvent;

        // === 메서드 ===

        /// <summary>
        /// 폼 닫기
        /// </summary>
        void CloseForm();

        /// <summary>
        /// 폼 표시
        /// </summary>
        void ShowForm();

    }
}
