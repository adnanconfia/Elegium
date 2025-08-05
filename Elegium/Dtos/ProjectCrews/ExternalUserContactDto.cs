using Elegium.Models.Professionals;
using Elegium.Models.Projects;
using Elegium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class ExternalUserContactDto
    {
        public int Id { get; set; }
        public int ExternalUserId { get; set; }
        public string Position { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneMobile { get; set; }
        public string PhoneOffice { get; set; }
        public string Fax { get; set; }
        public bool IsEdit { get; set; }
        public bool IsNew { get; set; }

    }
}
