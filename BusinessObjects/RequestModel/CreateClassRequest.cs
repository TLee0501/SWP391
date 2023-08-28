namespace BusinessObjects.RequestModel
{
    public class CreateClassRequest
    {
        public Guid SemesterId { get; set; }
        public Guid CourseId { get; set; }
        public Guid TeacherId { get; set; }
        public string ClassName { get; set; } = null!;
        public string EnrollCode { get; set; } = null!;
    }
}
