using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class ProjectSearchQuery
    {
        public int? ProductionTypeId { get; set; }
        public string BudgetMin { get; set; }
        public string BudgetMax { get; set; }
        public int? LanguageId { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
    }
}
