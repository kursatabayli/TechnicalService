using System.Net;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.DTOs.Results
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode Status { get; set; }
        public string StatusMessage { get; set; }
        public StatusCode StatusCode { get; set; } = StatusCode.None;

        public static Result Default(bool isSuccess, string message, StatusCode statusCode, HttpStatusCode status)
            => new() { IsSuccess = isSuccess, Status = status, StatusMessage = message, StatusCode = statusCode };
        public static Result Success(string message = "İşlem Başarılı", HttpStatusCode status = HttpStatusCode.OK)
            => new() { IsSuccess = true, Status = status, StatusMessage = message };

        public static Result Failure(string message, StatusCode statusCode, HttpStatusCode status = HttpStatusCode.BadRequest)
            => new() { IsSuccess = false, Status = status, StatusMessage = message, StatusCode = statusCode };


        public static implicit operator bool(Result result) => result.IsSuccess;
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }
        public static Result<T> Success(T data, string message = "İşlem Başarılı", HttpStatusCode status = HttpStatusCode.OK)
            => new() { IsSuccess = true, Status = status, StatusMessage = message, Data = data };

        public static new Result<T> Failure(string message, StatusCode statusCode, HttpStatusCode status = HttpStatusCode.BadRequest)
            => new() { IsSuccess = false, Status = status, StatusMessage = message, StatusCode = statusCode, Data = default };
            public static new Result<T> Failure(T Data,string message, StatusCode statusCode, HttpStatusCode status = HttpStatusCode.BadRequest)
            => new() { IsSuccess = false, Status = status, StatusMessage = message, StatusCode = statusCode, Data = Data };

    }
}
