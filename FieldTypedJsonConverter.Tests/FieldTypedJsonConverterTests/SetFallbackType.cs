namespace FieldTypedJsonConverter.Tests.FieldTypedJsonConverterTests;
class SetFallbackType
{
    readonly FieldTypedJsonConverter<TestBase> Converter = new("Type");

    public static IEnumerable<Func<Type?>> GetSettableTypes()
    {
        yield return () => null;
        yield return () => typeof(Derived1);
        yield return () => typeof(Derived2);
        yield return () => typeof(Derived3);
    }
    public static IEnumerable<Func<Type>> GetUnsettableTypes()
    {
        yield return () => typeof(TestBase);
        yield return () => typeof(NonAbstractTestBase);
        yield return () => typeof(AbstractDerived);
    }

    [Test]
    [MethodDataSource(typeof(SetFallbackType), nameof(GetSettableTypes))]
    public async Task Settable(Type? type)
    {
        Converter.SetFallbackType(type);

        await Assert.That(Converter.FallbackType).IsEqualTo(type);
    }

    [Test]
    [MethodDataSource(typeof(SetFallbackType), nameof(GetUnsettableTypes))]
    public async Task Unsettable(Type type) => await Assert.That(() => Converter.SetFallbackType(type)).Throws<InvalidOperationException>();

    [Test]
    public async Task LockedAfterUse()
    {
        Converter.SetFallbackType(typeof(Derived1));
        Converter.CanConvert(typeof(Derived1));

        await Assert.That(() => Converter.SetFallbackType(typeof(Derived2))).Throws<InvalidOperationException>();
        await Assert.That(() => Converter.SetFallbackType(typeof(Derived1))).ThrowsNothing();
    }
}
