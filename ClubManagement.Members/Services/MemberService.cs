using ClubManagement.Common.Hlepers;
using ClubManagement.Members.Models;
using ClubManagement.Members.Repositories;
using System;
using System.Data;

namespace ClubManagement.Members.Services
{
    public class MemberService : IMemberService
    {
        IMemberRepository _reposotory;
        public MemberService(IMemberRepository repository)
        {
            _reposotory = repository;
        }
        public string LoadStartDate()
        {
            return _reposotory.LoadStartDate();
        }

        public DataTable LoadMemberList(MemberSearchModel model)
        {
            var result = _reposotory.LodaMemberList(model);

            int i = 1;
            result.Columns.Add("No");
            result.Columns.Add("status");
            result.Columns.Add("position");
            result.Columns.Add("gender");
            result.Columns.Add("regularRate", typeof(double));
            result.Columns.Add("nonPayment");
            foreach (DataRow row in result.Rows)
            {
                row["No"] = i++;
                row["status"] = MemberHelper.GetMemberStatus(Convert.ToInt32(row["mem_status"]));
                row["gender"] = MemberHelper.GetMemberGenger(Convert.ToInt32(row["mem_gender"]));
                row["position"] = MemberHelper.GetMemberPositon(Convert.ToInt32(row["mem_position"]));
                int nonPayment = Convert.ToInt32(row["mem_dues"]) - Convert.ToInt32(row["Payment"]);
                row["nonPayment"] = nonPayment < 0 ? 0 : nonPayment;
                double rate = DataConvertHelper.ConvertRate(row["game_count"], row["reglar_count"]);
                row["regularRate"] = rate;
            }
            result.Columns.Remove("mem_status");
            result.Columns.Remove("mem_gender");
            result.Columns.Remove("mem_position");
            result.Columns.Remove("game_count");
            result.Columns.Remove("mem_dues");
            result.AcceptChanges();

            return result;
        }
        public DataTable LoadSearchMember(MemberSearchModel model)
        {
            var resutl = _reposotory.LoadSearchMember(model);
            return resutl;
        }

        public MemberModel LoadMemberInfo(int memberCode)
        {
            var result = _reposotory.LoadMemberInfo(memberCode);
            return new MemberModel
            {
                IsNew = false,
                Code = memberCode,
                MemberName = result["mem_name"].ToString().Trim(),
                Status = Convert.ToInt32(result["mem_status"]),
                Gender = Convert.ToInt32(result["mem_gender"]),
                Position = Convert.ToInt32(result["mem_position"]),
                Birth = result["mem_birth"].ToString().Trim(),
                Memo = result["mem_memo"].ToString().Trim(),
                AccessDate = (DateTime)result["mem_access"],
                SecessDate = result["mem_secess"] == DBNull.Value ? DateTime.Now : (DateTime)result["mem_secess"]
            };
        }

        public void SaveMember(MemberModel model)
        {
            if (model.IsNew)
                _reposotory.InsertMember(model);
            else
                _reposotory.UpdateMember(model);
        }
    }
}
