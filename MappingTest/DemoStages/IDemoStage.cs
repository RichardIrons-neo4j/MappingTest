namespace MappingTest.DemoStages;

public interface IDemoStage
{
    int Stage { get; }
    Task RunAsync();
}
