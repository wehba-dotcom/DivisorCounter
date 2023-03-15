using System.Data;
using System.Net;
using CacheService.Filters;
using MySqlConnector;
using RestSharp;

bool LoadBalancerConnector()
{
    var restClient = new RestClient("http://load-balancer");
    var response = restClient.Post(new RestRequest("Configuration?url=http://" + Environment.MachineName, Method.Post));
    if (!response.IsSuccessful)
    {
        return false;
    }

    Console.WriteLine("Service registered successfully.");
    return true;
}

var retryCount = 0;

while(!LoadBalancerConnector() && retryCount < 3)
{
    Thread.Sleep(2000);
    retryCount++;
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(config =>
{
    config.Filters.Add(new LogFilter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IDbConnection>(c =>
{
    return new MySqlConnection("Server=cache-db;Database=cache-database;Uid=div-cache;Pwd=C@ch3d1v;");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();