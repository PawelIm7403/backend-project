using CoreApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GatesController : ControllerBase
{
    private readonly IParkingGateService _service;

    public GatesController(IParkingGateService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllGates(int page, int size)
    {
        return Ok(await _service.GetAll(page, size));
    }
}