using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

namespace Sample.Context.Rest
{
    //<summary>
    // REST用の例外Map変換サポート。
    //</summary>
    class RestExceptionFilter : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            //TODO: bind
            context.Result = new JsonResult(ex.Message);
        }
    }

}