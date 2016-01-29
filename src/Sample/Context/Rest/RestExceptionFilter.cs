using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
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
            context.Result = ResultBuilder.Of(Localizer(context), context.Exception).Result();
        }

        private IStringLocalizer<Startup> Localizer(ExceptionContext context)
        {
            return context.HttpContext.ApplicationServices.GetService(typeof(IStringLocalizer<Startup>)) as IStringLocalizer<Startup>;
        }
    }

    class ResultBuilder
    {
        private IStringLocalizer<Startup> _localizer;
        private IDictionary<string, object> _values = new Dictionary<string, object>();
        private HttpStatusCode _statusCode = HttpStatusCode.OK;
        private ResultBuilder(IStringLocalizer<Startup> localizer, Exception exception)
        {
            this._localizer = localizer;
            if (exception is ValidationException)
            {
                switch (exception.Message)
                {
                    case ErrorKeys.Authentication:
                    case ErrorKeys.AccessDenied:
                        _statusCode = HttpStatusCode.Unauthorized;
                        break;
                    default:
                        _statusCode = HttpStatusCode.BadRequest;
                        break;
                }
                var e = exception as ValidationException;
                e.List().ForEach(warn => _values.Add(warn.Field, new string[]{ _localizer.GetString(warn.Message) }));
            } else {
                _statusCode = HttpStatusCode.InternalServerError;
                _values.Add("", exception.Message);
            }
        }

        public IActionResult Result()
        {
            var result = new JsonResult(_values);
            result.StatusCode = (int)_statusCode;
            return result;
        }

        public static ResultBuilder Of(IStringLocalizer<Startup> localizer, Exception ex)
        {
            return new ResultBuilder(localizer, ex);
        }
    }

}