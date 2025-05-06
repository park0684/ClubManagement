using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Games.DTOs
{
    public class IndividualPlayerDto
    {
        public int seq { get; set; }
        public string Player { get; set; }
        public int Score { get; set; }
        public int Handi { get; set; }
        public int Rank { get; set; }
        public int AddHandi { get; set; }
    }
}
