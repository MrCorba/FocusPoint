using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("integrations/google")]
public class IntegrationsController : ControllerBase
{
    private readonly GoogleAuthorizationCodeFlow _flow;
    private readonly IConfiguration _config;
    private readonly IUserService _userService;

    public IntegrationsController(GoogleAuthorizationCodeFlow flow, IConfiguration config, IUserService userService)
    {
        _flow = flow;
        _config = config;
        _userService = userService;
    }

    [HttpGet("connect")]
    public IActionResult Connect()
    {
        var redirectUri = _config["Google:RedirectUri"]!;
        var userId = _userService.GetLocalUserId(HttpContext);

        var state = Guid.NewGuid().ToString("N");
        var req = _flow.CreateAuthorizationCodeRequest(redirectUri);
        req.State = state;
        // Uncomment and adjust as needed:
        // req.AccessType = "offline";
        // req.Prompt = "consent";

        var authUrl = req.Build().ToString();
        return Redirect(authUrl);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        if (string.IsNullOrEmpty(code))
            return BadRequest("Missing 'code'.");

        var userId = _userService.GetLocalUserId(HttpContext);
        var redirectUri = _config["Google:RedirectUri"]!;

        TokenResponse token = await _flow.ExchangeCodeForTokenAsync(
            userId: userId,
            code: code,
            redirectUri: redirectUri,
            taskCancellationToken: HttpContext.RequestAborted
        );

        return Redirect("/connected");
    }
}