using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class UserMessages
    {
        public int Id { get; set; }
        public ApplicationUser ToUser { get; set; }
        public string ToUserId { get; set; }

        public ApplicationUser FromUser { get; set; }
        public string FromUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public Country Country { get; set; }
        public int CountryId { get; set; }
        public string PostCode { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

    }
}
