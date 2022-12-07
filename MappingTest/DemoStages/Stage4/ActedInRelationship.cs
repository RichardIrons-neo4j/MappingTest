using HydrationPrototype.Attributes;

namespace MappingTest.DemoStages.Stage4;

[GenerateHydrationFromRelationship]
public partial class ActedInRelationship
{
    [RelationshipStart]
    public MappingTest.DemoStages.Stage2.Person Actor { get; private set; } = new();

    [RelationshipEnd]
    public MappingTest.DemoStages.Stage2.Movie Movie { get; private set; } = new();

    [HydrateFromProperty("roles")]
    public List<string> Roles { get; private set; } = new();
}
