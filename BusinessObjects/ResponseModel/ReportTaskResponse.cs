namespace BusinessObjects.ResponseModel
{
    public class ReportTaskResponse
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public int NumberOfTask { get; set; }
        public int NumberOfDoingTask { get; set; }
        public int NumberOfFinishTask { get; set; }
        public int NumberOfUnFinishTask { get; set; }

    }
}
