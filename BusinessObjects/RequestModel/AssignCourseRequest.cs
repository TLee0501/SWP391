using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.RequestModel
{
    public class AssignCourseRequest
    {
        public Guid userID { get; set; }
        public Guid courseID { get; set; }
    }
}
