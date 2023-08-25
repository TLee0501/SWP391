namespace BusinessObjects.RequestModel
{
    public class AssignTaskRequest
    {
        public Guid MemberId { get; set; }
        public Guid TaskId { get; set; }
    }
}
