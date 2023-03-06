using LoadBalancer.LoadBalancer;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Controllers;

public class ConfigurationController : ControllerBase
{
    public ConfigurationController()
    {
        LoadBalancer.LoadBalancer.GetInstance().SetActiveStrategy(new StaticStupidStrategy());
    }
    
    public Guid Post([FromBody] string? url)
    {
        Console.WriteLine("Adding service at URL " + url);
        return LoadBalancer.LoadBalancer.GetInstance().AddService(url);
    }
}