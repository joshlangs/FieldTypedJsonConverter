namespace FieldTypedJsonConverter.Tests.FieldTypedJsonConverterTests;
class Map
{
    readonly FieldTypedJsonConverter<TestBase> Converter = new("Type");

    public static IEnumerable<Func<Type?>> GetMappableTypes()
    {
        yield return () => typeof(Derived1);
        yield return () => typeof(Derived2);
        yield return () => typeof(Derived3);
    }
    public static IEnumerable<Func<Type>> GetUnmappableTypes()
    {
        yield return () => typeof(TestBase);
        yield return () => typeof(NonAbstractTestBase);
        yield return () => typeof(AbstractDerived);
    }

    [Test]
    public async Task NullFieldValue() => await Assert.That(() => Converter.Map(null!, typeof(Derived1))).Throws<ArgumentNullException>();

    [Test]
    public async Task NullType() => await Assert.That(() => Converter.Map("Test", null!)).Throws<ArgumentNullException>();

    [Test]
    [MethodDataSource(typeof(Map), nameof(GetMappableTypes))]
    public async Task MapDerived(Type type)
    {
        Converter.Map("Test", type);

        await Assert.That(Converter.GetMappedFieldValues())
            .HasSingleItem()
            .And.ContainsOnly(x => x == "Test");
        await Assert.That(Converter.GetMappedType("Test")).IsEqualTo(type);
    }

    [Test]
    [MethodDataSource(typeof(Map), nameof(GetUnmappableTypes))]
    public async Task MapUnmappable(Type type) => await Assert.That(() => Converter.Map("Test", type)).Throws<InvalidOperationException>();

    [Test]
    public async Task Remap()
    {
        Converter.Map("Test", typeof(Derived1));
        Converter.Map("Test", typeof(Derived2));

        await Assert.That(Converter.GetMappedFieldValues())
            .HasSingleItem()
            .And.ContainsOnly(x => x == "Test");
        await Assert.That(Converter.GetMappedType("Test")).IsEqualTo(typeof(Derived2));
    }

    [Test]
    public async Task LockedAfterUse()
    {
        Converter.Map("Test", typeof(Derived1));
        Converter.CanConvert(typeof(Derived1));

        await Assert.That(() => Converter.Map("Test", typeof(Derived2))).Throws<InvalidOperationException>();
        await Assert.That(() => Converter.Map("Test2", typeof(Derived1))).Throws<InvalidOperationException>();
        await Assert.That(() => Converter.Map("Test", typeof(Derived1))).ThrowsNothing();
    }
}
