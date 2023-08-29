namespace BusinessObjects.ResponseModel
{
    public class SemesterDetailResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<ClassSemesterDetailResponse> Classes { get; set; } = null!;
    }

    public class ClassSemesterDetailResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = null!;
    }
}

