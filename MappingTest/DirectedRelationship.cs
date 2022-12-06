using HydrationPrototype.Attributes;

namespace MappingTest;

[GenerateHydrationFromRelationship]
public partial class DirectedRelationship
{
    [RelationshipStart]
    public Person Director { get; private set; } = new();

    [RelationshipEnd]
    public Movie Movie { get; private set; } = new();
}
