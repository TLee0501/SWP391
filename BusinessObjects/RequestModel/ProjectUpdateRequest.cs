using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.RequestModel
{
    public class ProjectUpdateRequest
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
