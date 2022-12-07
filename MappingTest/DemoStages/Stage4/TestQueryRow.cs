using HydrationPrototype.Attributes;
using MappingTest.DemoStages.Stage2;

namespace MappingTest.DemoStages.Stage4;

[GenerateHydrationFromRecord]
public partial class TestQueryRow
{
    [HydrateFromNode]
    public Person Person { get; private set; } = null!;

    [HydrateFromNode]
    public Movie Movie { get; private set; } = null!;

    [HydrateFromRelationship("relationship", "ACTED_IN")]
    public ActedInRelationship? ActedInRelationship { get; private set; }

    [HydrateFromRelationship("relationship", "DIRECTED")]
    public DirectedRelationship? DirectedRelationship { get; private set; }
}
