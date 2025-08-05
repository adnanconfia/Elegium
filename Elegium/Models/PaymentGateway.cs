using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class PaymentGateway
    {
        public Guid Id { get; set; }
        public string ApiSecret { get; set; }
        public bool Default { get; set; }
        public string ApiKey { get; set; }
        public string Provider { get; set; }
    }
}
