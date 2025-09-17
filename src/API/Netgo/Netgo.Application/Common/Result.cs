namespace Netgo.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
        public bool IsGeneric { get; set; }

        protected Result(bool isSuccess, string errorMessage, int statusCode)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
            IsGeneric = false;
        }

        public static Result Success(int statusCode = 200) => new(true, string.Empty, statusCode);
        public static Result Failure(string error, int statusCode) => new(false, error, statusCode);
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        private Result(T value, bool isSuccess, string error, int statusCode)
            : base(isSuccess, error, statusCode)
        {
            Value = value;
            ErrorMessage = error;
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            IsGeneric = true;
        }

        public static Result<T> Success(T value, int statusCode = 200) => new(value, true, string.Empty, statusCode);

        public static new Result<T> Failure(string error, int statusCode) => new(default!, false, error, statusCode);
    }

}
