using HydrationPrototype.Attributes;

namespace MappingTest;

[GenerateHydrationFromRecord]
public partial class TestQueryRow
{
    [HydrateFromNode("person")]
    public Person Person { get; private set; } = null!;

    [HydrateFromRelationship("relationship", "ACTED_IN")]
    public ActedInRelationship? ActedInRelationship { get; private set; }

    [HydrateFromRelationship("relationship", "DIRECTED")]
    public DirectedRelationship? DirectedRelationship { get; private set; }

    [HydrateFromNode("movie")]
    public Movie Movie { get; private set; } = null!;
}
