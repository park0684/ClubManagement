using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Members.Models
{
    public class MemberSearchModel
    {
        public string StartDate { get; set; }
        public string SearchWord { get; set; }
        public int? Status { get; set; }
        public DateTime? AccFromDate { get; set; }
        public DateTime? AccToDate { get; set; }
        public DateTime? SecFromDate { get; set; }
        public DateTime? SecToDate { get; set; }
        public DateTime? GameFromDate { get; set; }
        public DateTime? GameTodate { get; set; }
        public bool ExcludeRegular { get; set; }
        public bool ExcludeIrregular { get; set; }
        public bool ExcludeEvent { get; set; }
        public bool ExcludeMember { get; set; }
        public bool AccessCheck { get; set; }
        public bool SecessCheck { get; set; }
        public bool GameCheck { get; set; }
        public bool IsInclude { get; set; }

        public List<string> SetWhereCondition()
        {
            var condition = new List<string>();
            if (!string.IsNullOrEmpty(SearchWord))
                condition.Add($"mem_name LIKE '%{SearchWord}%'");
            if (Status != 0)
                if (ExcludeMember)
                {
                    condition.Add($"mem_status != {Status}");
                }
                else
                { condition.Add($"mem_status = {Status}"); }
            if (AccessCheck)
                condition.Add($"mem_access >=  '{ AccFromDate.Value.ToString("yyyy-MM-dd")}' AND mem_access < '{AccToDate.Value.AddDays(1).ToString("yyyy-MM-dd")}'");
            if (SecessCheck)
                condition.Add($"mem_secess >=  '{ SecFromDate.Value.ToString("yyyy-MM-dd")}' AND mem_secess < '{SecToDate.Value.AddDays(1).ToString("yyyy-MM-dd")}'");
            if (GameCheck)
                condition.Add($"mem_code IN ( SELECT att_memcode FROM attend, match WHERE match_code = att_code AND match_date >='{ GameFromDate.Value.ToString("yyyy-MM-dd")}' AND match_date < '{GameTodate.Value.AddDays(1).ToString("yyyy-MM-dd")}')");

            return condition;
        }

    }
}
