using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Members.Models;
using ClubManagement.Members.Repositories;
using ClubManagement.Members.Views;
using ClubManagement.Members.Services;
using ClubManagement.Common.Hlepers;

namespace ClubManagement.Members.Presenters
{
    public class DuesManagePresenter
    {
        IDuesManageView _view;
        IDuesRepsotiry _repository;
        IDuesService _service;
        DuesModel _model;

        public DuesManagePresenter(IDuesManageView view , IDuesRepsotiry repsotiry )
        {
            this._view = view;
            this._repository = repsotiry;
            this._service = new DuesService(repsotiry);
            _model = new DuesModel();
            _view.MemberSearchEvent += LoadMember;
            _view.StatementSearchEvent += LoadState;
            _view.StatementAddEvent += AddState;
            _view.StatementEditEvent += EditState;
            LoadMemberList();
            _model.MemberCode = 0;
            LoadStatementList();
        }
        /// <summary>
        /// 입출금 전표 수정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditState(object sender, EventArgs e)
        {
            int code = _view.GetStateMentCode.Value;
            IStatementDetailView view = new StatementDetailView();
            IDuesRepsotiry repository = new DuesRepsotiry();
            StatementDetailPresenter presenter = new StatementDetailPresenter(view, repository);
            presenter.LoadStatement(code);
            view.ShowForm();
            LoadMemberList();
            LoadStatementList();
        }
        /// <summary>
        /// 입출금 전표 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddState(object sender, EventArgs e)
        {
            IStatementDetailView view = new StatementDetailView();
            IDuesRepsotiry repository = new DuesRepsotiry();
            new StatementDetailPresenter(view, repository);
            view.ShowForm();
            LoadMemberList();
            LoadStatementList();
        }

        /// <summary>
        /// 입출금 전표 리스트 조회 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadState(object sender, EventArgs e)
        {
            LoadStatementList();
        }

        /// <summary>
        /// 회원리스트 조회 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadMember(object sender, EventArgs e)
        {
            LoadMemberList();
        }

        /// <summary>
        /// 회원 조회 메소드
        /// </summary>
        private void LoadMemberList()
        {
            //모델 속성 설정
            _model.SearchWord = _view.SearchWord;
            _model.FromDate = _view.FromDate;
            _model.ToDate = _view.ToDate;
            _model.SecessInclude = _view.SecessInclude;

            //회원 목록 조회
            var result = _service.LoadMemberList(_model);

            //결과 DataGridView에 Binding
            _view.SetMemberListBinding(result);
        }

        /// <summary>
        /// 입출금 리스트 조회 메소드
        /// </summary>
        private void LoadStatementList()
        {
            //모델 속성 설정
            _model.SearchWord = _view.SearchWord;
            _model.FromDate = _view.FromDate;
            _model.ToDate = _view.ToDate;
            _model.SecessInclude = _view.SecessInclude;
            _model.MemberCode = _view.GetMemberCode;
            
            var result = _service.LoadStatementList(_model);
            _view.SetStateListBinding(result);
        }
    }
}
