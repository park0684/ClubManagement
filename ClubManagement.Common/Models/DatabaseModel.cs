using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ClubManagement.Common.Models
{
    public class DatabaseModel
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public int Timeout { get; set; } = 5;
        public string ToConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = $"{Address},{Port}",
                UserID = User,
                Password = Password,
                InitialCatalog = Database,
                ConnectTimeout = Timeout
            };

            return builder.ConnectionString;
        }
    }
}
