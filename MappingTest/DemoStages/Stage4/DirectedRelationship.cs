using HydrationPrototype.Attributes;

namespace MappingTest.DemoStages.Stage4;

[GenerateHydrationFromRelationship]
public partial class DirectedRelationship
{
    [RelationshipStart]
    public MappingTest.DemoStages.Stage2.Person Director { get; private set; } = new();

    [RelationshipEnd]
    public MappingTest.DemoStages.Stage2.Movie Movie { get; private set; } = new();
}
