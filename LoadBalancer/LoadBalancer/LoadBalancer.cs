namespace LoadBalancer.LoadBalancer;

public class LoadBalancer : ILoadBalancer
{
    private ILoadBalancerStrategy? _strategy;
    private readonly Dictionary<Guid, string?> _urls = new();
    private static LoadBalancer? _instance;

    private LoadBalancer()
    { }

    public static LoadBalancer GetInstance()
    {
        return _instance ??= new LoadBalancer();
    }
    
    public List<string?> GetAllServices()
    {
        return _urls.Values.ToList();
    }

    public Guid AddService(string? url)
    {
        var id = Guid.NewGuid();
        _urls.Add(id, url);
        return id;
    }

    public Guid RemoveService(Guid id)
    {
        _urls.Remove(id);
        return id;
    }

    public ILoadBalancerStrategy? GetActiveStrategy()
    {
        return _strategy;
    }

    public void SetActiveStrategy(ILoadBalancerStrategy? strategy)
    {
        this._strategy = strategy;
    }

    public string? NextService()
    {
        return _strategy?.NextService(GetAllServices());
    }
}