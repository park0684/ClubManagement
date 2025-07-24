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
    public class GradeManagePresenter
    {
        IGradeManageView _view;
        IMemberScoreService _service;
        IMemberScoreRepository _repository;
        DataTable _grade;

        public GradeManagePresenter(IGradeManageView view, IMemberScoreRepository repository)
        {
            _view = view;
            _repository = repository;
            _service = new MemberScoreService(_repository);
            _view.AddGradeEvent += AddGradeItem;
            _view.CloseFormEvent += CloseForm;
            _view.DeleteGradeEvent += DeleteGradeItem;
            _view.SaveEvent += UpdateGradeInfo;
            ShowGradeForm();
        }

        private void UpdateGradeInfo(object sender, DataTable e)
        {
            try
            {
                _service.UpdateGradeInfo(e);
                _view.ShowMessagebox("회원 등급 정보 수정을 완료 하였습니다.");
                _view.CloseForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessagebox("회원 등급 정보 수정 실패\n" + ex.Message);
            }
        }

        private void DeleteGradeItem(object sender, EventArgs e)
        {
            try
            {
                _view.DeleteGradeItem();
            }
            catch(Exception ex)
            {
                _view.ShowMessagebox(ex.Message);
            }
        }

        private void CloseForm(object sender, EventArgs e)
        {
            _view.CloseForm();
        }

        private void AddGradeItem(object sender, EventArgs e)
        {
            try
            {
                _view.AddGradeItem();
            }
            catch(Exception ex)
            {
                _view.ShowMessagebox(ex.Message);
            }
        }

        private void ShowGradeForm()
        {
            try
            {
                var result = _service.GetMemberGrade();
                _grade = new DataTable();
                _grade.Columns.Add("grd_code", typeof(int));
                _grade.Columns.Add("grd_name", typeof(string));
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        _grade.Rows.Add(item.Key, item.Value);
                    }
                    _view.BindingGradeData(_grade);
                }
                _view.ShowForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessagebox(ex.Message);
            }
            
        }
    }
}
