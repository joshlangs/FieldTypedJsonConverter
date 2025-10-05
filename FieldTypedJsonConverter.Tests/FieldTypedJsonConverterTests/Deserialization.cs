using System.Text.Json;

namespace FieldTypedJsonConverter.Tests.FieldTypedJsonConverterTests;
class Deserialization
{
    readonly FieldTypedJsonConverter<TestBase> Converter = new("Type");
    readonly JsonSerializerOptions Options = new();

    public Deserialization()
    {
        Converter.Map("Test1", typeof(Derived1));
        Converter.Map("Test2", typeof(Derived2));
        Converter.Map("Test3", typeof(Derived3));
        Options.Converters.Add(Converter);
    }

    public static IEnumerable<Func<(string Json, bool Meow, string? Something1)>> GetDerived1Tests()
    {
        yield return () => (@"{""Type"": ""Test1""}", false, null);
        yield return () => (@"{""Type"": ""Test1"", ""A"": ""B"" }", false, null);
        yield return () => (@"{""Type"": ""Test1"", ""Meow"": true}", true, null);
        yield return () => (@"{""Type"": ""Test1"", ""meow"": true}", false, null);
        yield return () => (@"{""Type"": ""Test1"", ""Something1"": ""Okay""}", false, "Okay");
        yield return () => (@"{""Type"": ""Test1"", ""Something1"": ""Okay"", ""Meow"": true}", true, "Okay");
        yield return () => (@"{""Something1"": ""Okay"", ""Type"": ""Test1"", ""Meow"": true}", true, "Okay");
        yield return () => (@"{""Something1"": ""Okay"", ""Meow"": true, ""Type"": ""Test1""}", true, "Okay");
    }

    public static IEnumerable<Func<(string Json, bool Meow, string? Something2)>> GetDerived2Tests()
    {
        yield return () => (@"{""Type"": ""Test2""}", false, null);
        yield return () => (@"{""Type"": ""Test2"", ""A"": ""B"" }", false, null);
        yield return () => (@"{""Type"": ""Test2"", ""Meow"": true}", true, null);
        yield return () => (@"{""Type"": ""Test2"", ""meow"": true}", false, null);
        yield return () => (@"{""Type"": ""Test2"", ""Something2"": ""Okay""}", false, "Okay");
        yield return () => (@"{""Type"": ""Test2"", ""Something2"": ""Okay"", ""Meow"": true}", true, "Okay");
        yield return () => (@"{""Something2"": ""Okay"", ""Type"": ""Test2"", ""Meow"": true}", true, "Okay");
        yield return () => (@"{""Something2"": ""Okay"", ""Meow"": true, ""Type"": ""Test2""}", true, "Okay");
    }

    public static IEnumerable<Func<(string Json, bool Meow, string? Something2, string? Something3)>> GetDerived3Tests()
    {
        yield return () => (@"{""Type"": ""Test3""}", false, null, null);
        yield return () => (@"{""Type"": ""Test3"", ""A"": ""B"" }", false, null, null);
        yield return () => (@"{""Type"": ""Test3"", ""Meow"": true}", true, null, null);
        yield return () => (@"{""Type"": ""Test3"", ""meow"": true}", false, null, null);
        yield return () => (@"{""Type"": ""Test3"", ""Something2"": ""Okay""}", false, "Okay", null);
        yield return () => (@"{""Type"": ""Test3"", ""Something2"": ""Okay"", ""Meow"": true}", true, "Okay", null);
        yield return () => (@"{""Something2"": ""Okay"", ""Type"": ""Test3"", ""Meow"": true}", true, "Okay", null);
        yield return () => (@"{""Something2"": ""Okay"", ""Meow"": true, ""Type"": ""Test3""}", true, "Okay", null);
        yield return () => (@"{""Type"": ""Test3"", ""Something3"": ""Rawr""}", false, null, "Rawr");
        yield return () => (@"{""Type"": ""Test3"", ""A"": ""B"", ""Something3"": ""Rawr"" }", false, null, "Rawr");
        yield return () => (@"{""Type"": ""Test3"", ""Meow"": true, ""Something3"": ""Rawr""}", true, null, "Rawr");
        yield return () => (@"{""Type"": ""Test3"", ""meow"": true, ""Something3"": ""Rawr""}", false, null, "Rawr");
        yield return () => (@"{""Type"": ""Test3"", ""Something2"": ""Okay"", ""Something3"": ""Rawr""}", false, "Okay", "Rawr");
        yield return () => (@"{""Type"": ""Test3"", ""Something2"": ""Okay"", ""Meow"": true, ""Something3"": ""Rawr""}", true, "Okay", "Rawr");
        yield return () => (@"{""Something2"": ""Okay"", ""Type"": ""Test3"", ""Meow"": true, ""Something3"": ""Rawr""}", true, "Okay", "Rawr");
        yield return () => (@"{""Something2"": ""Okay"", ""Meow"": true, ""Type"": ""Test3"", ""Something3"": ""Rawr""}", true, "Okay", "Rawr");
        yield return () => (@"{""Something2"": ""Okay"", ""Meow"": true, ""Type"": ""Test3"", ""something3"": ""Rawr""}", true, "Okay", null);
    }

    [Test]
    [MethodDataSource(typeof(Deserialization), nameof(GetDerived1Tests))]
    public async Task DeserializeTestBase_Test1(string json, bool meow, string? something1)
    {
        var x = JsonSerializer.Deserialize<TestBase>(json, Options);
        var d = (x as Derived1)!;

        await Assert.That(x).IsTypeOf<Derived1>();
        await Assert.That(d.Type).IsEqualTo("Test1");
        await Assert.That(d.Meow).IsEqualTo(meow);
        await Assert.That(d.Something1).IsEqualTo(something1);
    }

    [Test]
    [MethodDataSource(typeof(Deserialization), nameof(GetDerived2Tests))]
    public async Task DeserializeTestBase_Test2(string json, bool meow, string? something2)
    {
        var x = JsonSerializer.Deserialize<TestBase>(json, Options);
        var d = (x as Derived2)!;

        await Assert.That(x).IsTypeOf<Derived2>();
        await Assert.That(d.Type).IsEqualTo("Test2");
        await Assert.That(d.Meow).IsEqualTo(meow);
        await Assert.That(d.Something2).IsEqualTo(something2);
    }

    [Test]
    [MethodDataSource(typeof(Deserialization), nameof(GetDerived3Tests))]
    public async Task DeserializeTestBase_Test3(string json, bool meow, string? something2, string? something3)
    {
        var x = JsonSerializer.Deserialize<TestBase>(json, Options);
        var d = (x as Derived3)!;

        await Assert.That(x).IsTypeOf<Derived3>();
        await Assert.That(d.Type).IsEqualTo("Test3");
        await Assert.That(d.Meow).IsEqualTo(meow);
        await Assert.That(d.Something2).IsEqualTo(something2);
        await Assert.That(d.Something3).IsEqualTo(something3);
    }

    [Test]
    [MethodDataSource(typeof(Deserialization), nameof(GetDerived3Tests))]
    public async Task DeserializeNonBase(string json, bool meow, string? something2, string? something3)
    {
        var x = JsonSerializer.Deserialize<Derived1>(json, Options)!;

        await Assert.That(x).IsTypeOf<Derived1>();
        await Assert.That(x.Type).IsEqualTo("Test3");
        await Assert.That(x.Meow).IsEqualTo(meow);
    }
}
