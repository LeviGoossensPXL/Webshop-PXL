using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string APIKEYNAME = "X-Api-Key";

        private ContentResult GetContentResult(int statusCode, string content)
        {
            var result = new ContentResult();
            result.StatusCode = statusCode;
            result.Content = content;
            return result;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(
                    APIKEYNAME, out var extractedApiKey))
            {
                context.Result = GetContentResult(
                    401, "Api Key was not provided");
                return;
            }
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var keyValue = extractedApiKey.ToString();
            if(keyValue != config.GetValue<string>("WebApi:ApiKey"))
            {
                context.Result = GetContentResult(
                    401, "Api Key value is not valid!");
                return;
            }
            await next();
        }
    }
}