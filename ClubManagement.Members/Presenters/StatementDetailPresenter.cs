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

namespace ClubManagement.Members.Presenters
{
    public class StatementDetailPresenter 
    {
        private IStatementDetailView _view;
        private IDuesRepsotiry _repository;
        IDuesService _service;
        private StatementModel _model;

        public StatementDetailPresenter(IStatementDetailView view, IDuesRepsotiry repository)
        {
            this._view = view;
            this._repository = repository;
            this._model = new StatementModel();
            this._service = new DuesService(repository);
            this._view.CloseEvent += CloseAction;
            this._view.SaveEvent += SatatementSave;
            this._view.SelectMemberEvent += MemberSelect;
            this._view.DeleteEvent += StatementDelete;
            this._view.TypeChaingedEvnet += TypeChainge;
            this._view.DuesAmountChaingedEvent += OnAmountChainged;
            this._model.IsNew = true;
            this._view.TypeChaingedSet();
        }

        private void OnAmountChainged(object sender, EventArgs e)
        {
            int amount = (int)_view.DueAmount;
            int applyCount = amount < 10000 ? 1: amount / 10000;
            _view.SetApplyCounter(applyCount);
        }

        private void TypeChainge(object sender, EventArgs e)
        {
            _view.TypeChaingedSet();
            if (_view.IsWithdrawal)
            {
                _model.MemberCode = 0;
            }
        }

        public void OnMemberSelected(int memberCode, string memberName)
        {
            _model.MemberCode = memberCode;
            _model.MemberName = memberName;

            // 뷰에 멤버 정보를 업데이트
            _view.MemberName = memberName;
        }
        private void MemberSelect(object sender, EventArgs e)
        {

            IMemberSearchView view = new MemberSearchView();
            IMemberRepository repository = new MemberRepository();
            var searchPresenter = new MemberSearchPresenter(view, repository, OnMemberSelected);
            view.ShowView();

        }

        private void StatementDelete(object sender, EventArgs e)
        {
            int code = (int)_model.StatementCode;
            _repository.DeleteStatement(code);
            _view.CloseForm();
        }

        public void LoadStatement(int code)
        {
            _model = _service.LoadStatement(code);

            _view.StatementDate = _model.StatementDate;
            _view.StatementType = _model.StatementType;

            if (_model.IsWithdrawal)
            {
                _view.DueAmount = 0;
                _view.Withdrawal = _model.StatementAmount;
                _view.WithdrawalDetail = _model.StatementDetail;
            }
            else
            {
                _view.DueAmount = _model.StatementAmount;
                _view.Withdrawal = 0;
                _view.MemberName = _model.StatementDetail;
            }
            _view.SetDeleteButtonVisivle();
        }

        public void SetModelDetail()
        {
            _model.StatementDate = _view.StatementDate;
            _model.IsWithdrawal = _view.IsWithdrawal;
            if (_model.IsWithdrawal)
            {
                _model.StatementAmount = _view.Withdrawal;
                _model.StatementDetail = _view.WithdrawalDetail;
            }
            else
            {
                _model.StatementAmount = _view.DueAmount;
                _model.StatementDetail = _view.MemberName;
            }
            _model.Apply = _view.Apply;
            _model.DueCount = _view.DueCount;
            _model.StatementType = _view.StatementType;
            _model.Memo = _view.Memo;
        }
        private void SatatementSave(object sender, EventArgs e)
        {
            SetModelDetail();
            try
            {
                _service.SaveStatement(_model);
                _view.CloseForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
            
        }


        private void CloseAction(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
