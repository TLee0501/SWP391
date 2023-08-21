using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ResponseModel
{
    public class TaskResponse
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; }
        public Guid ProjectId { get; set; }
        public string TaskName { get; set; } = null!;
        public string TaskDescription { get; set; } = null!;
        public string Status { get; set; } = null!; 
    }
}
