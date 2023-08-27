using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.RequestModel
{
    public class AssignClassRequest
    {
        public Guid UserID { get; set; }
        public Guid ClassID { get; set; }
    }
}
