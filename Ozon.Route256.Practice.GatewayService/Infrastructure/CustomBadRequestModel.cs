using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ozon.Route256.Practice.GatewayService.Infrastructure
{
    public class CustomBadRequestModel
    {
        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public Dictionary<string, string[]> Errors { get; private set; } = new Dictionary<string, string[]>();
        public CustomBadRequestModel(ActionContext context)
        {
            foreach (var keyModelStatePair in context.ModelState)
            {
                var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    var errorMessages = new string[errors.Count];
                    for (var i = 0; i < errors.Count; i++)
                    {
                        errorMessages[i] = errors[i].ErrorMessage;
                    }
                    Errors.Add(key, errorMessages);
                }
            }
        }
    }
}
