namespace HiWork.BLL.Responses
{
    public class BaseResponse
    {
        public string Id { get; set; }
        public bool IsOperationSuccessful { get; set; }
        public string Message { get; set; }
    }
}
