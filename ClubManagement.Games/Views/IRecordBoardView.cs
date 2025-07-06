using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Views
{
    public interface IRecordBoardView
    {
        // === 속성 ===

        /// <summary>
        /// 모임 타이틀
        /// </summary>
        string MatchTitle { set; }

        /// <summary>
        /// 게임 순번
        /// </summary>
        string GameSeq { set; }

        // === 이벤트 ===

        /// <summary>
        /// 게임 버튼 클릭
        /// </summary>
        event Action<int> GameButtonClick;

        /// <summary>
        /// 참가자 추가 클릭
        /// </summary>
        event Action<int, int> AssignPlayerClick;

        /// <summary>
        /// 개인 사이드 게임 설정
        /// </summary>
        event EventHandler SetIndividualSideEvent;

        /// <summary>
        /// 올커버 사이드 게임 설정
        /// </summary>
        event EventHandler AllcoverGameSetEvent;

        /// <summary>
        /// 플레이어 옵션 클릭
        /// </summary>
        event Action<string> PlayerOptionEvent;

        /// <summary>
        /// 점수 입력
        /// </summary>
        event Action<PlayerInfoDto> EnterScoreEvent;

        /// <summary>
        /// 개인 랭크 저장
        /// </summary>
        event EventHandler SaveIndividualRankEvent;

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
        /// 메시지 표시
        /// </summary>
        /// <param name="message">메시지 내용</param>
        void ShowMessage(string message);

        /// <summary>
        /// 전체 플레이어 리스트 설정
        /// </summary>
        void SetAllPlayerList(List<GameOrderDto> groups, List<PlayerInfoDto> players);

        /// <summary>
        /// 개인 사이드 게임 데이터 바인딩
        /// </summary>
        void SetBindingSideGame(List<PlayerInfoDto> players);

        /// <summary>
        /// 사이드 게임 플레이어 리스트 설정
        /// </summary>
        void SetSideGamePlayerList(List<PlayerInfoDto> players);

        /// <summary>
        /// 올커버 게임 플레이어 리스트 설정
        /// </summary>
        void SetAllcoverGamePlayers(List<PlayerInfoDto> players);

        /// <summary>
        /// 그룹 점수 리스트 설정
        /// </summary>
        void SetGroupScoreList(GameOrderDto groups);

        /// <summary>
        /// 게임 그룹 패널 초기화
        /// </summary>
        void flpGameGroupClear();

        /// <summary>
        /// 개인전 플레이어 패널 추가
        /// </summary>
        void AddPlayerPanal(PlayerInfoDto player);

        /// <summary>
        /// 게임 버튼 생성
        /// </summary>
        void CreateGameButton(List<GameOrderDto> games);

        /// <summary>
        /// 올커버 게임 플레이어 로드
        /// </summary>
        void LoadAllcoverGamePlayers(GameOrderDto game);

        /// <summary>
        /// 개인 사이드 랭크 리스트 생성
        /// </summary>
        /// <param name="rank">랭크 값</param>
        /// <returns>개인 플레이어 리스트</returns>
        List<IndividualPlayerDto> SetIndividualSideRank(int rank);

        /// <summary>
        /// 확인 메시지 박스
        /// </summary>
        /// <param name="message">메시지 내용</param>
        /// <returns>Yes: true, No: false</returns>
        bool ShowConfirmation(string message);

        /// <summary>
        /// 개인 점수 데이터 바인딩
        /// </summary>
        void BindingIndividualScore(DataTable players);

        /// <summary>
        /// 개인 게임 그룹 렌더링
        /// </summary>
        void RenderIndividualGameGroups(List<GroupDto> groups);

        /// <summary>
        /// 팀 게임 그룹 렌더링
        /// </summary>
        void RenderTeamGameGroups(List<GroupDto> groups, int gameSeq);

    }
}
