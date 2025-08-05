using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Voting
{
    public class VotingSetting
    {
        public int Id { get; set; }
        public string SettingCode { get; set; }
        public string SettingDesc { get; set; }
        public string SettingValue { get; set; }
    }
}
