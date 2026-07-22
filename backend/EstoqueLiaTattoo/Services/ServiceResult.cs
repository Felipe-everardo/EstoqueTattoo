namespace EstoqueLiaTattoo.Services;

public class ServiceResult<T>
{
    private ServiceResult(bool success, T? value, string code, string message)
    {
        Success = success;
        Value = value;
        Code = code;
        Message = message;
    }

    public bool Success { get; }
    public T? Value { get; }
    public string Code { get; }
    public string Message { get; }

    public static ServiceResult<T> Ok(T value) => new(true, value, string.Empty, string.Empty);

    public static ServiceResult<T> Fail(string code, string message) => new(false, default, code, message);
}
