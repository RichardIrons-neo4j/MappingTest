using HydrationPrototype;
using Microsoft.Extensions.Logging;

namespace MappingTest.DemoStages.Stage3;

public class DemoStage3 : IDemoStage
{
    public int Stage => 3;

    private readonly ExampleQuery _exampleQuery;
    private readonly ILogger<DemoStage3> _logger;

    public DemoStage3(
        ExampleQuery exampleQuery,
        ILogger<DemoStage3> logger)
    {
        _exampleQuery = exampleQuery;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        var records = await _exampleQuery.GetRecordsAsync();
        var rows = records.AsObjects<TestQueryRow>();
        foreach (var row in rows)
        {
            _logger.LogDebug(
                "Movie: {Title}, Person: {Person}",
                row.Movie.Title,
                row.Person.Name);
        }
    }
}
