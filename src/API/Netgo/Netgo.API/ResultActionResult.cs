using Microsoft.AspNetCore.Mvc;
using Netgo.Application.Common;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;

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
            object valueToReturn = _result;

            if (!_result.IsSuccess)
            {
                var json = new JObject();
                json["error"] = _result.ErrorMessage;
                valueToReturn = json.ToString();
            }


            if (!_result.IsGeneric)
            {
                valueToReturn = string.Empty;
            }

            if (_result.IsGeneric && _result.IsSuccess)
            {
                valueToReturn = _result.GetType().GetProperty("Value")?.GetValue(_result)!;
            }


            var objectResult = new ObjectResult(valueToReturn)
            {
                StatusCode = _result.StatusCode
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}
