namespace BusinessObjects.RequestModel
{
    public class CoursceUpdateRequest
    {
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
    }
}
