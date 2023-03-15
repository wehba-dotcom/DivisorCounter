using Microsoft.AspNetCore.Mvc.Filters;

namespace LoadBalancer.Filters;

public class LogFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Console.WriteLine("LB: " + nameof(context.Controller)  + "." + context.ActionDescriptor.DisplayName);
        var result = await next();
        // Console.WriteLine("------------------------------------------");
    }
}