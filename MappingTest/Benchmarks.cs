// using System.Reflection;
// using BenchmarkDotNet.Attributes;
// using BenchmarkDotNet.Running;
// using Microsoft.Extensions.Logging;
// using HydrationPrototype.Generators;
//
// namespace MappingTest;
//
// public interface IBenchmarks
// {
//     void Run();
// }
//
// public class Benchmarks : IBenchmarks
// {
//     private IDictionaryHydrator _dictionaryHydrator;
//     private IFallbackDictionaryHydrator _fallbackHydrator;
//     private ILogger<Benchmarks> _logger;
//
//     private readonly Dictionary<string, object> _benchmarkDictionary = new Dictionary<string, object>
//     {
//         ["Name"] = "Richard",
//         ["Age"] = 45
//     };
//
//     [Benchmark]
//     public void BenchmarkReflection()
//     {
//         _fallbackHydrator.GetHydrated<TestClass>(_benchmarkDictionary);
//     }
//
//     [Benchmark]
//     public void BenchmarkGenerated()
//     {
//         _dictionaryHydrator.GetHydrated<TestClass>(_benchmarkDictionary);
//     }
//
//     public Benchmarks()
//     {
//         var loggerFactory = LoggerFactory.Create(_ => { });
//         _logger = loggerFactory.CreateLogger<Benchmarks>();
//         _fallbackHydrator = new ReflectionDictionaryHydrator();
//         _dictionaryHydrator =
//             new DictionaryHydrator(_fallbackHydrator, loggerFactory.CreateLogger<DictionaryHydrator>());
//     }
//
//     public void Run()
//     {
//         var summary = BenchmarkRunner.Run<Benchmarks>();
//     }
// }
