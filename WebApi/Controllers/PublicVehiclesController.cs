using CoreApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/public/vehicles")]
[AllowAnonymous]
public class PublicVehiclesController : ControllerBase
{
    private readonly IDriverService _driverService;

    public PublicVehiclesController(IDriverService driverService)
    {
        _driverService = driverService;
    }

    [HttpGet("{licensePlate}/sessions")]
    public async Task<IActionResult> GetVehicleSessions(
        [FromRoute] string licensePlate)
    {
        var result = await _driverService.GetPublicVehicleSessions(licensePlate);

        return Ok(result);
    }

    [HttpGet("{licensePlate}/current-session")]
    public async Task<IActionResult> GetCurrentVehicleSession(
        [FromRoute] string licensePlate)
    {
        var result = await _driverService.GetPublicCurrentVehicleSession(licensePlate);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
    
    [HttpPost("{licensePlate}/pay-current-session")]
    public async Task<IActionResult> PayCurrentSession(
        [FromRoute] string licensePlate)
    {
        var result = await _driverService.PayCurrentSession(licensePlate);

        return Ok(result);
    }
}