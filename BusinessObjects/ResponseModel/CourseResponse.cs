using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ResponseModel
{
    public class CourseResponse
    {
        public Guid CourseId { get; set; }
        public Guid UserId { get; set; }
        public string createdBy { get; set; }
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public DateTime TimeCreated { get; set; }
    }
}
