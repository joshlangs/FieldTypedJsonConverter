namespace FieldTypedJsonConverter.Tests;

abstract class TestBase
{
    public required string Type { get; init; }    
    public bool Meow { get; set; }
}
class NonAbstractTestBase
{
    public required string Type { get; init; }
    public bool Meow { get; set; }
}

class Derived1 : TestBase
{
    public string? Something1 { get; init; }
}
class Derived2 : TestBase
{
    public string? Something2 { get; init; }
}
class Derived3 : Derived2
{
    public string? Something3 { get; init; }
}
abstract class AbstractDerived: Derived2
{
}