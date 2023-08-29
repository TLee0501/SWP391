namespace BusinessObjects.Enums
{
    public enum ErrorCode
    {
        Success = 99,
        Error = 100,
        // Semester
        SemesterOverlapTime = 1001,
        SemesterStartTimeEqualEndTime = 1002,
        SemesterMinDays = 1003,
        // Project
        ProjectDuplicatedName = 15,
        // Team
        TeamDuplicatedMember = 8,
        TeamDuplicated = 9,
    }
}

