using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Agency
{
    public class Agency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string City_State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneMobile { get; set; }
        public string Fax { get; set; }
        public string Note { get; set; }

        public bool Is_Deleted { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey("ProejectId")]

        public virtual Project Project { get; set; } 
    }
}
