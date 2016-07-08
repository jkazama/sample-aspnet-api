using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Net;

namespace Sample.Context.Rest
{
    //<summary>
    // REST用の例外Map変換サポート。
    //</summary>
    class RestExceptionFilter : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = ResultBuilder.Of(context.Exception).Result();
        }
    }

    class ResultBuilder
    {
        public HttpStatusCode StatusCode { get; set; }
        private IDictionary<string, object> _values = new Dictionary<string, object>();
        private ResultBuilder(Exception exception)
        {
            if (exception is ValidationException)
            {
                if (exception.Message == Resources.Exception.Authentication || exception.Message == Resources.Exception.AccessDenied)
                {
                    StatusCode = HttpStatusCode.Unauthorized;
                }
                else
                {
                    StatusCode = HttpStatusCode.BadRequest;
                }
                var e = exception as ValidationException;
                e.List().ForEach(AddValue);
            }
            else
            {
                StatusCode = HttpStatusCode.InternalServerError;
                _values.Add("", exception.Message);
            }
        }

        private void AddValue(Warn warn)
        {
            var key = warn.Field;
            if (_values.ContainsKey(key))
            {
                ((List<string>)_values[key]).Add(warn.Message);
            }
            else
            {
                _values.Add(key, new List<string>() { warn.Message });
            }
        }

        public IActionResult Result()
        {
            var result = new JsonResult(_values);
            result.StatusCode = (int)StatusCode;
            return result;
        }

        public static ResultBuilder Of(Exception ex)
        {
            return new ResultBuilder(ex);
        }
    }

}