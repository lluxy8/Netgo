namespace Netgo.Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public int StatusCode { get; }
        public bool HasValue { get; protected set; }

        public virtual object? Value => null;

        protected Result(bool isSuccess, string errorMessage, int statusCode)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
            HasValue = false;
        }

        public static Result Success(int statusCode = 200)
            => new(true, string.Empty, statusCode);

        public static Result Failure(string error, int statusCode)
            => new(false, error, statusCode);
    }

    public class Result<T> : Result
    {
        public T Data { get; }                
        public override object? Value => Data; 

        private Result(T value, bool isSuccess, string error, int statusCode)
            : base(isSuccess, error, statusCode)
        {
            Data = value;
            HasValue = true;
        }

        public static Result<T> Success(T value, int statusCode = 200)
            => new(value, true, string.Empty, statusCode);

        public static new Result<T> Failure(string error, int statusCode)
            => new(default!, false, error, statusCode);
    }
}
