using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Caching.Hybrid;

namespace hybrid_cache.Api.Infrastructure;

// Serializador JSON do HybridCache que grava acentos legíveis no Redis ("ç" em vez de "ç")
internal sealed class ReadableJsonSerializerFactory : IHybridCacheSerializerFactory
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public bool TryCreateSerializer<T>([NotNullWhen(true)] out IHybridCacheSerializer<T>? serializer)
    {
        serializer = new Serializer<T>();
        return true;
    }

    private sealed class Serializer<T> : IHybridCacheSerializer<T>
    {
        public T Deserialize(ReadOnlySequence<byte> source)
        {
            var reader = new Utf8JsonReader(source);
            return JsonSerializer.Deserialize<T>(ref reader, Options)!;
        }

        public void Serialize(T value, IBufferWriter<byte> target)
        {
            using var writer = new Utf8JsonWriter(target, new JsonWriterOptions { Encoder = Options.Encoder });
            JsonSerializer.Serialize(writer, value, Options);
        }
    }
}
