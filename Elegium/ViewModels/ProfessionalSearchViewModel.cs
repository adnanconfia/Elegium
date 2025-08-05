using Elegium.Dtos;
using Elegium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.ViewModels
{
    public class ProfessionalSearchViewModel
    {
        public UserProfileDto UserProfile { get; set; }
        public List<UserCredit> UserCredits { get; set; }
        public List<UserEquipment> UserEquipments { get; set; }

        public List<UserPromotionCategory> UserPromotionCategory { get; set; }

        public List<UserOtherLanguages> UserOtherLanguages { get; set; }
        public bool? Online { get; set; } = false;
        public bool? IsFollowing { get; set; } = false;
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public bool IsSaved { get; set; }
        public bool IsFavorite { get; set; }
    }
}
