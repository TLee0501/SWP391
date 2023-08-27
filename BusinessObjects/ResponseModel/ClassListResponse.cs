namespace BusinessObjects.ResponseModel
{
    public class ClassListResponse
    {
        public Guid ClassId { get; set; }
        public Guid UserId { get; set; }
        public string TeacherName { get; set; } = null!;
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string ClassName { get; set; } = null!;
        public string EnrollCode { get; set; } = null!;
        public bool? Enrolled { get; set; }
    }
}

