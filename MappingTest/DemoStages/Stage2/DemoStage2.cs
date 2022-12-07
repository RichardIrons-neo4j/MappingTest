using HydrationPrototype;
using Microsoft.Extensions.Logging;

namespace MappingTest.DemoStages.Stage2;

public class DemoStage2 : IDemoStage
{
    public int Stage => 2;

    private readonly ExampleQuery _exampleQuery;
    private readonly ILogger<DemoStage2> _logger;

    public DemoStage2(
        ExampleQuery exampleQuery,
        ILogger<DemoStage2> logger)
    {
        _exampleQuery = exampleQuery;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        var records = await _exampleQuery.GetRecordsAsync();
        var movies = records.AsObjects<Movie>("movie");
        foreach (var movie in movies.DistinctBy(m => m.Title?.Trim().ToLower()))
        {
            _logger.LogDebug(
                "Title: <{Title}>, Released {Released}, Tagline: {Tagline}",
                movie.Title,
                movie.Released,
                movie.Tagline);
        }
    }
}
