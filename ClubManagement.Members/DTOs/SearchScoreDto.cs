using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Members.DTOs
{
    public class SearchScoreDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string StartDate { get; set; }
        public int Status { get; set; }
        public int SortType { get; set; }
        public string SearchWord { get; set; }
        public bool IsSearchDate { get; set; }
        public bool IsSearchMatch { get; set; }
        public bool IsExcludedMember { get; set; }
    }
}
