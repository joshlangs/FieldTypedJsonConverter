namespace FieldTypedJsonConverter.Tests.FieldTypedJsonConverterTests;
class Ctor
{
    [Test]
    public async Task NullFieldName() => await Assert.That(() => new FieldTypedJsonConverter<TestBase>(null!)).Throws<ArgumentNullException>();
    [Test]
    public async Task EmptyFieldName() => await Assert.That(() => new FieldTypedJsonConverter<TestBase>("")).Throws<ArgumentException>();
    [Test]
    public async Task WhitespaceFieldName() => await Assert.That(() => new FieldTypedJsonConverter<TestBase>(" ")).Throws<ArgumentException>();
    [Test]
    public async Task NonAbstractBase() => await Assert.That(() => new FieldTypedJsonConverter<NonAbstractTestBase>("Test")).Throws<InvalidOperationException>();
}
