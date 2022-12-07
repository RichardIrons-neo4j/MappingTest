using MappingTest.DemoStages;
using MappingTest.DemoStages.Stage1;
using MappingTest.DemoStages.Stage2;
using MappingTest.DemoStages.Stage3;
using MappingTest.DemoStages.Stage4;
using MappingTest.DemoStages.Stage5;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MappingTest;

public class Program
{
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
            .AddTransient<IDemoStage, DemoStage4>()
            .AddTransient<IDemoStage, DemoStage5>();

        return services.BuildServiceProvider();
    }

    public static async Task Main()
    {
        var serviceProvider = BuildServices();
        var demoStages = serviceProvider.GetRequiredService<IEnumerable<IDemoStage>>();
        var demoStage = demoStages.First(d => d.Stage == 5);
        await demoStage.RunAsync();
        Console.ReadLine();
    }
}
