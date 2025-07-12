using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Common.DTOs;

namespace ClubManagement.Members.Views
{
    public interface IMemberScoreDetailView
    {
        /// <summary>
        /// 현재 에버
        /// </summary>
        Decimal Average { set; }

        /// <summary>
        /// 기간내 에버 스코어
        /// </summary>
        Decimal AverageScore { set; }

        /// <summary>
        /// 기간내 최고점
        /// </summary>
        int MaxScore { set; }

        /// <summary>
        /// 기간내 최저점
        /// </summary>
        int MinScore { set; }

        /// <summary>
        /// 기간내 게임 횟수
        /// </summary>
        int GameCount { set; }

        /// <summary>
        /// 회원명
        /// </summary>
        string MemberName { set; }

        /// <summary>
        /// 회원 성별
        /// </summary>
        string MemberGender { set; }

        /// <summary>
        /// 회원 핸디
        /// </summary>
        int MemberHandi { set; }

        /// <summary>
        /// 회원 등급
        /// </summary>
        int MemberGrade { get; set; }


        event EventHandler CloseFormEvent;
        event Action<int> ScoreSearchEvent;
        event EventHandler GradeSaveEvent;

        void ShowForm();
        void CloseForm();
        void ShowMessage(string message);
        void SetComboBoxItems(Dictionary<int, string> items);
        void CreateGameScoreList(List<ScoreDto> scores);
        void SetGraph(List<ScoreDto> scores);
    }
}
