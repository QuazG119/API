namespace WebApi.Infrastructure.Shared.Models;

public class Response<T>
{
    public bool Succeeded { get; set; }
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public IEnumerable<string>? Errors { get; set; }

    public static Response<T> Success(T data, string message = "Success", int code = 200) => new()
    {
        Succeeded = true,
        Code = code,
        Message = message,
        Data = data
    };

    public static Response<T> Fail(string message, int code = 400, IEnumerable<string>? errors = null) => new()
    {
        Succeeded = false,
        Code = code,
        Message = message,
        Errors = errors
    };
}
