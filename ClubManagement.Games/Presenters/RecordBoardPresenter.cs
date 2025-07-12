using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Common.Hlepers;
using ClubManagement.Games.DTOs;
using ClubManagement.Games.Models;
using ClubManagement.Games.Repositories;
using ClubManagement.Games.Views;
using ClubManagement.Games.Service;

namespace ClubManagement.Games.Presenters
{
    public class RecordBoardPresenter
    {
        IRecordBoardView _view;
        IRecordBoardRepository _repository;
        RecordBoardService _service;
        RecordBoardModel _model;
        int _gameSeq = 0;
        public RecordBoardPresenter(IRecordBoardView view, IRecordBoardRepository repository)
        {
            _view = view;
            _repository = repository;
            _model = new RecordBoardModel();
            _service = new RecordBoardService(_repository);
            _view.PlayerOptionEvent += SetPlayerOption;
            _view.GameButtonClick += ShowGameGroup;
            _view.AssignPlayerClick += ParticipantRegist;
            _view.EnterScoreEvent += EnterPlayerScore;
            _view.SetIndividualSideEvent += SetIndividualSide;
            _view.SaveIndividualRankEvent += SaveIndividualSideRank;
            _model.CurrentGame = 1;
            _model.CurrentGroup = 0;
        }

        



        /// <summary>
        /// 모임(경기) 정보 수신 후 그룹 및 참가자 목록 생성
        /// 설정한 참가자/팀 수 만큼 그룹 자동 생성
        /// </summary>
        /// <param name="matchCode"></param>
        public void GetMatchInfo(int matchCode)
        {
            // 모임내 게임리스트 설정 후 
            //각 게임별 참여자 및 그룹 설정
            _model = _service.LoadRecodeDate(matchCode, _model.CurrentGame);
            _view.MatchTitle = _repository.LoadMatchTitle(matchCode).ToString();
            _view.CreateGameButton(_model.GameList); // 게임 조회 버튼 생성
            _view.SetAllPlayerList(_model.GameList, _model.PlayerList); // 전체 참가자 리스트 생성 및 데이터 등록            

            ShowGameGroup(_model.CurrentGame);
        }



        /// <summary>
        /// 게임 버튼 클릭 이벤트 실행시 해당 게임의 그룹별 플레이어 정보를 표시
        /// 20250526 Presenter에서 뷰를 직접 생성하는 부분에 대해서
        /// View에 대상들만 전달하고 직접 반복생성하는 메소드 추가
        /// </summary>
        /// <param name="gameSeq"></param>
        private void ShowGameGroup(int gameSeq)
        {
            _model.CurrentGame = gameSeq;
            var selectedGame = _model.GameList.FirstOrDefault(g => g.GameSeq == gameSeq);
            if (selectedGame == null)
                return;
            _gameSeq = gameSeq;
            _view.flpGameGroupClear();
            if(selectedGame.GameType == 1)
            {
                int registeredCount = selectedGame.Groups.Sum(g => g.players.Count);
                
                if( registeredCount == 0)
                    InsertIndividualPlayer();//등록된 플레이어가 없을 경우 직접 등록
                    //ToDo -> 참석자가 수정되었을 경우 등록은 되었으나 플레이어 수가 맞지않음
                    //플레이어 숫자와 참석자 숫자로 대체 필요하며
                    //Presenter에서 플레이어 등록이 아닌 Service에서 처리 하도록 수정 필요

                _view.RenderIndividualGameGroups(selectedGame.Groups);
                //foreach (var targetGroup in selectedGame.Groups)
                //{
                //    foreach (var player in targetGroup.players)
                //    {
                //        _view.AddPlayerPanal(player);
                //    }
                //} 
            }
            else
            {
                //단체전의 경우 그룹 패널 생성
                _view.RenderTeamGameGroups(selectedGame.Groups, gameSeq);
                //foreach (var group in selectedGame.Groups)
                //{
                //    _view.CreateGroupPanal(group, gameSeq);
                //}
            }
            _view.GameSeq = $"{gameSeq} 게임 점수";
            
            SetSideGameGroupBox();
            _view.SetGroupScoreList(selectedGame);
        }
        

