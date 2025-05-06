using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Games.DTOs;

namespace ClubManagement.Games.Models
{
    public class RecordBoardModel
    {
        public int MatchCode { get; set; }
        public string MatchTitle { get; set; }
        public int CurrentGame { get; set; }
        public int CurrentGroup { get; set; }
        //public List<GameOrderDto> GameList { get; set; }
        public List<GameOrderDto> GameList { get; set; } = new List<GameOrderDto>();
        public List<PlayerInfoDto> PlayerList { get; set; } = new List<PlayerInfoDto>();
        public List<PlayerInfoDto> SideGamePlayers { get; set; } = new List<PlayerInfoDto>();
        public List<PlayerInfoDto> AllcoverGamePlayers { get; set; } = new List<PlayerInfoDto>();
        public List<IndividaulSetDto> IndividaulSideSet { get; set; } = new List<IndividaulSetDto>();
        public List<IndividualPlayerDto> IndividualRanker { get; set; } = new List<IndividualPlayerDto>();
    }
}
