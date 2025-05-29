using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClubManagement.Members.Models;

namespace ClubManagement.Members.Services
{
    public interface IMemberService
    {
        DataTable LoadMemberList(MemberSearchModel model);
        DataTable LoadSearchMember(MemberSearchModel model);
        MemberModel LoadMemberInfo(int memberCode);
        string LoadStartDate();
        void SaveMember(MemberModel model);

    }
}
