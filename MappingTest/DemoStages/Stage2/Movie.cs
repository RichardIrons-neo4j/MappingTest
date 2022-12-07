using HydrationPrototype.Attributes;

namespace MappingTest.DemoStages.Stage2;

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
