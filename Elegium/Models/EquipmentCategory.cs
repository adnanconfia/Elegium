using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class EquipmentCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EquipmentCategoryType EquipmentCategoryType { get; set; }
        public int EquipmentCategoryTypeId { get; set; }
    }
}
