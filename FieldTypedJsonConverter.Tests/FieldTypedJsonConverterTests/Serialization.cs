using System.Text.Json;

namespace FieldTypedJsonConverter.Tests.FieldTypedJsonConverterTests;
class Serialization
{
    readonly FieldTypedJsonConverter<TestBase> Converter = new("Type");
    readonly JsonSerializerOptions Options = new();

    public Serialization()
    {
        Converter.Map("Test1", typeof(Derived1));
        Converter.Map("Test2", typeof(Derived2));
        Converter.Map("Test3", typeof(Derived3));
        Options.Converters.Add(Converter);
    }

    [Test]
    public async Task Derived1_UntypedRoundtrips()
    {
        var x = new Derived1 { Type = "Test1", Meow = true, Something1 = "Something" };

        var json = JsonSerializer.Serialize(x, Options);
        var x2 = JsonSerializer.Deserialize<TestBase>(json, Options)!;
        var d = (x2 as Derived1)!;

        await Assert.That(d).IsTypeOf(x.GetType());
        await Assert.That(d.Type).IsEqualTo(x.Type);
        await Assert.That(d.Meow).IsEqualTo(x.Meow);
        await Assert.That(d.Something1).IsEqualTo(x.Something1);
    }

    [Test]
    public async Task Derived2_UntypedRoundtrips()
    {
        var x = new Derived2 { Type = "Test2", Meow = true, Something2 = "Something" };

        var json = JsonSerializer.Serialize(x, Options);
        var x2 = JsonSerializer.Deserialize<TestBase>(json, Options)!;
        var d = (x2 as Derived2)!;

        await Assert.That(d).IsTypeOf(x.GetType());
        await Assert.That(d.Type).IsEqualTo(x.Type);
        await Assert.That(d.Meow).IsEqualTo(x.Meow);
        await Assert.That(d.Something2).IsEqualTo(x.Something2);
    }

    [Test]
    public async Task Derived3_UntypedRoundtrips()
    {
        var x = new Derived3 { Type = "Test3", Meow = true, Something2 = "Something", Something3 = "Else" };

        var json = JsonSerializer.Serialize(x, Options);
        var x2 = JsonSerializer.Deserialize<TestBase>(json, Options)!;
        var d = (x2 as Derived3)!;

        await Assert.That(d).IsTypeOf(x.GetType());
        await Assert.That(d.Type).IsEqualTo(x.Type);
        await Assert.That(d.Meow).IsEqualTo(x.Meow);
        await Assert.That(d.Something2).IsEqualTo(x.Something2);
        await Assert.That(d.Something3).IsEqualTo(x.Something3);
    }

    [Test]
    public async Task Derived1_TestBaseRoundtrips()
    {
        var x = new Derived1 { Type = "Test1", Meow = true, Something1 = "Something" };

        var json = JsonSerializer.Serialize<TestBase>(x, Options);
        var x2 = JsonSerializer.Deserialize<TestBase>(json, Options)!;
        var d = (x2 as Derived1)!;

        await Assert.That(d).IsTypeOf(x.GetType());
        await Assert.That(d.Type).IsEqualTo(x.Type);
        await Assert.That(d.Meow).IsEqualTo(x.Meow);
        await Assert.That(d.Something1).IsEqualTo(x.Something1);
    }

    [Test]
    public async Task Derived2_TestBaseRoundtrips()
    {
        var x = new Derived2 { Type = "Test2", Meow = true, Something2 = "Something" };

        var json = JsonSerializer.Serialize<TestBase>(x, Options);
        var x2 = JsonSerializer.Deserialize<TestBase>(json, Options)!;
        var d = (x2 as Derived2)!;

        await Assert.That(d).IsTypeOf(x.GetType());
        await Assert.That(d.Type).IsEqualTo(x.Type);
        await Assert.That(d.Meow).IsEqualTo(x.Meow);
        await Assert.That(d.Something2).IsEqualTo(x.Something2);
    }

