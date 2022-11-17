using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace outputcache_middleware.Controllers;

[ApiController]
[Route("[controller]")]
//[OutputCache]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly Dictionary<string, List<string>> _countries;
    private readonly IOutputCacheStore _cache;


    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IOutputCacheStore cache)
    {
        _logger = logger;
        this._countries = new Dictionary<string, List<string>>();
        _countries.Add("Germany", new List<string>() { "Stuttgart", "Munich" });
        _countries.Add("France", new List<string>() { "Paris", "Lyon" });
        _cache = cache;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [OutputCache]
    public IActionResult Get()
    {
        return Ok(Summaries);
    }

    [HttpGet]
    [Route("City")]
    //[OutputCache(Duration = 1000, VaryByQueryKeys = new string[] { "country" })]
    [OutputCache(PolicyName = "Query1")]
    public IActionResult GetCityByCountry([FromQuery] string country)
    {
        var result = _countries[country];
        return Ok(result);
    }

    [HttpPost]
    [Route("CityPurge")]
    public async Task<IActionResult> CitcCachePurge()
    {
        await this._cache.EvictByTagAsync("tag-country", default);
        return Ok();
    }
}

