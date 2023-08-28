﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.RequestModel
{
    public class CreateClassRequest
    {
        public Guid CourseId { get; set; }
        public Guid UserId { get; set; }
        public string ClassName { get; set; } = null!;
        public Guid SemesterId { get; set; }
        public string EnrollCode { get; set; } = null!;
    }
}
