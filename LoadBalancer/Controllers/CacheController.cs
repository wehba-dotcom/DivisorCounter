using System.Net;
using System.Text.Json.Serialization;
using LoadBalancer.LoadBalancer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace LoadBalancer.Controllers;

[ApiController]
[Route("[controller]")]
public class CacheController : ControllerBase
{
    private readonly RestClient _restClient = new();
    private readonly ILoadBalancer _loadBalancer = LoadBalancer.LoadBalancer.GetInstance();
    
    [HttpGet]
    public async Task<ActionResult<int>> Get(long number)
    {
        var result = await CallService<int>("/Cache?number=" + number, Method.Get);
        return result;
    }

    [HttpPost]
    public async Task<ActionResult<object>> Post([FromQuery] long number, [FromQuery] int divisorCounter)
    {
        return await CallService<object>("/Cache?number=" + number + "&divisorCounter=" + divisorCounter, Method.Post);
    }

    private async Task<ActionResult<TResult>> CallService<TResult>(string url, Method method)
    {
        var service = _loadBalancer.NextService();
        if (service == null)
        {
            return StatusCode(503); // Service Unavailable
        }
        
        var result = await _restClient.ExecuteAsync<TResult>(new RestRequest(service.Url + url), method);
        int statusCode = (int)result.StatusCode;
        if (statusCode is 0 or >= 500)
        {
            Console.WriteLine("Service at URL " + service.Url + " returned status code " + (int)result.StatusCode + " and will be removed.");
            _loadBalancer.RemoveService(service.Id);
            return await CallService<TResult>(url, method);
        }
        
        return Ok(result.Data);
    }
}