namespace LoadBalancer.LoadBalancer;

public class StaticStupidStrategy : ILoadBalancerStrategy
{
    public string? NextService(List<string?> services)
    {
        return services.First();
    }
}