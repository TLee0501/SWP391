using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ResponseModel
{
    public class ClassResponse
    {
        public Guid ClassId { get; set; }
        public Guid UserId { get; set; }
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string ClassName { get; set; } = null!;
        public string EnrollCode { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
