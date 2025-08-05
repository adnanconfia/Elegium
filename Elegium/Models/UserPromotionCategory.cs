using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class UserPromotionCategory
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public PromotionCategory PromotionCategory { get; set; }
        public int PromotionCategoryId { get; set; }
    }
}