        /// <summary>
        /// 개인전 플레이어 자동 등록
        /// 처음 개인전 조회시 미등록 되었을 경우에만 실행
        /// </summary>
        private void InsertIndividualPlayer()
        {
            var targetGame = _model.GameList.FirstOrDefault(g => g.GameSeq == _model.CurrentGame);
            int groupNumber = 0;
            foreach (var player in _model.PlayerList)
            {
                groupNumber++;
                var group = targetGame.Groups.FirstOrDefault(g => g.GroupNumber == groupNumber);
                _model.CurrentGroup = groupNumber;
                group.players.Add(player);
                //_view.AddPlayerPanal(player);
                _repository.InsertGamePlayer(_model);
            }
            //Todo 참가자가 변경 되었을 경우 재등록 기능 필요 
        }

       
        /// <summary>
        /// 참가자 정보가 수정되었다면 model의 값 수정
        /// </summary>
        /// <param name="player"></param>
        private void UpdateInfo(PlayerInfoDto player)
        {
            var targetPlayer = _model.PlayerList.FirstOrDefault(p => p.PlayerName == player.PlayerName);
            
            //2025-07-16 view에서 핸디캡 속성 추가하여 토글 이벤트 또는 텍스트 박스 직접 입력값이 player.Handycap에 반영되도록 수정
            //player.Handycap = (player.Gender ? 15 : 0) + (player.IsPro ? -5 : 0);


            if (targetPlayer != null)
            {
                // 수정된 값 반영
                targetPlayer.Gender = player.Gender;
                targetPlayer.IsPro = player.IsPro;
                targetPlayer.IndividualSide = player.IndividualSide;
                targetPlayer.AllCoverSide = player.AllCoverSide;
                targetPlayer.Handycap = player.Handycap;
            }
            try
            {
                _service.UpdatePlayerOption(targetPlayer, _model.MatchCode);
                GetMatchInfo(_model.MatchCode);
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        /// <summary>
        /// 변경된 PlayerList 정보로 각 Side게임 DataGridView 화면 갱신
        /// </summary>
        private void SetSideGameGroupBox()
        {

            //개인 사이드 플레이어 생성
            var individualSidePalyer = _model.PlayerList.Where(p => p.IndividualSide).ToList();

            //개인 사이드 게임 플레이어 DataGridView에 표시
            _view.SetSideGamePlayerList(individualSidePalyer);
            
            //개인 사이드 핸디, 점수, 랭크 계산
            var scoreList = _service.IndividualScore(_model.GameList, _model.CurrentGame);
            _view.BindingIndividualScore(scoreList);


            //올커버 사이드 플레이어 생성 및 DataGridview에 표시
            var allCoverSidePlayer = _model.PlayerList.Where(p => p.AllCoverSide).ToList();
            _view.SetAllcoverGamePlayers(allCoverSidePlayer);
            
            // 선택되 게임 스코어 정보 적용
            var selectedGame = _model.GameList.FirstOrDefault(g => g.GameSeq == _model.CurrentGame);
            
            //사이드 게임 반영
            _view.LoadAllcoverGamePlayers(selectedGame);
        }

        

        /// <summary>
        /// 참가자의 핸디캡조건, 사이드게임 참가 여부 설정 이벤트
        /// </summary>
        /// <param name="obj"></param>
        private void SetPlayerOption(string obj)
        {
            IPlayerOptionView view = new PlayerOptionView();
            PlayerInfoDto player = _model.PlayerList.FirstOrDefault(p => p.PlayerName == obj);
            if (player != null)
            {
                PlayerOtionPresenter presenter = new PlayerOtionPresenter(view, player);
                presenter.UpdatePlayer += UpdateInfo;
                view.ShowForm();
            }
        }


        

        /// <summary>
        /// 플레이어 추가 버튼 클릭
        /// 전체 플레이어 리스트에서 
        /// </summary>
        /// <param name="gameSeq"></param>
        /// <param name="groupNumber"></param>
        private void ParticipantRegist(int gameSeq, int groupNumber)
        {
            IRecordBoardPlayerManageView view = new RecordBoardPlayerManageView();
            RecordBoardPlayerManagePresenter presenter = new RecordBoardPlayerManagePresenter(view, _repository, _model);
            _model.CurrentGame = gameSeq;
            _model.CurrentGroup = groupNumber;
            presenter.SetPlayerList();
            view.ShowForm();
            GetMatchInfo(_model.MatchCode);
        }
        private void SaveIndividualSideRank(object sender, EventArgs e)
        {
            int? rankCount = _model.IndividaulSideSet.Count;
            _model.IndividualRanker = _view.SetIndividualSideRank(_model.IndividaulSideSet.Count);
            RankAssignmentView view = new RankAssignmentView();
            RankAssignmentPresnter presnter = new RankAssignmentPresnter(view, _model);
            view.AddPlayerPanel();
            view.ShowForm();
            GetMatchInfo(_model.MatchCode);
        }

        /// <summary>
        /// 개인 사이드 게임 설정 창 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetIndividualSide(object sender, EventArgs e)
        {
            IIndividualSideSetView view = new IndividaulSideSetView();
            IndividualSideSetPresenter presenter = new IndividualSideSetPresenter(view, _repository, _model);
            view.ShowForm();
        }
        /// <summary>
        /// 플레이어별 점수 입력
        /// </summary>
        /// <param name="obj"></param>
        private void EnterPlayerScore(PlayerInfoDto obj)
        {
            IEnterScoreView view = new EnterScoreView();
            IRecordBoardRepository repository = new RecordBoardRepository();
            EnterScorePresenter presenter = new EnterScorePresenter(view, repository, _model);
            presenter.GetGameInfo(obj);
            view.ShowForm();
            //ShowGameGroup(_gameSeq);
            GetMatchInfo(_model.MatchCode);
        }

        /// <summary>
        /// 모임에 등록된 게임 정보 수신 후 _model 게임리스트에 반영
        /// 20250518 servie로 이전
        /// </summary>
        //private void LoadGames()
        //{
        //    DataTable games = _repository.LoadGameOrder(_model.MatchCode);
        //    _model.GameList = new List<GameOrderDto>();
        //    foreach (DataRow row in games.Rows)
        //    {
        //        var gameOrder = new GameOrderDto
        //        {
        //            GameSeq = Convert.ToInt32(row["game_order"]),
        //            GameType = Convert.ToInt32(row["game_type"]),
        //            PlayerCount = Convert.ToInt32(row["game_player"]),
        //            PersonalSideGame = Convert.ToInt32(row["game_side"]) == 1 ? true : false,
        //            AllCoverGame = Convert.ToInt32(row["game_allcover"]) == 1 ? true : false
        //        };
        //        _model.GameList.Add(gameOrder);
        //        //참가자 그룹 자동 생성
        //        CreateGroup(gameOrder);
        //    }
        //}

        /// <summary>
        /// 전체 참가자 정보 수신
        /// 20250518 servie로 이전
        /// </summary>
        //private void GetAllPlayer()
        //{
        //    // attend 참가자 정보에서 참가자 수신 후 리스트에 등록
        //    DataTable Players = _repository.LoadAllPalyerList(_model.MatchCode);
        //    _model.PlayerList = new List<PlayerInfoDto>();
        //    foreach (DataRow row in Players.Rows)
        //    {
        //        _model.PlayerList.Add(new PlayerInfoDto
        //        {
        //            //MemberCode = Convert.ToInt32(row["att_memcode"]),
        //            PlayerName = row["att_name"].ToString(),
        //            Gender = Convert.ToInt32(row["att_gender"]) == 1 ? true : false,
        //            IsPro = Convert.ToInt32(row["att_pro"]) == 1 ? true : false,
        //            //Handycap = Convert.ToInt32(row["att_handi"]),
        //            IndividualSide = Convert.ToInt32(row["att_side"]) == 1 ? true : false,
        //            AllCoverSide = Convert.ToInt32(row["att_allcover"]) == 1 ? true : false
        //        });
        //    }
        //    foreach (var player in _model.PlayerList)
        //    {
        //        int baseHandicap = 0;

        //        // 여자 핸디: +15점
        //        if (player.Gender)
        //            baseHandicap += 15;

        //        // 프로 핸디: -5점
        //        if (player.IsPro)
        //            baseHandicap -= 5;

        //        // 결과 저장
        //        player.Handycap = baseHandicap;
        //    }
        //    //_view.LoadAllPlayers(_model.PlayerList);
        //}

        /// <summary>
        /// 등록된 게임별 플레이어 정보 수신 후 각 그룹별로 플레이어 정보 등록
        /// 20250518 servie로 이전
        /// </summary>
        //private void LoadParticitnadPlayers()
        //{
        //    // 모임내 기록된 참가자 전체 정보 조회
        //    DataTable allGamePlayers = _repository.LoadGamePlayers(_model.MatchCode);
        //    // 전체 기록을 각 게임과 그룹 번호 확인 후 최종 Gruops.Player에 등록
        //    foreach (DataRow row in allGamePlayers.Rows)
        //    {

        //        int gameOrder = Convert.ToInt32(row["pl_game"]);
        //        int groupNumber = Convert.ToInt32(row["pl_group"]);

        //        var targetGame = _model.GameList.FirstOrDefault(g => g.GameSeq == gameOrder);
        //        var targetGroup = targetGame.Groups.FirstOrDefault(g => g.GroupNumber == groupNumber);

        //        PlayerInfoDto player = new PlayerInfoDto
        //        {
        //            PlayerName = row["pl_name"].ToString(),
        //            MemberCode = Convert.ToInt32(row["pl_member"]),
        //            Handycap = Convert.ToInt32(row["pl_handi"]),
        //            IsPro = row["pl_pro"].ToString() == "1" ? true : false,
        //            Gender = row["pl_gender"].ToString() == "1" ? true : false,
        //            IndividualSide = row["pl_side"].ToString() == "1" ? true : false,
        //            AllCoverSide = row["pl_allcover"].ToString() == "1" ? true : false,
        //            Score = Convert.ToInt32(row["pl_score"]),
        //            IsSelected = true,
        //            IsAllCover = row["pl_isallcover"].ToString() == "1" ? true : false
        //        };
        //        targetGroup.players.Add(player);
        //    }
        //}

    }
}
