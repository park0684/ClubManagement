using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Members.DTOs
{
    public class MemberDto
    {
        public int No { get; set; }
        public int MemberCode { get; set; }
        public string Name { get; set; }
        public string Brith { get; set; }
        public int Status { get; set; }
        public int Gender { get; set; }
        public int Position { get; set; }
        public int RegularMatch { get; set; }
        public int IrregularMatch { get; set; }
        public int EventMatch { get; set; }
        public double RegulaRate { get; set; }
        public int Payment { get; set; }
        public int NonPayament { get; set; }
        public DateTime? AccessDate { get; set; }
        public DateTime? SecessDate { get; set; }
        public DateTime? LastMatchDate { get; set; }
        public string Memo { get; set; }
        public int IsPro { get; set; }

    }
}
