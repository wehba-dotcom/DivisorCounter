using Microsoft.AspNetCore.Mvc.Filters;

namespace CacheService.Filters;

public class LogFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Console.WriteLine(Environment.MachineName + ": " + nameof(context.Controller)  + "." + context.ActionDescriptor.DisplayName);
        var result = await next();
    }
}