namespace BusinessObjects.ResponseModel
{
    public class SemesterResponse
    {
        public Guid SemesterId { get; set; }
        public string SemeterName { get; set; } = null!;
        public Guid SemesterTypeId { get; set; }
        public string SemeterTypeName { get; set; } = null!;
    }
}
