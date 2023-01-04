using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using HydrationPrototype;
using MappingTest.DemoStages;
using MappingTest.DemoStages.Stage1;
using MappingTest.DemoStages.Stage2;
using MappingTest.DemoStages.Stage3;
using MappingTest.DemoStages.Stage4;
using MappingTest.DemoStages.Stage5;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using Serilog;
using TestQueryRow = MappingTest.DemoStages.Stage4.TestQueryRow;

namespace MappingTest;

public class Benchmarks
{
    private List<IRecord>? _records;

    [GlobalSetup]
    public void GlobalSetup()
    {
        var serviceProvider = Program.BuildServices();
        _records = serviceProvider.GetRequiredService<ExampleQuery>().GetRecordsAsync().GetAwaiter().GetResult();
    }

    [Benchmark]
    public void DoHydration()
    {
        _records.AsObjects<TestQueryRow>().ToList();
    }
}


public class Program
{
    public static IServiceProvider BuildServices()
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
            .AddTransient<IDemoStage, DemoStage4>()
            .AddTransient<IDemoStage, DemoStage5>();

        return services.BuildServiceProvider();
    }

    public static async Task Main()
    {
        var summary = BenchmarkRunner.Run<Benchmarks>();
        return;

        var serviceProvider = BuildServices();
        var demoStages = serviceProvider.GetRequiredService<IEnumerable<IDemoStage>>().ToList();

        int stage = 0;
        do
        {
            string input;
            do
            {
                Console.Write("Stage: ");
                input = Console.ReadLine()!;

            } while (!(int.TryParse(input, out stage) && stage is >= 0 and <= 5));

            if (stage != 0)
            {
                var demoStage = demoStages.First(d => d.Stage == stage);
                await demoStage.RunAsync();
            }

        } while (stage != 0);
    }
}
