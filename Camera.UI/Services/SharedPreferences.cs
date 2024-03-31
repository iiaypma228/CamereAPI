using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Camera.UI.Services;

public interface ISharedPreferences
{
    Task<string> GetAsync(string key);
    Task<T?> GetAsync<T>(string key);
    Task SaveAsync<T>(string key, T value);
    Task SaveAsync(string key, string value);
}

public class SharedPreferences : ISharedPreferences
{
    private static IsolatedStorageFile Store => IsolatedStorageFile.GetUserStoreForDomain();
    private static readonly SemaphoreSlim Sema = new(1, 1);
    
    public async Task<string> GetAsync(string key)
    {
        await Sema.WaitAsync(new CancellationToken());

        // it may happen, that a value type changes and can't be deserialized
        // so prevent exceptions in this case
        try
        {
            await using var stream = Store.OpenFile(key, FileMode.Open);
            using var s = new StreamReader(stream);
            return await s.ReadToEndAsync();
        }
        finally
        {
            Sema.Release();
        }
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        await Sema.WaitAsync(new CancellationToken());

        // it may happen, that a value type changes and can't be deserialized
        // so prevent exceptions in this case
        try
        {
            await using var stream = Store.OpenFile(key, FileMode.Open);
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
        finally
        {
            Sema.Release();
        }
    }

    public async Task SaveAsync<T>(string key, T value)
    {
        await Sema.WaitAsync(new CancellationToken());
        try
        {
            await using var stream = Store.OpenFile(key, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(stream, value);
        }
        finally
        {
            Sema.Release();
        }
    }

    public async Task SaveAsync(string key, string value)
    {
        await Sema.WaitAsync(new CancellationToken());
        try
        {
            await using var stream = Store.OpenFile(key, FileMode.Create, FileAccess.Write);
            var bytes = Encoding.UTF8.GetBytes(value);
            await stream.WriteAsync(bytes, 0,  bytes.Length);
        }
        finally
        {
            Sema.Release();
        }
    }
}