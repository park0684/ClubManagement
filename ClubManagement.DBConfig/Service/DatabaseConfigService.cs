using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ClubManagement.Common.DTOs;
using ClubManagement.Common.Repositories;
using ClubManagement.Common.Hlepers;
using ClubManagement.Common.Models;

namespace ClubManagement.DBConfig.Service
{
    public class DatabaseConfigService
    {
        BaseRepository _repository;
        const string section = "Database Config";
        IniFileHelper _ini;
        DatabaseModel _model;
        public DatabaseConfigService()
        {
            _repository = new BaseRepository();
            _ini = new IniFileHelper();
            _model = new DatabaseModel();
        }
        public DatabaseDto LoadConnectionInfo()
        {
            var info = new DatabaseDto
            { 
                Address = _ini.Read("Database Config", "Address"),
                Port = Convert.ToInt32(_ini.Read("Database Config", "Port")),
                User = _ini.Read("Database Config", "User"),
                Password = _ini.Read("Database Config", "Password"),
                Database = _ini.Read("Database Config", "Database"),
            };
            
            return info;
        }
        public void ConnectTest(DatabaseModel model)
        {
            string connectionString = model.ToConnectionString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open(); // 연결이 실패하면 예외 발생
                // 연결 성공 시 아무것도 하지 않음
            }
        }
        public void SaveDatabaseInfo(DatabaseDto info)
        {
            _ini.Write("Database Config", "Address", info.Address);
            _ini.Write("Database Config", "Port", info.Port.ToString());
            _ini.Write("Database Config", "Database", info.Database);
            _ini.Write("Database Config", "User", info.User);
            _ini.Write("Database Config", "Password", info.Password);
        }
        public bool CheckChanged(DatabaseDto before, DatabaseDto after)
        {
            bool changed = false;
            foreach (var prop in typeof(DatabaseDto).GetProperties())
            {
                object beforeValue = prop.GetValue(before);
                object afterValue = prop.GetValue(after);

                if ((beforeValue == null && afterValue != null) ||
                    (beforeValue != null && !beforeValue.Equals(afterValue)))
                {
                    changed = true;
                    return changed;
                }
            }
            return changed;
        }
    }
}
