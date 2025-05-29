using ClubManagement.Members.Models;
using ClubManagement.Members.Repositories;
using ClubManagement.Members.Services;
using ClubManagement.Members.Views;
using System;
using System.Data;

namespace ClubManagement.Members.Presenters
{
    public class MemberSearchPresenter
    {
        IMemberSearchView _view;
        IMemberRepository _repository;
        MemberSearchModel _model;
        IMemberService _service;
        readonly Action<int, string> _onMemberSelected;
        //private IMemberSelectCallback _callback;
        public MemberSearchPresenter(IMemberSearchView view, IMemberRepository repository, Action<int, string> onMemberSelected = null)
        {
            _view = view;
            _repository = repository;
            _service = new MemberService(repository);
            _onMemberSelected = onMemberSelected;

            _view.CloseFormEvent += CloseAction;
            _view.MemberSeachEvent += MemberSearch;
            _view.SelectMemberEvent += SelectMember;

            _model = new MemberSearchModel();
            MemberSearch(this, EventArgs.Empty);
        }

        /// <summary>
        /// 조회된 회원 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectMember(object sender, EventArgs e)
        {
            int code = _view.MemberCode;
            string name = _view.MemberName;
            _onMemberSelected?.Invoke(code, name);
            _view.CloseForm();
        }

        /// <summary>
        /// 회원 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemberSearch(object sender, EventArgs e)
        {
            _model.SearchWord = _view.SearchWord;
            _model.IsInclude = _view.IsInculde;
            DataTable resutl = _service.LoadSearchMember(_model);
            _view.SetMemberList(resutl);
        }

        private void CloseAction(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
