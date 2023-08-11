using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.RequestModel
{
    public class CourseCreateRequest
    {
        public Guid UserId { get; set; }
        public string CourseName { get; set; } = null!;
    }
}
