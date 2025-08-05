using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class BackgroundThingsDto
    {
        public int? ProjectId { get; set; }
        public IFormFile file { get; set; }
        public string BackgroundImage { get; set; }
        public string BackgroundOpacity { get; set; }
        public string BackgroundColor { get; set; }
        public bool DarkMode { get; set; }
        public bool GlassMode { get; set; }
        public bool CinematicMode { get; set; }
    }
}
