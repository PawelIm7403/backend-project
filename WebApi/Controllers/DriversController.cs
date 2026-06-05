using System.Security.Claims;
using CoreApp.Dto;
using CoreApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/drivers")]
[Authorize]
public class DriversController : ControllerBase
{
    private readonly IDriverService _driverService;

    public DriversController(IDriverService driverService)
    {
        _driverService = driverService;
    }

    [HttpPost("vehicles")]
    public async Task<IActionResult> RegisterVehicle(
        [FromBody] CreateDriverVehicleDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _driverService.RegisterVehicle(dto, userId);

        return CreatedAtAction(
            nameof(GetMyVehicles),
            new { },
            result
        );
    }

    [HttpGet("vehicles")]
    public async Task<IActionResult> GetMyVehicles()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _driverService.GetMyVehicles(userId);

        return Ok(result);
    }
    
    [HttpGet("vehicles/{licensePlate}/sessions")]
    public async Task<IActionResult> GetVehicleSessions(
        [FromRoute] string licensePlate)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _driverService.GetVehicleSessions(licensePlate, userId);

        return Ok(result);
    }
    
    [HttpGet("vehicles/{licensePlate}/current-session")]
    public async Task<IActionResult> GetCurrentVehicleSession(
        [FromRoute] string licensePlate)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _driverService.GetCurrentVehicleSession(licensePlate, userId);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }
    
    [HttpPost("vehicles/{licensePlate}/pay-current-session")]
    public async Task<IActionResult> PayMyCurrentSession(
        [FromRoute] string licensePlate)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _driverService.PayMyCurrentSession(licensePlate, userId);

        return Ok(result);
    }
    
    [HttpGet("account")]
    public async Task<IActionResult> GetAccount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _driverService.GetAccount(userId);

        return Ok(result);
    }

    [HttpPost("account/topup")]
    public async Task<IActionResult> TopUpAccount([FromBody] TopUpAccountDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _driverService.TopUpAccount(userId, dto);

        return Ok(result);
    }
    
    [HttpPost("vehicles/{licensePlate}/pay-from-account")]
    public async Task<IActionResult> PayMyCurrentSessionFromAccount(
        [FromRoute] string licensePlate)
    
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _driverService.PayMyCurrentSessionFromAccount(
            licensePlate,
            userId);

        
        
        return Ok(result);
    }
    
    [HttpGet("discounts")]
    public async Task<IActionResult> GetMyDiscounts()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _driverService.GetMyDiscounts(userId);

        return Ok(result);
    }

    [HttpPost("discounts/activate-registration")]
    public async Task<IActionResult> ActivateRegistrationBonus()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _driverService.ActivateRegistrationBonus(userId);

        return Ok(result);
    }

    [HttpPost("discounts/activate-loyalty")]
    public async Task<IActionResult> ActivateLoyaltyDiscount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Unauthorized();
        }

        var result = await _driverService.ActivateLoyaltyDiscount(userId);

        return Ok(result);
    }
}