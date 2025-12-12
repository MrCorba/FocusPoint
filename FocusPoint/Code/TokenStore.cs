using System.Collections.Concurrent;
using Google.Apis.Util.Store;

public class TokenStore : IDataStore
{
    private readonly ConcurrentDictionary<string, object> _store = new();

    public Task StoreAsync<T>(string key, T value)
    {
        _store[key] = value!;
        return Task.CompletedTask;
    }

    public Task DeleteAsync<T>(string key)
    {
        _store.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    public Task<T> GetAsync<T>(string key)
    {
        if (_store.TryGetValue(key, out var value) && value is T typed)
            return Task.FromResult(typed);

        return Task.FromResult(default(T)!);
    }

    public Task ClearAsync()
    {
        _store.Clear();
        return Task.CompletedTask;
    }
}
