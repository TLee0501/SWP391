namespace BusinessObjects.ResponseModel
{
    public class ProjectAndStatusResponse
    {
        public Guid ProjectId { get; set; }
        public Guid ClassID { get; set; }
        public string ClassName { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsSelected { get; set; }
    }
}