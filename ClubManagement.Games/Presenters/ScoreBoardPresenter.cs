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

namespace ClubManagement.Games.Presenters
{
    public class ScoreBoardPresenter
    {
        IScoreBoardView _view;
        IScoreBoardRepository _repository;
        ScoreBoardModel _model;
        int _gameSeq = 0;
        public ScoreBoardPresenter(IScoreBoardView view, IScoreBoardRepository repository)
        {
            _view = view;
            _repository = repository;
            _model = new ScoreBoardModel();
            _view.PlayerOptionEvent += SetPlayerOption;
            _view.GameButtonClick += ShowGameGroup;
            _view.AssignPlayerClick += ParticipantRegist;
            _view.EnterScoreEvent += EnterPlayerScore;
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
            //조회 모임 코드 및 모임명 설정
            _model.MatchCode = matchCode;
            _view.MatchTitle = _repository.LoadMatchTitle(matchCode).ToString();

            //모임 내 등록된 게임 정보 수신 및 조회 버튼 생성
            LoadGames(); // 모임내 게임리스트 설정 후 
            //각 게임별 참여자 및 그룹 설정
            LoadParticitnadPlayers();
            GetAllPlayer(); //

            _view.CreateGameButton(_model.GameList); // 게임 조회 버튼 생성
            _view.SetAllPlayerList(_model.GameList, _model.PlayerList); // 전체 참가자 리스트 생성 및 데이터 등록            
            //사이드게임 데이터그리드뷰 서정
            SetSideGameGroupBox();
            ShowGameGroup(_model.CurrentGame);
        }

        /// <summary>
        /// 모임에 등록된 게임 정보 수신 후 _model 게임리스트에 반영
        /// </summary>
        private void LoadGames()
        {
            DataTable games = _repository.LoadGameOrder(_model.MatchCode);
            _model.GameList = new List<GameOrderDto>();
            foreach (DataRow row in games.Rows)
            {
                var gameOrder = new GameOrderDto
                {
                    GameSeq = Convert.ToInt32(row["game_order"]),
                    GameType = Convert.ToInt32(row["game_type"]),
                    PlayerCount = Convert.ToInt32(row["game_player"]),
                    PersonalSideGame = Convert.ToInt32(row["game_side"]) == 1 ? true : false,
                    AllCoverGame = Convert.ToInt32(row["game_allcover"]) == 1 ? true : false
                };
                _model.GameList.Add(gameOrder);
                //참가자 그룹 자동 생성
                CreateGroup(gameOrder);
            }
        }
        private void GetAllPlayer()
        {
            // attend 참가자 정보에서 참가자 수신 후 리스트에 등록
            DataTable Players = _repository.LoadAllPalyerList(_model.MatchCode);
            _model.PlayerList = new List<PlayerInfoDto>();
            foreach (DataRow row in Players.Rows)
            {
                _model.PlayerList.Add(new PlayerInfoDto
                {
                    MemberCode = Convert.ToInt32(row["att_memcode"]),
                    PlayerName = row["att_name"].ToString(),
                    Gender = Convert.ToInt32(row["att_gender"]) == 1 ? true : false,
                    IsPro = Convert.ToInt32(row["att_pro"]) == 1 ? true : false,
                    Handycap = Convert.ToInt32(row["att_handi"]),
                    IndividualSide = Convert.ToInt32(row["att_side"]) == 1 ? true : false,
                    AllCoverSide = Convert.ToInt32(row["att_allcover"]) == 1 ? true : false
                });
            }
            foreach (var player in _model.PlayerList)
            {
                int baseHandicap = 0;

                // 여자 핸디: +15점
                if (player.Gender)
                    baseHandicap += 15;

                // 프로 핸디: -5점
                if (player.IsPro)
                    baseHandicap -= 5;

                // 결과 저장
                player.Handycap = baseHandicap;
            }
            //_view.LoadAllPlayers(_model.PlayerList);
        }
        /// <summary>
        /// 게임 버튼 클릭 이벤트 실행시 해당 게임의 그룹별 플레이어 정보를 표시
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
                {
                    InsertIndividualPlayer();
                }
                
                foreach (var targetGroup in selectedGame.Groups)
                {
                    foreach (var player in targetGroup.players)
                    {
                        _view.AddPlayerPanal(player);

                    }
                }
            }
            else
            {
                //단체전의 경우 그룹 패널 생성
                foreach (var group in selectedGame.Groups)
                {
                    _view.CreateGroupPanal(group, gameSeq);
                }
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
        }

