using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Members.DTOs;
using ClubManagement.Members.Views;
using ClubManagement.Members.Repositories;
using ClubManagement.Members.Models;
using ClubManagement.Members.Services;
using ClubManagement.Common.Hlepers;

namespace ClubManagement.Members.Presenters
{
    public class MemberListPresenter
    {
        IMemberListView _view;
        IMemberRepository _repository;
        IMemberService _service;

        public MemberListPresenter(IMemberListView view, IMemberRepository repository)
        {
            _view = view;
            _repository = repository;
            _service = new MemberService(repository);
            _view.SearchEvent += LoadMemberList;
            _view.AddMemberEvent += AddMember;
            _view.EidtMemberEvent += LoadMemberInfo;
        }
        /// <summary>
        /// 회원 선택 후 상세 정보 조회 또는 수정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadMemberInfo(object sender, EventArgs e)
        {
            int memberCode = _view.SelectedCode.Value;
            IMemberDetailView view = new MemberDetailView();
            IMemberRepository repository = new MemberRepository();
            MemberDetailPresenter presenter = new MemberDetailPresenter(view, repository);
            presenter.GetMemberInfo(memberCode);
            view.ShowForm();
        }

        /// <summary>
        /// 회원 등록 버튼 클릭
        /// 신규 회원 등록 기능
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMember(object sender, EventArgs e)
        {
            IMemberDetailView view = new MemberDetailView();
            IMemberRepository repository = new MemberRepository();
            MemberDetailPresenter presenter = new MemberDetailPresenter(view, repository);
            view.ShowForm();
        }
        /// <summary>
        /// 회원 조회 이벤트
        /// view 필드의 회원 조회 조건을 MemberSearchModel에 등록 후 조회 진행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadMemberList(object sender, EventArgs e)
        {
            try
            {
                var model = new MemberSearchModel();
                model.Status = _view.Status;
                model.SearchWord = _view.SearchWord;
                model.AccessCheck = _view.AccessCheck;
                model.AccFromDate = _view.AccFromDate;
                model.AccToDate = _view.AccToDate;
                model.SecessCheck = _view.SecessCheck;
                model.SecFromDate = _view.SecFromDate;
                model.SecToDate = _view.SecToDate;
                model.GameCheck = _view.GameCheck;
                model.GameFromDate = _view.GameFromDate;
                model.GameTodate = _view.GameToDate;
                model.ExcludeMember = _view.ExcludeMember;
                model.ExcludeRegular = _view.ExcludeRegular;
                model.ExcludeIrregular = _view.ExcludeIrregular;
                model.ExcludeEvent = _view.ExcludeEvent;
                model.StartDate = _service.LoadStartDate();


                var result = _service.LoadMemberList(model);
                _view.MemberListBinding(result);
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }
    }
}
