using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Members.Views;
using ClubManagement.Members.Repositories;
using ClubManagement.Members.Services;
using ClubManagement.Members.Models;
using ClubManagement.Common.Hlepers;
using ClubManagement.Members.DTOs;
using System.Data;

namespace ClubManagement.Members.Presenters
{
    public class MemberScorePresenter
    {
        IMemberScoreView _view;
        IMemberScoreRepository _repository;
        IMemberScoreService _service;
        SearchScoreDto _dto;
        int _matchCode = 0;
        MemberScoreModel _model;
        public MemberScorePresenter(IMemberScoreView view, IMemberScoreRepository repository)
        {
            _view = view;
            _repository = repository;
            _service = new MemberScoreService(_repository);
            _view.MatachSearchEvent += SearchMatch;
            _view.MemberSelectedEvent += LoadMemberInfo;
            _view.SearchScoreEvent += LoadMemberScore;
            _view.GradeUpdateEvet += UpdateGrade;
            SetComboBoxItem();
            InitailizeModel();
        }

        private void UpdateGrade(object sender, List<int> memberList)
        {
            IBulkChangeView view = new BulkChangeView();
            var presenter = new GradeUpdatePresenter(view, _repository, memberList);
        }

        /// <summary>
        /// 조회 모델 초기화
        /// </summary>
        private void InitailizeModel()
        {
            // 모델 생성 및 프로그램 시작일 등록
            var newDto = new SearchScoreDto
            {
                StartDate = _service.GetStartDate()
            };
            _model = MemberScoreModel.FromDto(newDto);
            _model.UpdateSeachCondition();
        }

        /// <summary>
        /// 콤보박스 아이템 등록
        /// </summary>
        private void SetComboBoxItem()
        {
            // 회원 상태 콤보박스 설정
            var statusMap = MemberHelper.MemStatus;
            var items = new Dictionary<string, Dictionary<int, string>>();
            var statusItem = new Dictionary<int, string>();
            foreach(var item in statusMap)
            {
                statusItem.Add(item.Key, item.Value);
            }

            // 정렬 조건 콤보박스 아이템 설정
            string[] sortType = new string[] { "총핀(에버)", "기준에버", "향상", "하이로우" };
            int key = 0;
            var sortItem = new Dictionary<int, string>();
            foreach(string item in sortType )
            {
                sortItem.Add(key, item);
                key++;
            }

            items.Add("status", statusItem);
            items.Add("sort", sortItem);

            _view.SetComboboxItem(items);
            
        }
        
        /// <summary>
        /// 회원 점수 조회 이벤트 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadMemberScore(object sender, EventArgs e)
        {
            try
            {
                // 조회 조건 모델 설정
                _model.FromDate = _view.FromDate;
                _model.ToDate = _view.ToDate;
                _model.SearchWord = _view.SearchWord;
                _model.Status = _view.Status;
                _model.SortType = _view.SortType;
                _dto = _model.ToDto();

                // 데이터 그리드 뷰 칼럼 딕셔너리 설정
                var colunms = new Dictionary<string, string>
                {
                    { "No", "No" },
                    { "mem_code","회원코드" },
                    { "mem_name","회원명" },
                    { "grd_name" , "등급"},
                    { "mem_handi","핸디" },
                    { "reference_average","기준에버" },
                    { "total_score","총핀" },
                    { "average_score","에버" },
                    { "game_count","게임" }
                };

                //기간 조회 조건과 모임조회 조건 비교로 service 실행
                var resultData = new DataTable();
                if (_model.IsSearchDate && !_model.IsSearchMatch)
                {
                    var result = _service.GetTotalScore(_dto);
                    resultData = result;
                }
                else if (!_model.IsSearchDate && _model.IsSearchMatch)
                {
                    var result = _service.GetMatchScore(_dto);

                    //모임 내 게임 수만큼 컬럼 추가
                    colunms.Remove("game_count");
                    for (int i = 1; i <= result.gameOrder; i++)
                    {
                        colunms.Add($"Game{i}", $"Game{i}");
                    }
                    
                    resultData = result.resutData;
                    
                }

                // 나머지 컬럼 추가
                colunms.Add("reference_gap", "편차");
                colunms.Add("MAX_SCORE", "최고점");
                colunms.Add("MIN_SCORE", "최저점");
                colunms.Add("game_gap", "하이로우");
                
                int no = 1;
                resultData.Columns.Add("No");
                foreach (DataRow row in resultData.Rows)
                {
                    row["No"] = no;
                    no++;
                }
                //컬럼 셋팅 및 데이터 바인딩
                _view.SetDatagirColumns(colunms);
                _view.MemberScoreBinding(resultData);
            }
            catch(Exception ex)
            {
                _view.ShowMessga(ex.Message);
            }
        }

        /// <summary>
        /// 회원 더블 클릭 시 선택 회원 상세 정보 실행
        /// </summary>
        /// <param name="membere"></param>
        private void LoadMemberInfo(int membere)
        {
            try
            {
                IMemberScoreDetailView view = new MemberScoreDetailView();
                MemberScoreDetailPresenter presenter = new MemberScoreDetailPresenter(view, _repository, membere);
            }
            catch (Exception ex)
            {
                _view.ShowMessga(ex.Message);
            }
        }

        /// <summary>
        /// 모임 검색 체크 시 선택 할 모임 조회
        /// 게임 기록이 있는 모임들만 조회됨
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchMatch(object sender, EventArgs e)
        {
            try
            {
                if(_view.IsMatch)
                {
                    ISearchMatchView view = new SearchMatchView();
                    IMemberScoreRepository repository = new MemberScoreRepository();
                    new SearchMatchPresenter(view, repository);
                    _model.IsSearchMatch = _view.IsMatch;
                    _model.UpdateSeachCondition();
                    view.SelectedMatchEvent += (s, args) =>
                    {
                        _model.MatchCode = view.MatchCode;
                        _view.MatchTitel = _service.GetMatchTitle(_model.MatchCode);
                        view.CloseForm();
                    };
                    view.ShowForm();
                }
                else
                {
                    _model.IsSearchMatch = _view.IsMatch;
                    _model.UpdateSeachCondition();
                    _view.MatchTitel = "";
                    _model.MatchCode = 0;
                }
                
            }
            catch (Exception ex)
            {
                _view.ShowMessga(ex.Message);
            }
        }
    }
}
