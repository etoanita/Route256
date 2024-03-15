using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using Grpc.Core;
using System.Net;

namespace Ozon.Route256.Practice.GatewayService.Infrastructure
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is RpcException rpcException)
            {
                context.Result = new ObjectResult(new
                {
                    context.Exception.Message,
                    context.Exception.Source,
                    ExceptionType = context.Exception.GetType().FullName,
                });
                switch (rpcException.StatusCode)
                {
                    case StatusCode.NotFound:
                        {
                            ((ObjectResult)context.Result).StatusCode = (int)HttpStatusCode.NotFound;
                            break;
                        }
                    default:
                        {
                            ((ObjectResult)context.Result).StatusCode = (int)HttpStatusCode.InternalServerError;
                            break;
                        }
                }

                context.ExceptionHandled = true;
            }
            else if (context.Exception != null)
            {
                context.Result = new ObjectResult(new
                {
                    context.Exception.Message,
                    context.Exception.Source,
                    ExceptionType = context.Exception.GetType().FullName,
                    StatusCode = (int)HttpStatusCode.InternalServerError
                });

                context.ExceptionHandled = true;
            }
        }
    }
}
