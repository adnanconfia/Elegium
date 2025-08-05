using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Interfaces
{
    public interface ITask
    {
        bool Completed { get; set; }
        bool HasDeadline { get; set; }
        DateTime? Deadline { get; set; }
    }
}
