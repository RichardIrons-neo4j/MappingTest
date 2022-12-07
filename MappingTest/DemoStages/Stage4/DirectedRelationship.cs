using HydrationPrototype.Attributes;
using MappingTest.DemoStages.Stage2;

namespace MappingTest.DemoStages.Stage4;

[GenerateHydrationFromRelationship]
public partial class DirectedRelationship
{
    [RelationshipStart]
    public Person Director { get; private set; } = new();

    [RelationshipEnd]
    public Movie Movie { get; private set; } = new();
}
