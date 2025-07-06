using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Games.Views
{
    public interface IIndividualSideSetView
    {

        // ===속성===
        
        /// <summary>
        /// 1등 보상금
        /// </summary>
        int? Prize1st { get; set; }
        /// <summary>
        /// 2등 보상금
        /// </summary>
        int? Prize2nd { get; set; }
        /// <summary>
        /// 3등 보상금
        /// </summary>
        int? Prize3rd { get; set; }
        /// <summary>
        /// 1등 핸디캡 점수
        /// </summary>
        int? Handi1st { get; set; }
        /// <summary>
        /// 2등 핸디갭 점수
        /// </summary>
        int? Handi2nd { get; set; }
        /// <summary>
        /// 3등 핸디캡 점수
        /// </summary>
        int? Handi3rd { get; set; }

        // === 이벤트 ===

        /// <summary>
        /// 보상 및 핸디캡 설정 저장
        /// </summary>
        event EventHandler SaveEvent;
        /// <summary>
        /// 폼 종료
        /// </summary>
        event EventHandler CloseEvent;

        // === 메서드 ===

        /// <summary>
        /// 개인전 보상 및 핸디캡 설정창 폼 실해
        /// </summary>
        void ShowForm();
        /// <summary>
        /// 개인전 보상 및 핸디캡 설정창 폼 종료
        /// </summary>
        void CloseForm();
        /// <summary>
        /// 메세지 박스 표시
        /// </summary>
        /// <param name="message"></param>
        void ShowMessage(string message);


    }
}
