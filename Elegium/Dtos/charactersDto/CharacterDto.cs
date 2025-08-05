using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.charactersDto
{
    public class CharacterDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? GroupOfCharacters { get; set; } = false;
        public bool? Sugggestion { get; set; } = false;
        public string Description { get; set; }
        public bool? marked { get; set; } = false;
        public int? Index { get; set; }
        public int Project_Id { get; set; }
        public bool Default { get; set; }
        public bool HasFile { get; set; }
        public DocumentFilesDto? file { get; set; }
    }
}
