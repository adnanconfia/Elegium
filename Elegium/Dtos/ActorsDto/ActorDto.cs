using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.ActorsDto
{
    public class ActorDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Union { get; set; }
        public string PhoneHome { get; set; }
        public string RealName { get; set; }
        public string DOB { get; set; }
        public string PhoneMobile { get; set; }
        public string Fax { get; set; }
        public string FirstStreet { get; set; }
        public string FirstCity { get; set; }
        public string FirstPostalCode { get; set; }
        public string FirstCountry { get; set; }
        public string SecondStreet { get; set; }
        public string SecondCity { get; set; }
        public string SecondPostalCode { get; set; }
        public string SecondCountry { get; set; }
        public string ProdStreet { get; set; }
        public string ProdCity { get; set; }
        public string ProdPostalCode { get; set; }
        public string ProdCountry { get; set; }
        public string Gender { get; set; }
        public int AgeMin { get; set; }
        public int AgeMax { get; set; }
        public string Languages { get; set; }
        public string Talent { get; set; }
        public string VocalRange { get; set; }
        public string Instruments { get; set; }
        public string Ethnicity { get; set; }
        public string BodyType { get; set; }
        public string EyeColor { get; set; }
        public string HairLength { get; set; }
        public string HairColor { get; set; }
        public string HairType { get; set; }
        public string FacialHair { get; set; }
        public bool WillCutHair { get; set; }
        public bool WillShave { get; set; }
        public bool WearsGlasses { get; set; }
        public bool HasTattoo { get; set; }
        public bool HasPiercings { get; set; }
        public string AppearanceNote { get; set; }
        public string ShirtSize { get; set; }
        public string PantsSize { get; set; }
        public string DressSize { get; set; }
        public string BraSize { get; set; }
        public string GloveSize { get; set; }
        public string ShoeSize { get; set; }
        public string BodyHeight { get; set; }
        public string NeckToFloor { get; set; }
        public string HeadGirth { get; set; }
        public string NeckGirth { get; set; }
        public string SoulderLength { get; set; }
        public string BackWidth { get; set; }
        public string BackLength { get; set; }
        public string ChestGirth { get; set; }
        public string UnderChestGirth { get; set; }
        public string NeckToBreast { get; set; }
        public string NeckToWaist { get; set; }
        public string FrontWaistLength { get; set; }
        public string WaistCircumference { get; set; }
        public string HipSize { get; set; }
        public string HipHeightFromWaist { get; set; }
        public string ArmLength { get; set; }
        public string UpperArmGirth { get; set; }
        public string ArmBedGirth { get; set; }
        public string ArmBedLength { get; set; }
        public string HandGirth { get; set; }
        public string Inseam { get; set; }
        public string InnerLengthOfLeg { get; set; }
        public string OuterLengthOfLeg { get; set; }
        public string CalfGirth { get; set; }
        public string ThighGirth { get; set; }
        public string KneeHeight { get; set; }
        public string KneeCircumference { get; set; }
        public string AnkleWidth { get; set; }
        public string FootLength { get; set; }
        public string FrontFootWidth { get; set; }
        public string FootWidth { get; set; }
        public bool IsVeg { get; set; }
        public bool IsVegan { get; set; }
        public bool IsHalal { get; set; }
        public bool IsKosher { get; set; }
        public string Allergies { get; set; }
        public string Note { get; set; }
        public bool Is_deleted { get; set; }

        public int? AgencyId { get; set; }
 
       

        public int? ProjectId { get; set; }
        public bool Default { get; set; }
        public bool HasFile { get; set; }
        public DocumentFilesDto? file { get; set; }
    }
}
