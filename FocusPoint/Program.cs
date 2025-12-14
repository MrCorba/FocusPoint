using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<IDataStore, TokenStore>();

builder.Services.AddSingleton<GoogleAuthorizationCodeFlow>(sp =>
{
    var cfg = builder.Configuration.GetSection("Google");
    var clientSecrets = new ClientSecrets
    {
        ClientId = cfg["ClientId"]!,
        ClientSecret = cfg["ClientSecret"]!,
    };

    return new GoogleAuthorizationCodeFlow(
        new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = clientSecrets,
            Scopes = new[] { CalendarService.Scope.CalendarReadonly },
            DataStore = sp.GetRequiredService<IDataStore>(),
        }
    );
});

var app = builder.Build();

// ---- Helpers ----
// In your app, you already have your own auth.
// Replace this with your real logged-in user id (from cookie/session/claims).
string GetLocalUserId(HttpContext ctx) => "demo-user-1";

// ---- 1) Start OAuth (Connect Google) ----
app.MapGet(
    "/integrations/google/connect",
    (HttpContext ctx, GoogleAuthorizationCodeFlow flow, IConfiguration config) =>
    {
        var redirectUri = config["Google:RedirectUri"]!;
        var userId = GetLocalUserId(ctx);

        // Optional: CSRF protection - store state in session/db; simplified here
        var state = Guid.NewGuid().ToString("N");

        var req = flow.CreateAuthorizationCodeRequest(redirectUri);
        req.State = state;

// re
//         // IMPORTANT for refresh token: request offline access
//         req.AccessType = "offline"; // ensures refresh token is issued on first consent [1](https://developers.google.com/api-client-library/dotnet/guide/aaa_oauth)

//         // If you’re not receiving a refresh token (because user already consented),
//         // forcing consent can help:
//         req.Prompt = "consent"; // supported values include "consent" [5](https://googleapis.dev/dotnet/Google.Apis.Auth/latest/api/Google.Apis.Auth.OAuth2.Flows.GoogleAuthorizationCodeFlow.html)

        // Optional: if you want to suggest a Google account:
        // req.LoginHint = "user@domain.com";

        var authUrl = req.Build().ToString();
        return Results.Redirect(authUrl);
    }
);

// ---- 2) OAuth callback ----
app.MapGet(
    "/integrations/google/callback",
    async (HttpContext ctx, GoogleAuthorizationCodeFlow flow, IConfiguration config) =>
    {
        var code = ctx.Request.Query["code"].ToString();
        if (string.IsNullOrEmpty(code))
            return Results.BadRequest("Missing 'code'.");

        var userId = GetLocalUserId(ctx);
        var redirectUri = config["Google:RedirectUri"]!;

        // Exchange code for tokens (stored in the flow's DataStore)
        TokenResponse token = await flow.ExchangeCodeForTokenAsync(
            userId: userId,
            code: code,
            redirectUri: redirectUri,
            taskCancellationToken: ctx.RequestAborted
        );

        // token contains access_token, expires_in, and (often) refresh_token on first consent
        // A refresh token is typically returned when using access_type=offline. [1](https://developers.google.com/api-client-library/dotnet/guide/aaa_oauth)
        return Results.Redirect("/connected"); // or wherever your UI lives
    }
);

// ---- 3) Example API endpoint: list events ----
app.MapGet(
    "/api/google/events",
    async (HttpContext ctx, GoogleAuthorizationCodeFlow flow) =>
    {
        var userId = GetLocalUserId(ctx);

        // Load token from store and create credential
        var token = await flow.LoadTokenAsync(userId, ctx.RequestAborted);
        if (token == null)
            return Results.Unauthorized(); // not connected yet

        var credential = new Google.Apis.Auth.OAuth2.UserCredential(flow, userId, token);

        var service = new CalendarService(
            new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "My .NET 8 + Vue Calendar App",
            }
        );

        // List next 20 events from primary calendar
        var request = service.Events.List("primary");
        request.TimeMinDateTimeOffset = DateTime.UtcNow;
        request.ShowDeleted = false;
        request.SingleEvents = true;
        request.MaxResults = 20;
        request.OrderBy = Google.Apis.Calendar.v3.EventsResource.ListRequest.OrderByEnum.StartTime;

        var events = await request.ExecuteAsync(ctx.RequestAborted);

        // Return a simplified format suitable for FullCalendar
        var result =
            events.Items?.Select(e => new
            {
                id = e.Id,
                title = e.Summary,
                start = e.Start.DateTimeRaw ?? e.Start.Date,
                end = e.End.DateTimeRaw ?? e.End.Date,
                allDay = e.Start.Date != null,
            }) ?? Enumerable.Empty<object>();

        return Results.Ok(result);
    }
);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
