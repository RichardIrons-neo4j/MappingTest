using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HydrationPrototype;
using Neo4j.Driver;
using Serilog;

namespace MappingTest;

public class Program
{
    private readonly ILogger<Program> _logger;
    private readonly ExampleQuery _exampleQuery;

    public Program(
        ILogger<Program> logger,
        ExampleQuery exampleQuery)
    {
        _logger = logger;
        _exampleQuery = exampleQuery;
    }

    public Program()
    {
        var serilog = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Verbose()
            .CreateLogger();

        var loggerFactory = LoggerFactory.Create(lb => lb.AddSerilog(serilog));

        _logger = loggerFactory.CreateLogger<Program>();
        _exampleQuery = new ExampleQuery();
    }

    private static IServiceProvider BuildServices()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Verbose()
            .CreateLogger();

        var services = new ServiceCollection()
            .AddTransient<Program>()
            .AddTransient<ExampleQuery>()
            .AddLogging(l => l.AddSerilog());

        return services.BuildServiceProvider();
    }

    private async Task Run()
    {
        var records = await _exampleQuery.GetRecordsAsync();

        _logger.LogDebug("Hydrated records:");
        var hydratedRecords = records.AsObjects<TestQueryRow>().ToList();
        foreach (var hydrated in hydratedRecords)
        {
            if (hydrated.ActedInRelationship is { } acted)
            {
                _logger.LogDebug(
                    "{Movie} starred {Actor} as {Role}",
                    acted.Movie.Title,
                    acted.Actor.Name,
                    acted.Roles.First());
            }
            else if (hydrated.DirectedRelationship is { } directed )
            {
                _logger.LogDebug("{Movie} was directed by {Director}", directed.Movie.Title, directed.Director.Name);
            }
        }

        var tomHanks = hydratedRecords.Select(h => h.Person).First(p => p.Name == "Tom Hanks");
        _logger.LogDebug("Actor {Actor} has appeared in:", tomHanks.Name);
        foreach (var relation in tomHanks.GetRelations<ActedInRelationship>())
        {
            var movie = relation.As<Movie>();
            var otherActors = movie.GetRelations<ActedInRelationship>().Where(a => a != tomHanks);
            var costars = string.Join(", ", otherActors.Select(a => a.As<Person>().Name));
            var director = movie.GetRelations<DirectedRelationship>().First().As<Person>();
            _logger.LogDebug("{Movie} with {Costars}, directed by {Director}", movie.Title, costars, director.Name);
        }

        Console.ReadLine();
    }

    public static async Task Main()
    {
        var serviceProvider = BuildServices();
        await serviceProvider.GetRequiredService<Program>().Run();
        await Task.Yield();
    }
}
