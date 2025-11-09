using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetItDoneBro.Api.Controllers;

[ApiController]
[Route("api/test")]
[Authorize]
public class WeatherForecastController() : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> Test()
    {
        return await Task.FromResult(Ok("elo"));
    }
}
