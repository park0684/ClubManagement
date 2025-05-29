using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Members.Models;


namespace ClubManagement.Members.Services
{
    public interface IDuesService
    {
        DataTable LoadMemberList(DuesModel model);
        DataTable LoadStatementList(DuesModel model);
        StatementModel LoadStatement(int statementCode);
        void SaveStatement(StatementModel model);
    }
}
