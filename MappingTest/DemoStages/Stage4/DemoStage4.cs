using HydrationPrototype;
using Microsoft.Extensions.Logging;

namespace MappingTest.DemoStages.Stage4;

public class DemoStage4 : IDemoStage
{
    public int Stage => 4;

    private readonly ExampleQuery _exampleQuery;
    private readonly ILogger<DemoStage4> _logger;

    public DemoStage4(
        ExampleQuery exampleQuery,
        ILogger<DemoStage4> logger)
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
            switch (row)
            {
                case { ActedInRelationship: not null }:
                    _logger.LogDebug(
                        "{Actor} starred in {Movie} as {Character}",
                        row.Person.Name,
                        row.Movie.Title,
                        row.ActedInRelationship.Roles[0]);

                    break;

                case { DirectedRelationship: not null }:
                    _logger.LogDebug("{Director} directed {Movie}", row.Person.Name, row.Movie.Title);
                    break;
            }
        }
    }
}
