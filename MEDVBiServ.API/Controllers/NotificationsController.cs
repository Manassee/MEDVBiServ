using MEDVBiServ.Application.Dtos;
using MEDVBiServ.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MEDVBiServ.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class NotificationsController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationsController(INotificationService service)
    {
        _service = service;
    }

    // GET /api/notifications?take=50
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<NotificationDto>>> GetLatest([FromQuery] int take = 50, CancellationToken ct = default)
    {
        var items = await _service.GetLatestAsync(take, ct);
        return Ok(items);
    }

    // POST /api/notifications
    [HttpPost]
    public async Task<ActionResult<NotificationDto>> Create([FromBody] CreateNotificationRequest request, CancellationToken ct = default)
    {
        try
        {
            var created = await _service.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetLatest), new { take = 50 }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}