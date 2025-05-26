using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Common.Hlepers;
using ClubManagement.Common.Models;
using ClubManagement.Common.Repositories;

namespace ClubManagement.Common.Service
{
    public class DatabaseService: IDatabaseService
    {
        const string Section = "Database Config";
        IniFileHelper _ini;
        BaseRepository _repository;
        
        public DatabaseService()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dbconn.ini");
            _ini = new IniFileHelper();

            if (!File.Exists(path))
                File.Create(path).Close();
        }

        
        public void ConnectTest()
        {
            _repository = new BaseRepository();
            using (var conn = _repository.OpenSql()) ;
        }

        public void SaveDatabaseInfo(DatabaseModel model)
        {
            var ini = new IniFileHelper();
            ini.Write("Database Config", "Address", model.Address);
            ini.Write("Database Config", "Port", model.Prer.ToString());
            ini.Write("Database Config", "Database", model.Database);
            ini.Write("Database Config", "User", model.User);
            ini.Write("Database Config", "Password", model.Password);
        }
    }
}
