namespace Proto.Common.Presentation;

public class ApiResponseWithData<T> : ApiResponse
{
    public T? Data { get; set; }
}
