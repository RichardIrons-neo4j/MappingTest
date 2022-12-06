using HydrationPrototype.Attributes;

namespace MappingTest;

[GenerateHydrationFromNode]
public partial class Person
{
    [HydrateFromProperty("name")]
    public string Name { get; set; } = string.Empty;

    [HydrateFromProperty("born")]
    public int Born { get; set; }
}
