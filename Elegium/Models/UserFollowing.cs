using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class UserFollowing
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public ApplicationUser FollowingTo { get; set; }
        public string FollowingToId { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    }
}
