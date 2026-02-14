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
        => _service = service;

    // GET /api/notifications?take=50
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<NotificationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<NotificationDto>>> GetLatest(
        [FromQuery] int take = 50,
        CancellationToken ct = default)
    {
        // Guard rails (Performance + Schutz)
        if (take <= 0) take = 50;
        if (take > 200) take = 200;

        var items = await _service.GetLatestAsync(take, ct);
        return Ok(items);
    }

    // POST /api/notifications
    [HttpPost]
    [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<NotificationDto>> Create(
        [FromBody] CreateNotificationRequest request,
        CancellationToken ct = default)
    {
        // ModelState wird bei [ApiController] automatisch validiert (wenn DataAnnotations existieren)

        try
        {
            var created = await _service.CreateAsync(request, ct);

            // CreatedAtAction zu GetLatest ist ok, aber nicht super semantisch.
            // Wenn du später GET /api/notifications/{id} hast -> darauf verweisen.
            return CreatedAtAction(nameof(GetLatest), new { take = 50 }, created);
        }
        catch (ArgumentException ex)
        {
            // sauberer als anonymous {error=...}
            return ValidationProblem(new ValidationProblemDetails
            {
                Title = "Ungültige Benachrichtigung",
                Detail = ex.Message
            });
        }
    }
}