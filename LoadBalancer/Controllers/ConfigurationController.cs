using LoadBalancer.LoadBalancer.Strategies;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigurationController : ControllerBase
{
    public ConfigurationController()
    {
        LoadBalancer.LoadBalancer.GetInstance().SetActiveStrategy(new RoundRobinStrategy());
    }
    
    [HttpPost]
    public Guid Post([FromQuery] string? url)
    {
        Console.WriteLine("LB: Adding service at URL " + url);
        return LoadBalancer.LoadBalancer.GetInstance().AddService(url);
    }
}