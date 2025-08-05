using Elegium.Models;
using Elegium.Models.Agency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.AgencyDto
{
    public class AgencyDto
    {
        public Agency Agency { get; set; }
        public List<AgencyContact> AgencyContact { get; set; }
        public bool Default { get; set; }
        public bool HasFile { get; set; }
        public DocumentFilesDto? file { get; set; }
    }
}
