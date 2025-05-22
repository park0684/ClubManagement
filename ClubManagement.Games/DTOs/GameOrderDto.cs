using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Games.DTOs
{
    public class GameOrderDto
    {
        public int GameSeq { get; set; }
        public int GameType { get; set; }
        public int PlayerCount { get; set; }
        public bool AllCoverGame { get; set; }
        public bool PersonalSideGame { get; set; }
        public List<GroupDto> Groups { get; set; } = new List<GroupDto>();
        public List<IndividualPlayerDto> IndividualPlayers { get; set; } = new List<IndividualPlayerDto>();
    }
}
