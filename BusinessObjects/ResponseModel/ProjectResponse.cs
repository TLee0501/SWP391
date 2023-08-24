﻿namespace BusinessObjects.ResponseModel
{
    public class ProjectResponse
    {
        public Guid ProjectId { get; set; }
        public Guid ClassID { get; set; }
        public string ClassName { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsSelected { get; set; }
    }
}