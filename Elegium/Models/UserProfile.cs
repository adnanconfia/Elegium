using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        
        //General Info
        public byte[] Photo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string StreetAddress { get; set; }
        public string StreetAddress2 { get; set; }
        public Country Country { get; set; }
        public int? CountryId { get; set; }
        public City City { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }

        public string State { get; set; }
        public string PostCode { get; set; }
        public string IntroText { get; set; }
        public string Email { get; set; }
        public string Skype { get; set; }
        public LanguageLevel EnglishLevel { get; set; }
        public int? EnglishLevelId { get; set; }
        public string OtherLanguage { get; set; }

        //Web links
        public string FacebookLink { get; set; }
        public bool FacebookLinkIsPublic { get; set; }
        public string InstagramLink { get; set; }
        public bool InstagramLinkIsPublic { get; set; }
        public string LinkedinLink { get; set; }
        public bool LinkedinLinkIsPublic { get; set; }
        public string PinterestLink { get; set; }
        public bool PinterestLinkIsPublic { get; set; }
        public string TwitterLink { get; set; }
        public bool TwitterLinkIsPublic { get; set; }
        public string YoutubeLink { get; set; }
        public bool YoutubeLinkIsPublic { get; set; }
        public string ImdbLink { get; set; }
        public bool ImdbLinkIsPublic { get; set; }
        public string VimeoLink { get; set; }
        public bool VimeoLinkIsPublic { get; set; }

        //Company Info
        public byte[] CompanyLogo { get; set; }
        public string CompanyName { get; set; }
        public CompanyType CompanyType { get; set; }
        public int? CompanyTypeId { get; set; }
        public bool CompanyLegalRepresent { get; set; }
        public WorkingPosition CompanyPosition { get; set; }
        public int? CompanyPositionId { get; set; }
        public string CompanyStreetAddress { get; set; }
        public string CompanyStreetAddress2 { get; set; }
        public Country CompanyCountry { get; set; }
        public int? CompanyCountryId { get; set; }
        public City CompanyCity { get; set; }
        public int? CompanyCityId { get; set; }
        public string CompanyCityName { get; set; }
        public string CompanyState { get; set; }
        public string CompanyPostCode { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyWeb { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyStudioAddress { get; set; }
        public string CompanyStudioAddress2 { get; set; }
        public Country CompanyStudioCountry { get; set; }
        public int? CompanyStudioCountryId { get; set; }
        public City CompanyStudioCity { get; set; }
        public int? CompanyStudioCityId { get; set; }
        public string CompanyStudioCityName { get; set; }
        public string CompanyStudioState { get; set; }
        public string CompanyStudioPostCode { get; set; }
        public string CompanyStudioEmail { get; set; }
        public string CompanyStudioWeb { get; set; }


        //Self Promotion Page
        public string PromotionCategories { get; set; }
        public Skill Skill { get; set; }
        public int? SkillId { get; set; }
        public SkillLevel SkillLevel { get; set; }
        public int? SkillLevelId { get; set; }
        public CompanyType OwnCompanyType { get; set; }
        public int? OwnCompanyTypeId { get; set; }
        public string OwnCompanyName { get; set; }

        //Link to the application user
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        //age weight etc

        public string Age { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string EyeColor { get; set; }
        public string HairColor { get; set; }
        public string Build { get; set; }
        public string SkinColor { get; set; }

        [NotMapped]
        public bool? Online { get; set; } = false;
        [NotMapped]
        public bool Following { get; set; } = false;

        //background image
        public byte[] BackgroundImage { get; set; }
        public string BgColor { get; set; } = "#fff";
        public double? BgOpacity { get; set; } = 0;
        public bool DarkMode { get; set; }
        public bool GlassMode { get; set; }
        public bool CinematicMode { get; set; }
    }
}
