﻿namespace BusinessObjects.ResponseModel
{
    public class ClassDetailResponseSemester
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
    public class ClassDetailResponseProject
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? FunctionalReq { get; set; }
        public string? NonfunctionalReq { get; set; }
    }

    public class ClassDetailResponseStudent
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

    public class ClassDetailResponseTeam
    {
        public Guid Id { get; set; }
        public ClassDetailResponseProject Project { get; set; } = null!;
        public ClassDetailResponseStudent Leader { get; set; } = null!;
        public List<ClassDetailResponseStudent> Members { get; set; } = null!;
    }

    public class ClassDetailResponse
    {
        public Guid ClassId { get; set; }
        public Guid? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string ClassName { get; set; } = null!;
        public string EnrollCode { get; set; } = null!;
        public bool? Enrolled { get; set; }
        public List<ClassDetailResponseProject> Projects { get; set; } = null!;
        public List<ClassDetailResponseStudent> Students { get; set; } = null!;
        public List<ClassDetailResponseTeam> Teams { get; set; } = null!;
        public ClassDetailResponseSemester Semester { get; set; } = null!;
        public DateTime? RegisterTeamStartDate { get; set; }
        public DateTime? RegisterTeamEndDate { get; set; }
        public DateTime? ReportStartDate { get; set; }
        public DateTime? ReportEndDate { get; set; }
    }
}