    [Test]
    public async Task Derived3_TestBaseRoundtrips()
    {
        var x = new Derived3 { Type = "Test3", Meow = true, Something2 = "Something", Something3 = "Else" };

        var json = JsonSerializer.Serialize<TestBase>(x, Options);
        var x2 = JsonSerializer.Deserialize<TestBase>(json, Options)!;
        var d = (x2 as Derived3)!;

        await Assert.That(d).IsTypeOf(x.GetType());
        await Assert.That(d.Type).IsEqualTo(x.Type);
        await Assert.That(d.Meow).IsEqualTo(x.Meow);
        await Assert.That(d.Something2).IsEqualTo(x.Something2);
        await Assert.That(d.Something3).IsEqualTo(x.Something3);
    }


    [Test]
    public async Task Derived1_TestBaseRoundtrips_Validated()
    {
        Converter.DoFieldValueValidation(x => x.Type);
        var x = new Derived1 { Type = "Test1", Meow = true, Something1 = "Something" };

        var json = JsonSerializer.Serialize<TestBase>(x, Options);
        var x2 = JsonSerializer.Deserialize<TestBase>(json, Options)!;
        var d = (x2 as Derived1)!;

        await Assert.That(d).IsTypeOf(x.GetType());
        await Assert.That(d.Type).IsEqualTo(x.Type);
        await Assert.That(d.Meow).IsEqualTo(x.Meow);
        await Assert.That(d.Something1).IsEqualTo(x.Something1);
    }

    [Test]
    public async Task Derived2_TestBaseRoundtrips_Validated()
    {
        Converter.DoFieldValueValidation(x => x.Type);
        var x = new Derived2 { Type = "Test2", Meow = true, Something2 = "Something" };

        var json = JsonSerializer.Serialize<TestBase>(x, Options);
        var x2 = JsonSerializer.Deserialize<TestBase>(json, Options)!;
        var d = (x2 as Derived2)!;

        await Assert.That(d).IsTypeOf(x.GetType());
        await Assert.That(d.Type).IsEqualTo(x.Type);
        await Assert.That(d.Meow).IsEqualTo(x.Meow);
        await Assert.That(d.Something2).IsEqualTo(x.Something2);
    }

    [Test]
    public async Task Derived3_TestBaseRoundtrips_Validated()
    {
        Converter.DoFieldValueValidation(x => x.Type);
        var x = new Derived3 { Type = "Test3", Meow = true, Something2 = "Something", Something3 = "Else" };

        var json = JsonSerializer.Serialize<TestBase>(x, Options);
        var x2 = JsonSerializer.Deserialize<TestBase>(json, Options)!;
        var d = (x2 as Derived3)!;

        await Assert.That(d).IsTypeOf(x.GetType());
        await Assert.That(d.Type).IsEqualTo(x.Type);
        await Assert.That(d.Meow).IsEqualTo(x.Meow);
        await Assert.That(d.Something2).IsEqualTo(x.Something2);
        await Assert.That(d.Something3).IsEqualTo(x.Something3);
    }

    [Test]
    public async Task Derived1_WrongType()
    {
        Converter.DoFieldValueValidation(x => x.Type);
        var x = new Derived1 { Type = "Test2", Meow = true, Something1 = "Something" };

        await Assert.That(() => JsonSerializer.Serialize<TestBase>(x, Options)).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Derived1_UnknownType()
    {
        Converter.DoFieldValueValidation(x => x.Type);

        var x = new Derived1 { Type = "asdf", Meow = true, Something1 = "Something" };

        await Assert.That(() => JsonSerializer.Serialize<TestBase>(x, Options)).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Derived1_NullType()
    {
        Converter.DoFieldValueValidation(x => x.Type);

        var x = new Derived1 { Type = null!, Meow = true, Something1 = "Something" };

        await Assert.That(() => JsonSerializer.Serialize<TestBase>(x, Options)).Throws<InvalidOperationException>();
    }
}
