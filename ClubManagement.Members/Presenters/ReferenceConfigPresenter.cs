using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Members.Views;
using ClubManagement.Members.Services;
using ClubManagement.Members.Repositories;

namespace ClubManagement.Members.Presenters
{
    public class ReferenceConfigPresenter
    {
        IReferenceAverConfigView _view;
        IMemberScoreService _service;
        IMemberScoreRepository _repository;

        public ReferenceConfigPresenter(IReferenceAverConfigView view, IMemberScoreRepository repository)
        {
            _view = view;
            _repository = repository;
            _service = new MemberScoreService(_repository);
            _view.CloseFormEvent += FormClose;
            _view.SaveEvent += ConfigSave;
            ShowView();
        }

        private void ConfigSave(object sender, int e)
        {
            try
            {
                _service.SaveReferenceAverageInterval(e);
                _view.ShowMessage("기준 에버 기간 설정을 저장 하였습니다.");
                _view.CloseForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void FormClose(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
        private void ShowView()
        {
            int interval = _service.GetAverageInterval();
            _view.GetInterval(interval);
            _view.ShowForm();
        }
    }
}
