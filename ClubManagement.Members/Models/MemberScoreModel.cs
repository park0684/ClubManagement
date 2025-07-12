using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClubManagement.Members.DTOs;

namespace ClubManagement.Members.Models
{
    public class MemberScoreModel
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

        public MemberScoreModel(string start)
        {
            StartDate = start;
        }

        public static MemberScoreModel FromDto(SearchScoreDto dto)
        {
            var result = new MemberScoreModel(dto.StartDate)
            {
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                Status = dto.Status,
                SortType = dto.SortType,
                SearchWord = dto.SearchWord,
                IsSearchDate = dto.IsSearchDate,
                IsSearchMatch = dto.IsSearchMatch,
                IsExcludedMember = dto.IsExcludedMember
            };
            return result;
        }

        public SearchScoreDto ToDto()
        {
            return new SearchScoreDto
            {
                FromDate = this.FromDate,
                ToDate = this.ToDate,
                StartDate = this.StartDate,
                Status = this.Status,
                SortType = this.SortType,
                SearchWord = this.SearchWord,
                IsSearchDate = this.IsSearchDate,
                IsSearchMatch = this.IsSearchMatch,
                IsExcludedMember = this.IsExcludedMember
            };
        }
        public void UpdateSeachCondition()
        {
            if (IsSearchMatch)
                IsSearchDate = false;
            else
                IsSearchDate = true;
        }
    }
}
