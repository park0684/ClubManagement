using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Games.DTOs
{
    public class GroupDto
    {
        public int GroupNumber { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
        public List<PlayerInfoDto> players { get; set; } = new List<PlayerInfoDto>();
    }
}
