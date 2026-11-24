namespace ToirProfAPI.Dtos;

public class ApiResponce<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    
    public static ApiResponce<T> Ok(T data) => new ApiResponce<T> { Success = true, Data = data };
    public static ApiResponce<T> Fail(string message) => new ApiResponce<T> { Success = false, Message = message };
}