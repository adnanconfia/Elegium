using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos
{
    public class FileMessageDto
    {
        public DocumentFilesDto FileObj { get; set; }
        public string[] SendTo { get; set; }
        public string Message { get; set; }
    }
}
