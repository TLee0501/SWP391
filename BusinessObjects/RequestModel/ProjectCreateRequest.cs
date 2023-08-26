namespace BusinessObjects.RequestModel
{
    public class ProjectCreateRequest
    {
        public Guid ClassId { get; set; }
        public string ProjectName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? FunctionalReq { get; set; }
        public string? NonfunctionalReq { get; set; }
    }
}
