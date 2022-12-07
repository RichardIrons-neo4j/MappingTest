using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HydrationPrototype;
using MappingTest.DemoStages;
using MappingTest.DemoStages.Stage1;
using MappingTest.DemoStages.Stage2;
using MappingTest.DemoStages.Stage3;
using MappingTest.DemoStages.Stage4;
using Neo4j.Driver;
using Serilog;

namespace MappingTest;

public class Program
{
    private readonly ILogger<Program> _logger;
    private readonly IEnumerable<IDemoStage> _demoStages;
    private readonly ExampleQuery _exampleQuery;

    public Program(
        ILogger<Program> logger,
        IEnumerable<IDemoStage> demoStages,
        ExampleQuery exampleQuery)
    {
        _logger = logger;
        _demoStages = demoStages;
        _exampleQuery = exampleQuery;
    }

    public Program()
    {
        var serilog = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Verbose()
            .CreateLogger();

        var loggerFactory = LoggerFactory.Create(lb => lb.AddSerilog(serilog));

        _demoStages = Enumerable.Empty<IDemoStage>();
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
            .AddLogging(l => l.AddSerilog())
            .AddTransient<IDemoStage, DemoStage1>()
            .AddTransient<IDemoStage, DemoStage2>()
            .AddTransient<IDemoStage, DemoStage3>()
            .AddTransient<IDemoStage, DemoStage4>();

        return services.BuildServiceProvider();
    }

    private async Task Run()
    {
        var demoStage = _demoStages.First(d => d.Stage == 4);
        await demoStage.RunAsync();
        Console.ReadLine();
        //return;

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
