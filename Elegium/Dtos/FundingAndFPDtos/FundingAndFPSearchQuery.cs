using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.FundingAndFPDtos
{
    public class FundingAndFPSearchQuery
    {
        public string Type { get; set; }
        public int? ProductionTypeId { get; set; }
        public int? CityId { get; set; }
        public int? CountryId { get; set; }
        public string ProjectPhaseId { get; set; }
        public float? BudgetUpto { get; set; }
        public int? CurrencyId { get; set; }
    }
}
