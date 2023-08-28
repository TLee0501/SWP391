namespace BusinessObjects.RequestModel
{
    public class UpdateClassRequest
    {
        public Guid ClassId { get; set; }
        public string ClassName { get; set; } = null!;
        public string EnrollCode { get; set; } = null!;
        public Guid? TeacherId { get; set; }
    }
}
