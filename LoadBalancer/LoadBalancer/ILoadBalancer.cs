namespace LoadBalancer.LoadBalancer;

public interface ILoadBalancer
{
    public List<string?> GetAllServices();
    public Guid AddService(string? url);
    public Guid RemoveService(Guid id);
    public ILoadBalancerStrategy? GetActiveStrategy();
    public void SetActiveStrategy(ILoadBalancerStrategy? strategy);
    public string? NextService();
}