using HydrationPrototype.Attributes;
using MappingTest.DemoStages.Stage2;

namespace MappingTest.DemoStages.Stage3;

[GenerateHydrationFromRecord]
public partial class TestQueryRow
{
    [HydrateFromNode]
    public Person Person { get; private set; } = null!;

    [HydrateFromNode]
    public Movie Movie { get; private set; } = null!;
}
