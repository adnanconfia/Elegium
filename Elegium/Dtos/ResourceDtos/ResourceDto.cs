using Elegium.Models;
using Elegium.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.ResourceDtos
{
    public class ResourceDto
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
        public List<String> MediaFileIds { get; set; }
        public string UserId { get; set; }

        public UserProfileDto ResourceOwner { get; set; }
        public int ResourceOwnerId { get; set; }
        public bool IsSaved { get; set; }
        public bool IsFavorite { get; set; }
        public ProjectResourceDto ProjectResourceDto { get; set; }
        public ResourceDto()
        {
            ProjectResourceDto = new ProjectResourceDto();
        }
        public string Status { get; set; }
        public string Action { get; set; }
    }
}
