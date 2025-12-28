using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/google")]
public class GoogleController : ControllerBase
{
    private readonly GoogleAuthorizationCodeFlow _flow;
    private readonly IUserService _userService;

    public GoogleController(GoogleAuthorizationCodeFlow flow, IUserService userService)
    {
        _flow = flow;
        _userService = userService;
    }

    [HttpGet("events")]
    public async Task<IActionResult> Events()
    {
        var userId = _userService.GetLocalUserId(HttpContext);

        var token = await _flow.LoadTokenAsync(userId, HttpContext.RequestAborted);
        if (token == null)
            return Unauthorized();

        var credential = new UserCredential(_flow, userId, token);
        var service = new CalendarService(
            new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "FocusPoint",
            }
        );

        var request = service.Events.List("primary");
        request.TimeMinDateTimeOffset = DateTime.UtcNow;
        request.ShowDeleted = false;
        request.SingleEvents = true;
        request.MaxResults = 20;
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

        var events = await request.ExecuteAsync(HttpContext.RequestAborted);
        var result =
            events.Items?.Select(e => new
            {
                id = e.Id,
                title = e.Summary,
                start = e.Start.DateTimeRaw ?? e.Start.Date,
                end = e.End.DateTimeRaw ?? e.End.Date,
                allDay = e.Start.Date != null,
            }) ?? Enumerable.Empty<object>();

        return Ok(result);
    }
}
