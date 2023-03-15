// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using RestSharp;

public class Program
{
    private static readonly RestClient RestClient = new RestClient("http://load-balancer/");
    //private static IDbConnection divisorCache = new MySqlConnection("Server=cache-db;Database=cache-database;Uid=div-cache;Pwd=C@ch3d1v;");
    
    public static void Main()
    {
        Thread.Sleep(5000); // Let's wait for things to start up
        
        long first = 1_000_000_100;
        long last = 1_000_000_200;

        var numberWithMostDivisors = first;
        var result = 0;

        var watch = Stopwatch.StartNew();
        for (var i = first; i <= last; i++)
        {
            var innerWatch = Stopwatch.StartNew();
            var divisorCounter = CountDivisors(i);
            
            // divisorCache.Execute("INSERT INTO counters (number, divisors) VALUES (@number, @divisors)", new { number = i, divisors = divisorCounter });
            RestClient.PostAsync(new RestRequest($"/cache?number={i}&divisorCounter={divisorCounter}"));

            innerWatch.Stop();
            Console.WriteLine("Counted " + divisorCounter + " divisors for " + i + " in " + innerWatch.ElapsedMilliseconds + "ms");

            if (divisorCounter > result)
            {
                numberWithMostDivisors = i;
                result = divisorCounter;
            }
        }
        watch.Stop();
        
        Console.WriteLine("The number with most divisors inside range is: " + numberWithMostDivisors + " with " + result + " divisors.");
        Console.WriteLine("Elapsed time: " + watch.ElapsedMilliseconds + "ms");
        Console.ReadLine();
    }

    private static int CountDivisors(long number)
    {
        // var divisorCounter = divisorCache.QueryFirstOrDefault<int>("SELECT divisors FROM counters WHERE number = @number", new { number = i });
        var task = RestClient.GetAsync<int>(new RestRequest("/cache?number=" + number));

        //var divisorResult = task.Content.ReadAsStringAsync().Result;
        var divisorCounter = 0; //int.Parse(divisorResult);

        //if (divisorCounter == 0)
        {
            for (var divisor = 1; divisor <= number; divisor++)
            {
                if (task?.Status == TaskStatus.RanToCompletion)
                {
                    var cachedResult = task.Result;
                    if (cachedResult != 0)
                    {
                        return cachedResult;
                    }

                    task = null;
                }
                    
                if (number % divisor == 0)
                {
                    divisorCounter++;
                }
            }

            return divisorCounter;
        }
    }
}