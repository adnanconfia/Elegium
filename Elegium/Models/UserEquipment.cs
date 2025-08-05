using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class UserEquipment
    {
        public int Id { get; set; }
        public string EquipmentName { get; set; }
        public string LendingType { get; set; }
        public EquipmentCategory EquipmentCategory { get; set; }
        public int EquipmentCategoryId { get; set; }


        //Link to the application user
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
