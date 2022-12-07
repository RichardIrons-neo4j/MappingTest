using Microsoft.Extensions.Logging;
using Neo4j.Driver;

namespace MappingTest.DemoStages.Stage1;

public class DemoStage1 : IDemoStage
{
    private readonly ExampleQuery _exampleQuery;
    private readonly ILogger<DemoStage1> _logger;

    public DemoStage1(
        ExampleQuery exampleQuery,
        ILogger<DemoStage1> logger)
    {
        _exampleQuery = exampleQuery;
        _logger = logger;
    }

    public int Stage => 1;

    public async Task RunAsync()
    {
        var records = await _exampleQuery.GetRecordsAsync();
        foreach (var record in records)
        {
            var person = record["person"].As<INode>()["name"].As<string>();
            var relationship = record["relationship"].As<IRelationship>().Type;
            var title = record["movie"].As<INode>()["title"].As<string>();
            _logger.LogDebug("{Person} {Relationship} {Movie}", person, relationship, title);
        }
    }
}
