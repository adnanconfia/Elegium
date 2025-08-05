using Elegium.Dtos;
using Elegium.Models;
using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.ViewModels.FundingAndFP
{
    public class UserFundingAndFPDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public float BudgetUpto { get; set; }
        public ProductionType ProductionType { get; set; }
        public int? ProductionTypeId { get; set; }
        public Currency Currency { get; set; }
        public int? CurrencyId { get; set; }
        public Country Country { get; set; }
        public int? CountryId { get; set; }
        public string CityName { get; set; }
        public City City { get; set; }
        public int? CityId { get; set; }
        public float Offer { get; set; }
        public string OfferShare { get; set; }
        public ProjectManagementPhases ProjectPhase { get; set; }
        public Guid? ProjectPhaseId { get; set; }
        public string SupportDetail { get; set; }
        public string OtherRequirements { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public UserProfileDto User { get; set; }
        public string UserId { get; set; }
        public bool IsSaved { get; set; }
        public bool IsFavorite { get; set; }

    }
}
