using Dqq.MatchServer;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls(builder.Configuration["urls"] ?? "http://127.0.0.1:5077");
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddSingleton<MatchService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<MatchService>());

WebApplication app = builder.Build();
app.UseCors();

app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    service = "DQQ Match Server",
    utc = DateTimeOffset.UtcNow
}));

app.MapPost("/api/matchmaking/join", (JoinRequest request, MatchService service) =>
{
    if (request.HeroId is < 1 or > 6)
        return Results.BadRequest(new { error = "heroId must be between 1 and 6" });
    return Results.Ok(service.Join(request));
});

app.MapGet("/api/matchmaking/tickets/{ticketId}", (string ticketId, MatchService service) =>
    service.GetTicket(ticketId) is { } ticket ? Results.Ok(ticket) : Results.NotFound());

app.MapGet("/api/matches/{matchId}", (string matchId, string playerId, string token, MatchService service) =>
{
    MatchSnapshot? snapshot = service.GetMatch(matchId, playerId, token);
    return snapshot == null ? Results.Unauthorized() : Results.Ok(snapshot);
});

app.MapPost("/api/matches/{matchId}/upgrade", (string matchId, UpgradeRequest request, MatchService service) =>
{
    MatchSnapshot? snapshot = service.SubmitUpgrade(matchId, request);
    return snapshot == null ? Results.BadRequest(new { error = "invalid match, token, or upgrade" }) : Results.Ok(snapshot);
});

app.MapPost("/api/matches/{matchId}/result", (string matchId, RoundResultRequest request, MatchService service) =>
{
    MatchSnapshot? snapshot = service.SubmitRoundResult(matchId, request);
    return snapshot == null ? Results.BadRequest(new { error = "invalid or duplicate round result" }) : Results.Ok(snapshot);
});

app.MapGet("/api/server/stats", (MatchService service) => Results.Ok(service.Stats()));

app.Run();
