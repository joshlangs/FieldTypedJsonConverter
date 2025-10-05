using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Epoche;

public sealed class FieldTypedJsonConverter<TBase> : JsonConverter<TBase> where TBase : class
{
    readonly string FieldName;
    readonly byte[] FieldBytes;
    readonly Lock LockObject = new();
    bool Locked;
    readonly Dictionary<string, Type> ValueToType = [];
    public Type? FallbackType { get; private set; }
    Func<TBase, string?>? GetFieldValue;
    public bool IsValidatingFieldValue => GetFieldValue is not null;

    public FieldTypedJsonConverter(string fieldName) : this(fieldName, null) { }
    public FieldTypedJsonConverter(string fieldName, Func<TBase, string?>? getFieldValue)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fieldName);
        if (!typeof(TBase).IsAbstract)
        {
            throw new InvalidOperationException($"{typeof(TBase).FullName} must be abstract");
        }
        FieldName = fieldName;
        FieldBytes = Encoding.UTF8.GetBytes(fieldName);
        GetFieldValue = getFieldValue;
    }

    public string[] GetMappedFieldValues()
    {
        lock (LockObject)
        {
            return [.. ValueToType.Keys];
        }
    }
    public Type? GetMappedType(string fieldValue)
    {
        ArgumentNullException.ThrowIfNull(fieldValue);
        lock (LockObject)
        {
            ValueToType.TryGetValue(fieldValue, out var type);
            return type;
        }
    }

    public override bool CanConvert(Type typeToConvert)
    {
        if (!Locked)
        {
            lock (LockObject)
            {
                Locked = true;
            }
        }
        return base.CanConvert(typeToConvert);
    }

    static void CheckDeserializable(Type type)
    {
        if (type.IsAbstract)
        {
            throw new InvalidOperationException($"{type.FullName} is abstract");
        }
        if (!type.IsAssignableTo(typeof(TBase)))
        {
            throw new InvalidOperationException($"{type.FullName} is not assignable to {typeof(TBase).FullName}");
        }
    }

    void CheckNotLocked()
    {
        if (Locked)
        {
            throw new InvalidOperationException("Types cannot change after the converter has been used");
        }
    }

    public void DoFieldValueValidation(Func<TBase, string?>? getFieldValue)
    {
        lock (LockObject)
        {
            if (GetFieldValue == getFieldValue)
            {
                return;
            }
            CheckNotLocked();
            GetFieldValue = getFieldValue;
        }
    }

    public void SetFallbackType(Type? fallbackType)
    {
        if (fallbackType is not null)
        {
            CheckDeserializable(fallbackType);
        }
        lock (LockObject)
        {
            if (FallbackType == fallbackType)
            {
                return;
            }
            CheckNotLocked();
            FallbackType = fallbackType;
        }
    }

    public void Map(string fieldValue, Type type)
    {
        ArgumentNullException.ThrowIfNull(fieldValue);
        ArgumentNullException.ThrowIfNull(type);
        CheckDeserializable(type);
        lock (LockObject)
        {
            if (ValueToType.TryGetValue(fieldValue, out var existing) && existing == type)
            {
                return;
            }
            CheckNotLocked();
            ValueToType[fieldValue] = type;
        }
    }

    public override TBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var clone = reader;
        while (clone.Read())
        {
            if (clone.TokenType == JsonTokenType.PropertyName)
            {
                if (clone.ValueTextEquals(FieldBytes))
                {
                    clone.Read();
                    var type = clone.GetString();
                    if (type is null || !ValueToType.TryGetValue(type, out var t))
                    {
                        if (FallbackType is not null)
                        {
                            break;
                        }
                        throw new NotSupportedException($"Unknown field value {FieldName}={(type is null ? "null" : $"'{type}'")}");
                    }
                    return JsonSerializer.Deserialize(ref reader, t, options) as TBase ?? throw new NotSupportedException($"Object with field value {FieldName}={type} deserialized as null");
                }
                clone.Skip();
            }
        }
        if (FallbackType is not null)
        {
            return JsonSerializer.Deserialize(ref reader, FallbackType, options) as TBase ?? throw new NotSupportedException($"Fallback object deserialized as null");
        }
        throw new NotSupportedException($"Missing '{FieldName}' field");
    }

    public override void Write(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options)
    {
        var type = value.GetType();
        if (GetFieldValue is not null)
        {
            var f = GetFieldValue(value);
            if (f is null || !ValueToType.TryGetValue(f, out var expected))
            {
                throw new InvalidOperationException($"We don't know what type to expect when serializing {FieldName}={(f is null ? "null" : $"'{f}'")}");
            }
            if (expected != type)
            {
                throw new InvalidOperationException($"When {FieldName}='{f}', we expected to serialize type {expected.FullName}, not {type.FullName}");
            }
        }
        JsonSerializer.Serialize(writer, value, type, options);
    }
}
