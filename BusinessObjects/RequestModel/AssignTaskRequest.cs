using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.RequestModel
{
    public class AssignTaskRequest
    {
        public Guid userID { get; set; }
        public string userName { get; set; }
        public Guid taskId { get; set; }
    }
}
