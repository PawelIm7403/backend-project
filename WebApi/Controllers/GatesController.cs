using CoreApp.Dto;
using CoreApp.Mappers;
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
    public async Task<IActionResult> GetAllGates(int page = 1, int size = 10)
    {
        return Ok(await _service.GetAll(page, size));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetGate(Guid id)
    {
        var dto = await _service.GetById(id);

        if (dto is null)
        {
            return NotFound();
        }

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGate([FromBody] CreateGateDto dto)
    {
        var gateDto = dto.ToEntity().ToDto();

        await _service.Add(gateDto);

        return CreatedAtAction(nameof(GetGate), new { id = gateDto.Id }, gateDto);
    }
}