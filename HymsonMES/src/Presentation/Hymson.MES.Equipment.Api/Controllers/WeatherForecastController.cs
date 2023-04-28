using Hymson.Web.Framework.WorkContext;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Equipment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ICurrentEquipment _currentEquipment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            ICurrentEquipment  currentEquipment,
             IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _currentEquipment = currentEquipment;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
           var claimsPrincipal= _httpContextAccessor?.HttpContext?.User;
            _logger.LogInformation($"{_currentEquipment.Name} {_currentEquipment.Id} {_currentEquipment.SiteId} {_currentEquipment.FactoryId}");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}