namespace FieldTypedJsonConverter.Tests.FieldTypedJsonConverterTests;
class DoFieldValueValidation
{
    readonly FieldTypedJsonConverter<TestBase> Converter = new("Type");

    [Test]
    public async Task DefaultsFalse() => await Assert.That(Converter.IsValidatingFieldValue).IsFalse();

    [Test]
    public async Task SetsTrue()
    {
        Converter.DoFieldValueValidation(x => x.Type);

        await Assert.That(Converter.IsValidatingFieldValue).IsTrue();
    }

    [Test]
    public async Task NullSetsFalse()
    {
        Converter.DoFieldValueValidation(null);

        await Assert.That(Converter.IsValidatingFieldValue).IsFalse();

        Converter.DoFieldValueValidation(x => x.Type);
        Converter.DoFieldValueValidation(null);

        await Assert.That(Converter.IsValidatingFieldValue).IsFalse();
    }

    [Test]
    public async Task ThrowsAfterLocked()
    {
        Converter.CanConvert(typeof(string));

        await Assert.That(() => Converter.DoFieldValueValidation(x => x.Type)).Throws<InvalidOperationException>();
        await Assert.That(() => Converter.DoFieldValueValidation(null)).ThrowsNothing();
    }
}
