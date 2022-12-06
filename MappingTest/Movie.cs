using HydrationPrototype.Attributes;

namespace MappingTest;

[GenerateHydrationFromNode]
public partial class Movie
{
    [HydrateFromProperty]
    public string? Tagline { get; private set; }

    [HydrateFromProperty]
    public string? Title { get; private set; }

    [HydrateFromProperty]
    public int Released { get; private set; }
}
