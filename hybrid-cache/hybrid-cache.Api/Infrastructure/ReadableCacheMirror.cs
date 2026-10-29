using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace hybrid_cache.Api.Infrastructure;

// Apenas para Development: além da entrada normal do HybridCache (que tem um cabeçalho binário),
// grava uma cópia "debug:<chave>" como string JSON pura, para leitura no Redis Insight.
internal sealed class ReadableCacheMirror(
    IDistributedCache inner,
    IConnectionMultiplexer redis,
    ILogger<ReadableCacheMirror> logger) : IDistributedCache
{
    private const string Prefix = "debug:";
    private const string HybridCacheInternalPrefix = "__MSFT_HCT__";
    private const int MaxTrailingBytes = 16;

    public byte[]? Get(string key) => inner.Get(key);

    public Task<byte[]?> GetAsync(string key, CancellationToken token = default) => inner.GetAsync(key, token);

    public void Refresh(string key) => inner.Refresh(key);

    public Task RefreshAsync(string key, CancellationToken token = default) => inner.RefreshAsync(key, token);

    public void Remove(string key)
    {
        inner.Remove(key);
        DeleteMirror(key);
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        await inner.RemoveAsync(key, token);
        DeleteMirror(key);
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        inner.Set(key, value, options);
        WriteMirror(key, value, options);
    }

    public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        await inner.SetAsync(key, value, options, token);
        WriteMirror(key, value, options);
    }

    private void WriteMirror(string key, byte[] payload, DistributedCacheEntryOptions options)
    {
        if (key.StartsWith(HybridCacheInternalPrefix, StringComparison.Ordinal))
            return;

        try
        {
            if (!TryExtractJson(payload, out var json))
            {
                logger.LogDebug("Não foi possível extrair o JSON da chave {CacheKey}", key);
                return;
            }

            redis.GetDatabase().StringSet(Prefix + key, json, expiry: GetExpiry(options), flags: CommandFlags.FireAndForget);
        }
        catch (Exception ex)
        {
            // A cópia é só para visualização: nunca deve derrubar a requisição
            logger.LogWarning(ex, "Falha ao gravar a cópia legível da chave {CacheKey}", key);
        }
    }

    private void DeleteMirror(string key)
    {
        try
        {
            redis.GetDatabase().KeyDelete(Prefix + key, CommandFlags.FireAndForget);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Falha ao remover a cópia legível da chave {CacheKey}", key);
        }
    }

    // O payload do HybridCache é: [cabeçalho binário][JSON][poucos bytes finais].
    // Procura o primeiro ponto em que começa um JSON válido que vai até (quase) o fim do payload.
    private static bool TryExtractJson(byte[] payload, out byte[] json)
    {
        for (var start = 0; start < payload.Length; start++)
        {
            var first = payload[start];
            if (first is not ((byte)'{' or (byte)'[' or (byte)'n'))
                continue;

            try
            {
                var reader = new Utf8JsonReader(payload.AsSpan(start));
                if (!reader.Read() || !reader.TrySkip())
                    continue;

                var length = (int)reader.BytesConsumed;
                if (payload.Length - (start + length) > MaxTrailingBytes)
                    continue;

                json = payload.AsSpan(start, length).ToArray();
                return true;
            }
            catch (JsonException)
            {
            }
        }

        json = [];
        return false;
    }

    private static TimeSpan? GetExpiry(DistributedCacheEntryOptions options)
    {
        if (options.AbsoluteExpirationRelativeToNow is { } relative)
            return relative;

        if (options.AbsoluteExpiration is { } absolute)
            return absolute - DateTimeOffset.UtcNow;

        return null;
    }
}
