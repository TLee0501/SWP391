using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ResponseModel
{
    public class UserBasicResponse
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = null!;
    }
}
