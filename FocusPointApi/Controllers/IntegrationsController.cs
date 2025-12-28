using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options; 

[ApiController]
[Route("integrations/google")]
public class IntegrationsController : ControllerBase
{
    private readonly GoogleAuthorizationCodeFlow _flow;
    private readonly GoogleSettings _googleSettings;
    private readonly IUserService _userService;

    public IntegrationsController(GoogleAuthorizationCodeFlow flow, IOptions<GoogleSettings> googleSettings, IUserService userService)
    {
        _flow = flow;
        _googleSettings = googleSettings.Value;
        _userService = userService;
    }

    [HttpGet("connect")]
    public IActionResult Connect()
    {
        var userId = _userService.GetLocalUserId(HttpContext);

        var state = Guid.NewGuid().ToString("N");
        var req = _flow.CreateAuthorizationCodeRequest(_googleSettings.RedirectUri);
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

        TokenResponse token = await _flow.ExchangeCodeForTokenAsync(
            userId: userId,
            code: code,
            redirectUri: _googleSettings.RedirectUri,
            taskCancellationToken: HttpContext.RequestAborted
        );

        return Redirect("/connected");
    }
}