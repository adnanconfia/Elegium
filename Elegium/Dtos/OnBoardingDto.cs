using Elegium.Dtos.ProjectDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class OnBoardingDto
    {
        public string name { get; set; }
        public bool completed { get; set; }
        public bool ignore_as_first_step { get; set; }
        public string font_icon { get; set; }
        public string url_hash { get; set; }
        public bool is_last { get; set; }
        public bool next_step { get; set; }
        public string description { get; set; }
        public int projectId { get; set; }
        public string parent_description { get; set; }
        public int parent_percentage { get; set; }
    }

    public class OnBoardOverlayDto
    {
        public ProjectDto Project { get; set; }
        public List<OnBoardingDto> OnBoardingDtoList { get; set; }

    }
}
