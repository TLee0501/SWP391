using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.RequestModel
{
    public class UpdateClassRequest
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public Guid SemesterId { get; set; }
        public string EnrollCode { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public bool IsCompleted { get; set; }
    }
}
