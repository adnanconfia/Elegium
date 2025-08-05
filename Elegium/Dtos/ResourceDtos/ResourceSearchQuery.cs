using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.ResourceDtos
{
    public class ResourceSearchQuery
    {
        public int EquipmentCategoryId { get; set; }
        public int ConditionId { get; set; }
        public string HireOrSale { get; set; }
        public int MinRentalPeriod { get; set; }
        public int MaxRentalPeriod { get; set; }
        public int RentalPrice { get; set; }
        public int CurrencyId { get; set; }
        public int CountryId { get; set; }
        public bool Insured { get; set; }
        public int SalePrice { get; set; }
    }
}
