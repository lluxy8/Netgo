using Microsoft.AspNetCore.Mvc;
using Netgo.Application.Common;

namespace Netgo.API
{
    public class ResultActionResult : IActionResult
    {
        private readonly Result _result;

        public ResultActionResult(Result result)
        {
            _result = result;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var valueToReturn = _result switch
            {
                { IsSuccess: false } => new
                {
                    code = _result.StatusCode,
                    message = _result.ErrorMessage
                },
                { HasValue: false } => string.Empty,
                _ => _result.Value   
            };

            var objectResult = new ObjectResult(valueToReturn)
            {
                StatusCode = _result.StatusCode
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}
