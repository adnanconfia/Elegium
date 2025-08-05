using Elegium.Models.Professionals;
using Elegium.Models.Projects;
using Elegium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.ProjectCrews
{
    public class ProjectExternalUser
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneMobile { get; set; }
        public string PhoneOffice { get; set; }
        public string Fax { get; set; }
        public WorkingPosition Position { get; set; }
        public int? PositionId { get; set; }
        public int PositionDescription { get; set; }

        public Department Department { get; set; }
        public int? DepartmentId { get; set; }

        //Addresss
        public string UserStreet { get; set; }
        public string UserCity { get; set; }
        public string UserPostalCode { get; set; }
        public Country UserCountry { get; set; }
        public int? UserCountryId { get; set; }
        //Addresss
        public string CompanyName { get; set; }
        public string CompanyStreet { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyPostalCode { get; set; }
        public Country CompanyCountry { get; set; }
        public int? CompanyCountryId { get; set; }

        //Skills
        public string Certificates { get; set; }
        public string EducationTraining { get; set; }
        public string CV { get; set; }

        //Food
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public bool IsHalal { get; set; }
        public bool IsKosher { get; set; }
        public string Allergies { get; set; }

        //Driving license
        public bool IsDrivingLicense { get; set; }
        public string DrivingLicenseClasses { get; set; }

        public string Note { get; set; }

        //Contract
        public DateTime? ContainsLOI { get; set; }
        public DateTime? ContainsDealMemo { get; set; }
        public DateTime? ContractCreated { get; set; }
        public DateTime? ContractSent { get; set; }
        public DateTime? ContractSigned { get; set; }
        public string Message { get; set; }
    }
}
