using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class AgencyContact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneOffice { get; set; }
        public string PhoneMobile { get; set; }
        public string Fax { get; set; }
        public int? AgencyId { get; set; }
        [ForeignKey("AgencyId")]
        public virtual Agency.Agency Agency { get; set; }


    }
}
