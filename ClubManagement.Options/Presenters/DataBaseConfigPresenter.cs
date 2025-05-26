using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Options.views;
using ClubManagement.Common.Service;
using ClubManagement.Common.Models;

namespace ClubManagement.Options.Presenters
{
    public class DataBaseConfigPresenter
    {
        IDatabaseConfigView _view;
        IDatabaseService _service;
        DatabaseModel _model;
        public DataBaseConfigPresenter(IDatabaseConfigView view)
        {
            _view = view;
            _model = new DatabaseModel();
            _service = new DatabaseService();
            _view.CloseFromEvent += CloseConfig;
            _view.TestEvent += ConnectTest;
            _view.SaveEvent += SaveConfig;
            SetDataBaseInfo();
        }

        private void SetDataBaseInfo()
        {
            _model.Address = _view.Address;
            _model.Prer = _view.Port;
            _model.User = _view.User;
            _model.Password = _view.Password;
            _model.Database = _view.Database;
        }

        private void SaveConfig(object sender, EventArgs e)
        {
            try
            {
                _service.SaveDatabaseInfo(_model);
                _view.CloseForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void ConnectTest(object sender, EventArgs e)
        {
            try
            {
                _service.ConnectTest();
                _view.ShowMessage("정상연결 : 데이터 베이스 연결에 성공하였습니다");
            }
            catch (Exception ex)
            {
                _view.ShowMessage("연결실패 : "+ ex.Message);
            }
        }

        private void CloseConfig(object sender, EventArgs e)
        {
            _view.CloseForm();
        }
    }
}
