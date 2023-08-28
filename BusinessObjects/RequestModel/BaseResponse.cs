using BusinessObjects.Enums;

namespace BusinessObjects.RequestModel
{
    public class BaseResponse<T>
    {
        public ErrorCode Code { get; set; }
        public string Message { get; set; } = null!;
        public T? Data { get; set; }
    }
}

