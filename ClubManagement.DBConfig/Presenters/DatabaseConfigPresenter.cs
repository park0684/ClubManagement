using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.DBConfig.views;
using ClubManagement.DBConfig.Service;
using ClubManagement.Common.Models;
using ClubManagement.Common.DTOs;
namespace ClubManagement.DBConfig.Presenters
{
    public class DatabaseConfigPresenter
    {
        IDatabaseConfigView _view;
        DatabaseConfigService _service;
        DatabaseModel _model;

        public DatabaseConfigPresenter(IDatabaseConfigView view)
        {
            _model = new DatabaseModel();
            _view = view;
            _view.CloseFromEvent += CloseForm;
            _view.SaveEvent += SaveDatabaseInfo;
            _view.TestEvent += ConnectTest; 
            _service = new DatabaseConfigService();
            LoadConnectionInfo();
        }

        public void LoadConnectionInfo()
        {
            var database = _service.LoadConnectionInfo();
            _model.Address = database.Address;
            _model.Port = database.Port;
            _model.User = database.User;
            _model.Password = database.Password;
            _model.Database = database.Database;

            _view.Address = _model.Address;
            _view.Port = _model.Port;
            _view.User = _model.User;
            _view.Password = _model.Password;
            _view.Database = _model.Database;
        }
        private void GetModel()
        {
            _model.Address = _view.Address;
            _model.Port = _view.Port;
            _model.User = _view.User;
            _model.Password = _view.Password;
            _model.Database = _view.Database;
        }
        private void ConnectTest(object sender, EventArgs e)
        {
            try
            {
                GetModel();
                _service.ConnectTest(_model);
                _view.ShowMessage("DB 연결 성공");
            }
            catch(Exception ex)
            {
                _view.ShowMessage("DB 연결 실패 :" + ex.Message);
            }
        }

        private void SaveDatabaseInfo(object sender, EventArgs e)
        {
            try
            {
                GetModel();
                DatabaseDto database = new DatabaseDto
                {
                    Address = _model.Address,
                    Port = _model.Port,
                    User = _model.User,
                    Password = _model.Password,
                    Database = _model.Database
                };
                _service.SaveDatabaseInfo(database);
                _view.CloseForm();
            }
            catch(Exception ex)
            {
                _view.ShowMessage(ex.Message);
            }
        }

        private void CloseForm(object sender, EventArgs e)
        {
            var before = new DatabaseDto
            {
                Address = _model.Address,
                Port = _model.Port,
                User = _model.User,
                Password = _model.Password,
                Database = _model.Database
            };
            var after = new DatabaseDto
            {
                Address = _view.Address,
                Port = _view.Port,
                User = _view.User,
                Password = _view.Password,
                Database = _view.Database
            };
            bool chainged = _service.CheckChanged(before, after);
            if (chainged)
            {
                string message = "변경 사항이 있습니다.\n 저장 하지 않고 종료 하시겠습니까?";
                if (_view.ShowConfirmation(message))
                    _view.CloseForm();
                else
                    return;
            }
            else
            {
                _view.CloseForm();
            }
            
        }
    }
}
