using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class ConnectedController : ControllerBase
{
    [HttpGet("connected")]
    public IActionResult Connected()
    {
        return Ok("Google Calendar connected! You can close this window.");
    }
}