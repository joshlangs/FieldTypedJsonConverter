# FieldTypedJsonConverter

`FieldTypedJsonConverter` is a `JsonConverter` that allows for deserialization of objects based on a mapping from a field's value to a type.

Published on NuGet as `FieldTypedJsonConverter`

# The Basic Steps

1. Create an abstract base class
2. Derive types from it
3. Call `.Map` to map strings to types

# Example

Create your classes.  The `MessageBase` abstract class has a field `MessageType`.

```
abstract class MessageBase
{
    public string? MessageType { get; init; }
}

class PingMessage : MessageBase {}
class PongMessage : MessageBase {}
```

Create your converter.  Map field values.

```
var converter = new FieldTypedJsonConverter<MessageBase>("MessageType");
converter.Map("ping", typeof(PingMessage));
converter.Map("pong", typeof(PongMessage));
```

Add the converter to your `JsonSerializerOptions`.

```
var options = new JsonSerializationOptions();
options.Converters.Add(converter);
```

Now deserialize incoming messages, for example an HTTP POST request body.

```
var msg = JsonSerializer.Deserialize<MessageBase>(json, options);
```

The object returned depends on the field value present in the json.

```
var pingMessage = JsonSerializer.Deserialize<MessageBase>("{\"MessageType\": \"ping\"}", options);
var pongMessage = JsonSerializer.Deserialize<MessageBase>("{\"MessageType\": \"pong\"}", options);

Console.WriteLine(pingMessage.GetType().Name);
// PRINTS "PingMessage"

Console.WriteLine(pongMessage.GetType().Name);
// PRINTS "PongMessage"
```

# Fallback

If the field is missing, or the value wasn't mapped to a type, a `NotSupportedException` will be thrown.

Alternatively, you can call `SetFallbackType`.  If a fallback type is provided, the object will be deserialize as the fallback type instead of throwing the exception.

You could consider using `[JsonExtensionData]` on a field in the fallback type if you're still interested in the contents of the message.

# Rules

The base class must be abstract.

All mapped types (including the fallback type) must derive from the base class.

Once the converter has been used, no changes to mappings are allowed.

# Deserialization

Make sure to use the generic Deserialize call with the base type, in order to trigger the converter.

For example:  `JsonSerializer.Deserialize<MessageBase>(...`

A typical usage scenario might look like:
```
var msg = JsonSerializer.Deserialize<MessageBase>(json, options);
switch (msg)
{
    case PingMessage ping: return await HandlePingAsync(ping);
    case PongMessage pong: return await HandlePongAsync(pong);
}
```

# Serialization

Serialization is allowed, although this converter is typically used for deserialization scenarios.

Optionally, call `DoFieldValueValidation` if you want the field value to be checked according to the deserialization mapping.  Make sure to use the generic Serialize call with the base type if you want this behavior, or else the converter will not be triggered.
