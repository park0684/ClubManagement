using ClubManagement.Members.Repositories;
using ClubManagement.Members.Services;
using ClubManagement.Members.Views;
using System;
using System.Collections.Generic;

namespace ClubManagement.Members.Presenters
{
    public class GradeUpdatePresenter
    {
        IBulkChangeView _view;
        IMemberScoreService _service;
        IMemberScoreRepository _repository;
        List<int> _memberList;
        public GradeUpdatePresenter(IBulkChangeView view, IMemberScoreRepository repository, List<int> memberList)
        {
            _view = view;
            _repository = repository;
            _service = new MemberScoreService(_repository);
            _memberList = memberList;
            _view.CloseFormEvent += CloseForm;
            _view.SelectedEvent += SaveMemberGarde;
            SetComboBoxItems();
            _view.ShowForm();
        }

        private void SaveMemberGarde(object sender, int item)
        {
            try
            {
                _service.BulkSvaeMemberGared(_memberList, item);
                _view.ShowMessageBox("등록이 완료 되엇습니다.");
                _view.CloseForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessageBox(ex.Message);
            }
        }

        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }

        private void SetComboBoxItems()
        {
            try
            {
                var items = _service.GetMemberGrade();
                _view.SetComboBoxItems(items);
            }
            catch(Exception ex)
            {
                _view.ShowMessageBox(ex.Message);
            }
        }
    }
}
