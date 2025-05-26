using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ClubManagement.Options.Repositries
{
    public interface IOptionRepository
    {
        DataTable LoadHdaniSet(int code);
        DataTable LoadClupInfo(int code);
    }
}
