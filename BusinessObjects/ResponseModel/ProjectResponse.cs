﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ResponseModel
{
    public class ProjectResponse
    {
        public Guid ProjectId { get; set; }
        public Guid CourseId { get; set; }
        public string CourseName { get; set; }
        public string ProjectName { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}