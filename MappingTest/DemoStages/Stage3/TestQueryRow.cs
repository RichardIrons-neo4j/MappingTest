using HydrationPrototype.Attributes;
using MappingTest.DemoStages.Stage2;

namespace MappingTest.DemoStages.Stage3;

[GenerateHydrationFromRecord]
public partial class TestQueryRow
{
    [HydrateFromNode]
    public MappingTest.DemoStages.Stage2.Person Person { get; private set; } = null!;

    [HydrateFromNode]
    public MappingTest.DemoStages.Stage2.Movie Movie { get; private set; } = null!;
}
