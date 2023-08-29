namespace BusinessObjects.ResponseModel
{
    public class ResponseCodeAndMessageModel
    {
        public int Code { get; set; }
        public string? Message { get; set; }

        public ResponseCodeAndMessageModel() { }

        public ResponseCodeAndMessageModel(int code, string? message)
        {
            Code = code;
            Message = message;
        }
    }
}
