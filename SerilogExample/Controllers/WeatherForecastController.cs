using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace SerilogExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger _performanceMetricsLogger;

        public WeatherForecastController(ILoggerFactory loggerFactory)
        {
            _performanceMetricsLogger = loggerFactory.CreateLogger("PerformanceMetrics");
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var activity = "EnquiryVerification";
            var activityIdentifier = Guid.NewGuid().ToString();
            var startTime = DateTime.Now;       

            using (_performanceMetricsLogger.BeginScope(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("activity", activity),
                new KeyValuePair<string, object>("activityIdentifier", activityIdentifier),
                new KeyValuePair<string, object>("event", "Request")
            }))
            {
                this._performanceMetricsLogger.Log(LogLevel.Information, $"EnquiryVerification started for {activityIdentifier}");
            }

            var random = new Random(5);
            random.Next(30);
            Thread.Sleep(random.Next(10) * 1000);
            var endTime = DateTime.Now;

            using (_performanceMetricsLogger.BeginScope(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("activity", activity),
                new KeyValuePair<string, object>("activityIdentifier", activityIdentifier),                
                new KeyValuePair<string, object>("event", "Response"),
                new KeyValuePair<string, object>("duration", endTime.Subtract(startTime).TotalMilliseconds)
            }))
            {
                this._performanceMetricsLogger.Log(LogLevel.Information, $"EnquiryVerification completed for {activityIdentifier}");
            }
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("test")]
        public IEnumerable<WeatherForecast> Get(int id)
        {
            var activity = "CreateOrder";
            var activityIdentifier = Guid.NewGuid().ToString();
            var startTime = DateTime.Now;

            using (_performanceMetricsLogger.BeginScope(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("activity", activity),
                new KeyValuePair<string, object>("activityIdentifier", activityIdentifier),
                new KeyValuePair<string, object>("event", "Request")
            }))
            {
                this._performanceMetricsLogger.Log(LogLevel.Information, $"CreateOrder started for {activityIdentifier}");
            }

            var random = new Random(5);
            random.Next(30);
            Thread.Sleep(random.Next(10) * 1000);
            var endTime = DateTime.Now;

            using (_performanceMetricsLogger.BeginScope(new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>("activity", activity),
                new KeyValuePair<string, object>("activityIdentifier", activityIdentifier),
                new KeyValuePair<string, object>("event", "Response"),
                new KeyValuePair<string, object>("duration", endTime.Subtract(startTime).TotalMilliseconds)
            }))
            {
                this._performanceMetricsLogger.Log(LogLevel.Information, $"CreateOrder completed for {activityIdentifier}");
            }
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
