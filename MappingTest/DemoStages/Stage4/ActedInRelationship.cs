using HydrationPrototype.Attributes;
using MappingTest.DemoStages.Stage2;

namespace MappingTest.DemoStages.Stage4;

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
