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

        /// <summary>
        /// 회비 금액 변경 시 적용 횟수 계산 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAmountChainged(object sender, EventArgs e)
        {
            int amount = (int)_view.DueAmount;
            int applyCount = amount < 10000 ? 1: amount / 10000;
            _view.SetApplyCounter(applyCount);
        }

        /// <summary>
        /// 전표 유형 변경 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TypeChainge(object sender, EventArgs e)
        {
            _view.TypeChaingedSet();
            if (_view.IsWithdrawal)
            {
                _model.MemberCode = 0;
            }
        }

        /// <summary>
        /// 멤버 선택 시 코드와 이름 반영
        /// </summary>
        /// <param name="memberCode">선택된 멤버 코드</param>
        /// <param name="memberName">선택된 멤버 이름</param>
        public void OnMemberSelected(int memberCode, string memberName)
        {
            _model.MemberCode = memberCode;
            _model.MemberName = memberName;

            // 뷰에 멤버 이름 업데이트
            _view.MemberName = memberName;
        }

        /// <summary>
        /// 멤버 검색 뷰 열고 선택 이벤트 등록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemberSelect(object sender, EventArgs e)
        {

            IMemberSearchView view = new MemberSearchView();
            IMemberRepository repository = new MemberRepository();
            var searchPresenter = new MemberSearchPresenter(view, repository, OnMemberSelected);
            view.ShowForm();

        }

        /// <summary>
        /// 입출금 전표 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatementDelete(object sender, EventArgs e)
        {
            //선택 전표번호 설정
            int code = (int)_model.StatementCode;
            try
            {
                //전표 삭제 실행
                _repository.DeleteStatement(code);

                //완료 후 폼 종료
                _view.CloseForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        /// <summary>
        /// 입출금 전표 조회
        /// </summary>
        /// <param name="code">전표 코드</param>
        public void LoadStatement(int code)
        {
            // 전표 데이터 로드
            _model = _service.LoadStatement(code);

            // 뷰에 기본 정보 반영
            _view.StatementDate = _model.StatementDate;
            _view.StatementType = _model.StatementType;

            try
            {
                if (_model.IsWithdrawal)
                {
                    // 출금인 경우
                    _view.DueAmount = 0;
                    _view.Withdrawal = _model.StatementAmount;
                    _view.WithdrawalDetail = _model.StatementDetail;
                }
                else
                {
                    // 입금인 경우
                    _view.DueAmount = _model.StatementAmount;
                    _view.Withdrawal = 0;
                    _view.MemberName = _model.StatementDetail;
                }

                // 삭제 버튼 표시
                _view.SetDeleteButtonVisivle();
            }
            catch (Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        /// <summary>
        /// 모델 속성 값을 뷰 값으로 설정
        /// </summary>
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

        /// <summary>
        /// 전표 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 폼 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseAction(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
