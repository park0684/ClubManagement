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
        SearchMemberDto _dto;
        int _matchCode = 0;
        MemberScoreModel _model;
        public MemberScorePresenter(IMemberScoreView view, IMemberScoreRepository repository)
        {
            _view = view;
            _repository = repository;
            _service = new MemberScoreService(_repository);
            _view.MatachSearchEvent += SearchMatch;
            _view.MemberSelectedEvent += LoadMemberInfo;
            _view.SearchScoreEvent += LoadTotalScore;
            SetComboBoxItem();
            InitailizeModel();
        }
        private void InitailizeModel()
        {
            var newDto = new SearchScoreDto
            {
                StartDate = _service.GetStartDate()
            };
            _model = MemberScoreModel.FromDto(newDto);
            _model.UpdateSeachCondition();
        }
        private void SetComboBoxItem()
        {
            var statusMap = MemberHelper.MemStatus;
            var items = new Dictionary<string, Dictionary<int, string>>();
            var statusItem = new Dictionary<int, string>();
            foreach(var item in statusMap)
            {
                statusItem.Add(item.Key, item.Value);
            }


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
        
        private void LoadTotalScore(object sender, EventArgs e)
        {
            try
            {
                var colunms = new Dictionary<string, string>
                {
                    { "mem_code","회원코드" },
                    { "mem_name","회원명" },
                    { "mem_hanid","핸디" },
                    { "mem_aver","기준에버" },
                    { "game_count","게임" },
                    { "total_score","총핀" },
                    { "average_score","에버" }
                };
                var resultData = new DataTable();
                if (_view.IsSearchDate && !_view.IsMatch)
                {
                    _dto = new SearchMemberDto
                    {
                        GameCheck = _view.IsSearchDate,
                        GameFromDate = _view.FromDate,
                        GameTodate = _view.ToDate,
                        Status = _view.Status,
                        ExcludeMember = _view.ExculuedMember,
                        SearchWord = _view.SearchWord
                    };
                    var result = _service.GetTotalScore(_dto);
                    resultData = result;
                }
                else if (!_view.IsSearchDate && _view.IsMatch)
                {
                    var result = _service.GetMatchScore(_matchCode);

                    for (int i = 1; i <= result.gameOrder; i++)
                    {
                        colunms.Add($"{i}Game", $"{i}Game");
                    }

                    resultData = result.resutData;
                    
                }
                colunms.Add("max_score", "최고점");
                colunms.Add("min_score", "최저점");
                colunms.Add("diff_score", "하이로우");
                _view.SetDatagirColumns(colunms);
                //_view.MemberScoreBinding(resultData);
            }
            catch(Exception ex)
            {
                _view.ShowMessga(ex.Message);
            }
        }

        private void LoadMemberInfo(object sender, int e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                _view.ShowMessga(ex.Message);
            }
        }

        private void SearchMatch(object sender, EventArgs e)
        {
            try
            {
                if(_view.IsMatch)
                {
                    _model.IsSearchMatch = _view.IsMatch;
                    _model.UpdateSeachCondition();
                    _view.ShowMessga("만드는중" + _model.IsSearchDate + _model.IsSearchMatch);
                    _view.MatchTitel = "만들고 있어";
                    
                    _matchCode = 83;
                    _view.MatchTitel = _service.GetMatchTitle(_matchCode);
                }
                else
                {
                    _model.IsSearchMatch = _view.IsMatch;
                    _model.UpdateSeachCondition();
                    _view.ShowMessga("해제" + _model.IsSearchDate + _model.IsSearchMatch);
                    _view.MatchTitel = "";
                    _matchCode = 0;
                }
                
            }
            catch (Exception ex)
            {
                _view.ShowMessga(ex.Message);
            }
        }
    }
}
