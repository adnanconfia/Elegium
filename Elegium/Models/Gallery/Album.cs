using Elegium.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public class Album
    {
        [JsonConverter(typeof(IntToStringConverter))]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; } //P-> Photos, V-> Videos, A -> Audios
        public bool? AccessRight { get; set; } = false;
        public bool? Favorite { get; set; } = false;
    }
}