        /// <summary>
        /// 참가자의 핸디캡조건, 사이드게임 참가 여부 설정 이벤트
        /// </summary>
        /// <param name="obj"></param>
        private void SetPlayerOption(string obj)
        {
            IPlayerOptionView view = new PlayerOptionView();
            PlayerInfoDto player = _model.PlayerList.FirstOrDefault(p => p.PlayerName == obj);
            if(player != null)
            {
                PlayerOtionPresenter presenter = new PlayerOtionPresenter(view,player);
                presenter.UpdatePlayer += UpdateInfo;
                view.ShowForm();
            }
            
        }
        /// <summary>
        /// 참가자 정보가 수정되었다면 model의 값 수정
        /// </summary>
        /// <param name="player"></param>
        private void UpdateInfo(PlayerInfoDto player)
        {
            var targetPlayer = _model.PlayerList.FirstOrDefault(p => p.PlayerName == player.PlayerName);
            if (targetPlayer != null)
            {
                // 수정된 값 반영
                targetPlayer.Gender = player.Gender;
                targetPlayer.IsPro = player.IsPro;
                targetPlayer.IndividualSide = player.IndividualSide;
                targetPlayer.AllCoverSide = player.AllCoverSide;
                targetPlayer.Handycap = player.Handycap;
            }
            _repository.UpdatePlayerInfo(targetPlayer, _model.MatchCode);

            GetMatchInfo(_model.MatchCode);            
        }

        /// <summary>
        /// 변경된 PlayerList 정보로 각 Side게임 DataGridView 화면 갱신
        /// </summary>
        private void SetSideGameGroupBox()
        {
            
            var IndividualsidePlayers = _model.PlayerList.Where(p => p.IndividualSide).ToList();
            _view.SetSideGamePlayerList(IndividualsidePlayers);  // 사이드 게임만
            
            var AllcoverSidePlayers = _model.PlayerList.Where(p => p.AllCoverSide).ToList();
            _view.SetAllcoverGamePlayers(AllcoverSidePlayers);
            var selectedGame = _model.GameList.FirstOrDefault(g => g.GameSeq == _model.CurrentGame);

            _view.SetSideGameScore(selectedGame);
            _view.LoadAllcoverGamePlayers(selectedGame);
        }
        
        /// <summary>
        /// gameOrder내 참가자/팀 수 만큼 그룹 자동 생성 메서드
        /// </summary>
        /// <param name="gameOrder"></param>
        private void CreateGroup(GameOrderDto gameOrder)
        {
            gameOrder.Groups = new List<GroupDto>();
            for(int i =1; i <= gameOrder.PlayerCount; i++)
            {
                GroupDto group = new GroupDto
                {
                    GroupNumber = i,
                    Score = 0,
                    Rank = 0,
                };
                gameOrder.Groups.Add(group);
            }
        }
        /// <summary>
        /// 등록된 게임별 플레이어 정보 수신 후 각 그룹별로 플레이어 정보 등록
        /// </summary>
        private void LoadParticitnadPlayers()
        {
            // 모임내 기록된 참가자 전체 정보 조회
            DataTable allGamePlayers = _repository.LoadGamePlayers(_model.MatchCode);
            // 전체 기록을 각 게임과 그룹 번호 확인 후 최종 Gruops.Player에 등록
            foreach(DataRow row in allGamePlayers.Rows)
            {
                
                int gameOrder = Convert.ToInt32(row["pl_game"]);
                int groupNumber = Convert.ToInt32(row["pl_group"]);

                var targetGame = _model.GameList.FirstOrDefault(g => g.GameSeq == gameOrder);
                var targetGroup = targetGame.Groups.FirstOrDefault(g => g.GroupNumber == groupNumber);

                PlayerInfoDto player = new PlayerInfoDto
                {
                    PlayerName = row["pl_name"].ToString(),
                    MemberCode = Convert.ToInt32(row["pl_member"]),
                    Handycap = Convert.ToInt32(row["pl_handi"]),
                    IsPro = row["pl_pro"].ToString() == "1" ? true : false,
                    Gender = row["pl_gender"].ToString() == "1" ? true : false,
                    IndividualSide = row["pl_side"].ToString() == "1" ? true : false,
                    AllCoverSide = row["pl_allcover"].ToString() == "1" ? true : false,
                    Score = Convert.ToInt32(row["pl_score"]),
                    IsSelected = true,
                    IsAllCover = row["pl_isallcover"].ToString() == "1" ? true : false
                };
                targetGroup.players.Add(player);
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
            IScoreBoardPlayerManageView view = new ScoreBoardPlayerManageView();
            ScoreBoardPlayerManagePresenter presenter = new ScoreBoardPlayerManagePresenter(view, _repository, _model);
            _model.CurrentGame = gameSeq;
            _model.CurrentGroup = groupNumber;
            presenter.SetPlayerList();
            view.ShowForm();
            //ShowGameGroup(gameSeq);
            GetMatchInfo(_model.MatchCode);
        }

        /// <summary>
        /// 플레이어별 점수 입력
        /// </summary>
        /// <param name="obj"></param>
        private void EnterPlayerScore(PlayerInfoDto obj)
        {
            IEnterScoreView view = new EnterScoreView();
            IScoreBoardRepository repository = new ScoreBoardRepository();
            EnterScorePresenter presenter = new EnterScorePresenter(view, repository, _model);
            presenter.GetGameInfo(obj);
            view.ShowForm();
            //ShowGameGroup(_gameSeq);
            GetMatchInfo(_model.MatchCode);
        }

    }
}
