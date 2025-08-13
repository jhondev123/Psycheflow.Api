namespace Psycheflow.Api.Dtos.Responses
{
    public class GenericResponseDto<T>
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
        public T? Data { get; set; }

        public GenericResponseDto() { }

        public GenericResponseDto(string message, bool success, T? data)
        {
            Message = message;
            Success = success;
            Data = data;
        }

        public static GenericResponseDto<T> ToSuccess(string message, T? data)
        {
            return new GenericResponseDto<T>(message, true, data);
        }

        public static GenericResponseDto<T> ToFail(string message, T? data = default)
        {
            return new GenericResponseDto<T>(message, false, data);
        }
        public static GenericResponseDto<T> ToException(Exception ex, T? data = default)
        {
            return new GenericResponseDto<T>(ex.Message, false, data);
        }
    }
}
