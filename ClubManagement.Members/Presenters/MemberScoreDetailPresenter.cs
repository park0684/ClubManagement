using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Members.DTOs;
using ClubManagement.Members.Repositories;
using ClubManagement.Members.Services;
using ClubManagement.Members.Views;
using ClubManagement.Common.DTOs;
using ClubManagement.Common.Hlepers;
using System.Data;

namespace ClubManagement.Members.Presenters
{
    public class MemberScoreDetailPresenter
    {
        IMemberScoreDetailView _view;
        IMemberScoreRepository _repository;
        IMemberScoreService _service;

        int _member;
        public MemberScoreDetailPresenter(IMemberScoreDetailView view, IMemberScoreRepository repository, int member)
        {
            _view = view;
            _repository = repository;
            _service = new MemberScoreService(_repository);
            _view.CloseFormEvent += FormClose;
            _view.ScoreSearchEvent += GetScoreList;
            _view.GradeSaveEvent += GradeSave;
            _member = member;
            SetCombobox();
            GetMemberInfo();
            GetScore(3);
            _view.ShowForm();
        }

        /// <summary>
        /// ScoreSearchEvent 이벤트 실행 시 설정 조회 기간내 점수 조회 메서드 실행
        /// </summary>
        /// <param name="interval">조회 기간</param>
        private void GetScoreList(int interval)
        {
            try
            {
                GetScore(interval);
            }
            catch (Exception ex)
            {
                _view.ShowMessage("기록 불러오기 실패\n" + ex.Message);
            }
        }

        /// <summary>
        /// 등급 콤보박스 설정
        /// </summary>
        private void SetCombobox()
        {
            try
            {
                //DB에서 등급 정보 조회
                var result = _service.GetMemberGrade();

                //등급 미설정 회원 표시를 위한 미설정 아이템 추가
                result.Add(-1, "미설정");

                //view 아이템 등록 메서드 실행
                _view.SetComboBoxItems(result);
            }
            catch(Exception ex)
            {
                _view.ShowMessage("회원 등급 설정 실패\n" + ex.Message);
            }
        }

        /// <summary>
        /// view 실행 시 회원 기본정보 수신
        /// </summary>
        private void GetMemberInfo()
        {
            try
            {
                var result = _service.GetMemberBaseInfo(_member);
                _view.MemberName = result["mem_name"].ToString();
                _view.MemberHandi = Convert.ToInt32(result["mem_handi"]);
                _view.MemberGender = MemberHelper.GetMemberGenger(Convert.ToInt32(result["mem_gender"]));
                _view.MemberGrade = Convert.ToInt32(result["mem_grade"]);
                _view.Average = Convert.ToDecimal(result["AVG_SCORE"]);
            }
            catch(Exception ex)
            {
                _view.ShowMessage("회원조회 실패\n" + ex.Message);
            }
        }

        /// <summary>
        /// 회원 등급 수정 메서드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GradeSave(object sender, EventArgs e)
        {
            try
            {
                _service.SaveMemberGrade(_member, _view.MemberGrade);
                _view.ShowMessage("수정이 완료 되었습니다.");
            }
            catch(Exception ex)
            {
                _view.ShowMessage("등급 저장 실패\n" + ex.Message);
            }
        }

        /// <summary>
        /// 종료 메서드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormClose(object sender, EventArgs e)
        {
            _view.CloseForm();
        }

        /// <summary>
        /// 회원 점수 정보 조회 및 표기
        /// </summary>
        /// <param name="interval"></param>
        private void GetScore(int interval)
        {
            try
            {
                var result = _service.GetMemberScoreList(_member, interval);
                if (result == null)
                    return;
                _view.CreateGameScoreList(result);
                
                var allScores = result.SelectMany(s => s.GameScore).Select(x => (decimal)x).ToList();

                _view.MaxScore = allScores.Any() ? Convert.ToInt32(allScores.Max()) : 0;
                _view.MinScore = allScores.Any() ? Convert.ToInt32(allScores.Min()) : 0;
                _view.AverageScore = allScores.Any() ? Math.Round(allScores.Average(),1) : 0;
                _view.GameCount = allScores.Count;

                _view.SetGraph(result);
            }
            catch(Exception ex)
            {
                _view.ShowMessage("점수 조회 오류\n" + ex.Message);
            }
        }
    }
}
