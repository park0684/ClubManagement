using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Common.Models;

namespace ClubManagement.Common.Service
{
    public interface IDatabaseService
    {
        void ConnectTest();
        void SaveDatabaseInfo(DatabaseModel model);
    }
}
