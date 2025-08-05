using Elegium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.ViewModels
{
    public class UserProfileViewModel
    {
        public UserProfile UserProfile { get; set; }
        public List<UserCredit> UserCredits { get; set; }
        public List<UserEquipment> UserEquipments { get; set; }

        public List<UserAdditionalSkills> UserAdditionalSkills { get; set; }

        public int[] UserPromotionCategory { get; set; }

        public int[] UserOtherLanguages { get; set; }
        public string UserImage { get; set; }
        public string CompanyImage { get; set; }
    }
}
