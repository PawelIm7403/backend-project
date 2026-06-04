using CoreApp.Dto;
using CoreApp.Mappers;
using CoreApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using CoreApp.Authorization;
using Microsoft.AspNetCore.Authorization;

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
    [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
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

    [HttpPost("{gateId:guid}/captures")]
    public async Task<IActionResult> AddCameraCapture(
        [FromRoute] Guid gateId,
        [FromBody] CreateCameraCaptureDto dto)
    {
        var capture = await _service.AddCapture(gateId, dto);

        return CreatedAtAction(
            nameof(GetCaptures),
            new { gateId },
            capture
        );
    }

    [HttpGet("{gateId:guid}/captures")]
    public async Task<IActionResult> GetCaptures(
        [FromRoute] Guid gateId,
        int page = 1,
        int size = 10)
    {
        var captures = await _service.GetCaptures(gateId, page, size);

        return Ok(captures);
    }
    
    [HttpDelete("{gateId:guid}/captures/{captureId:guid}")]
    public async Task<IActionResult> DeleteCameraCapture(
        [FromRoute] Guid gateId,
        [FromRoute] Guid captureId)
    {
        await _service.DeleteCapture(gateId, captureId);

        return NoContent();
    }
}