using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Common.DTOs
{
    public class ScoreDto
    {
        public DateTime GameDate { get; set; }
        public string GameTitle { get; set; }
        public int TotalScore { get; set; }
        public Decimal GameAverage { get; set; }
        public List<int> GameScore { get; set; }
    }
}
