using Elegium.Data.Migrations;
using Elegium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMessages = Elegium.Models.UserMessages;

namespace Elegium.Dtos
{
    public class UserMessageDTO
    {
        public UserMessages UserMessages { get; set; }
        public UserProfile UserProfile { get; set; }
    }

    public class ForwardMessage
    {
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string Message { get; set; }
    }

    public class ForwardMessageDTO
    {
        public ForwardMessage ForwardMessage { get; set; }
        public UserProfile UserProfile { get; set; }
        public string Url { get; set; }

    }
}
