using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class ProfessionalSearchQuery
    {
        public int? PromotionCategory { get; set; }
        public int? CountryId { get; set; }
        public int? CompanyPositionId { get; set; }
        public int? SkillId { get; set; }
        public int? SkillLevelId { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        public int? CityId { get; set; }
    }
}
