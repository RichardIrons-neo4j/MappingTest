using HydrationPrototype.Attributes;

namespace MappingTest;

[GenerateHydrationFromRelationship]
public partial class ActedInRelationship
{
    [RelationshipStart]
    public Person Actor { get; private set; } = new();

    [RelationshipEnd]
    public Movie Movie { get; private set; } = new();

    [HydrateFromProperty("roles")]
    public List<string> Roles { get; private set; } = new();
}
