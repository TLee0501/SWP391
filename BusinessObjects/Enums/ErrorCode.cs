namespace BusinessObjects.Enums
{
    public enum ErrorCode
    {
        Success = 99,
        Error = 100,
        // Semester
        SemesterDuplicatedName = 1,
        SemesterDuplicatedStartDate = 2,
        SemesterDuplicatedEndDate = 3,
        SemesterStartDateLessThanEndDate = 4,
        SemesterStartDateGreaterThanEndDate = 5,
        // Project
        ProjectDuplicatedName = 15,
        // Team
        TeamDuplicatedMember = 8,
        TeamDuplicated = 9,
    }
}

