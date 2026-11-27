using System.Text.Json.Serialization;

namespace ToirProfAPI.Dtos;

public class ApiResponce<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    
    [JsonPropertyName("error_code")]
    public string ? ErrorCode { get; set; }


    public static ApiResponce<T> Ok(T data, string? message = null) => new ApiResponce<T>
    {
        Success = true,
        Data = data,
        Message = message
    };

    public static ApiResponce<T> Fail(string message, string? errorCode = null) => new ApiResponce<T>
    {
        Success = false,
        Message = message,
        ErrorCode = errorCode

    };


}