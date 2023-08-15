using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.RequestModel
{
    public class CourseCreateRequest
    {
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
    }
}
