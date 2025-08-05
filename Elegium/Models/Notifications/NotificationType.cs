using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Notifications
{
    public class NotificationType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Template { get; set; }
        public string Type { get; set; } 
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        [NotMapped] 
        public string Action { get; set; }
    }
}
