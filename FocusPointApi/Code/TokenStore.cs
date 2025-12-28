using Google.Apis.Util.Store;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class TokenStore : IDataStore
{
    private readonly IServiceProvider _serviceProvider;

    public TokenStore(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        // Create a scope to ensure the database is created
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureCreated();
    }

    private AppDbContext GetDbContext()
    {
        var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    public async Task StoreAsync<T>(string key, T value)
    {
        using var dbContext = GetDbContext();
        var userId = GetCurrentUserId();
        var serializedValue = JsonSerializer.Serialize(value);

        var existingToken = await dbContext.Tokens.FirstOrDefaultAsync(t =>
            t.UserId == userId && t.Key == key
        );

        if (existingToken != null)
        {
            existingToken.Value = serializedValue;
        }
        else
        {
            dbContext.Tokens.Add(new TokenEntity
                {
                    UserId = userId,
                    Key = key,
                    Value = serializedValue,
                }
            );
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync<T>(string key)
    {
        using var dbContext = GetDbContext();
        var userId = GetCurrentUserId();
        var token = await dbContext.Tokens.FirstOrDefaultAsync(t => 
            t.UserId == userId && t.Key == key
        );

        if (token != null)
        {
            dbContext.Tokens.Remove(token);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<T> GetAsync<T>(string key)
    {
        using var dbContext = GetDbContext();
        var userId = GetCurrentUserId();
        var token = await dbContext.Tokens
            .FirstOrDefaultAsync(t => t.UserId == userId && t.Key == key);

        if (token != null)
        {
            return JsonSerializer.Deserialize<T>(token.Value)!;
        }

        return default(T)!;
    }

    public async Task ClearAsync()
    {
        using var dbContext = GetDbContext();
        var userId = GetCurrentUserId();
        var tokens = dbContext.Tokens.Where(t => t.UserId == userId);
        dbContext.Tokens.RemoveRange(tokens);
        await dbContext.SaveChangesAsync();
    }

    private string GetCurrentUserId()
    {
        // For now, return a fixed user ID. In a real app, you'd get this from the current user context
        // This should match what you use in your controllers
        return "demo-user-1";
    }
}
