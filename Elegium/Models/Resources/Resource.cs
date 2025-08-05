using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Resources
{
    public class Resource
    {
        public int Id { get; set; }
        public EquipmentCategory EquipmentCategory { get; set; }
        public int EquipmentCategoryId { get; set; }
        public string Name { get; set; }
        public ResourceCondition Condition { get; set; }
        public int? ConditionId { get; set; }
        public string Description { get; set; }
        public bool IsEquipment { get; set; }
        public string HireOrSale { get; set; } //Hire or Sale => H or S
        //Rental Props
        public int? MinRentalPeriod { get; set; }
        public int? MaxRentalPeriod { get; set; }
        public int? RentalPrice { get; set; }
        public Currency Currency { get; set; }
        public int CurrencyId { get; set; }
        public bool Insured { get; set; }
        public string RentalTerms { get; set; }
        public string LendingType { get; set; }
        //Sale Props
        public int? SalePrice { get; set; }
        //End Sale props

        public string Website { get; set; }
        public string Link { get; set; }
        public string YoutubeVideoLink { get; set; }
        public string VimeoVideoLink { get; set; }
        public string OtherTerms { get; set; }

        public DateTime CreateDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        //Link to the application user
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public bool Sold { get; set; }

    }
}
