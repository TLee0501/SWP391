namespace BusinessObjects.ResponseModel
{
    public class ProjectAndStatusResponse
    {
        public Guid ProjectId { get; set; }
        public Guid ClassID { get; set; }
        public string ClassName { get; set; } = null!;
        public string ProjectName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string RequestStatus { get; set; } = null!;
        public bool IsSelected { get; set; }
    }
}