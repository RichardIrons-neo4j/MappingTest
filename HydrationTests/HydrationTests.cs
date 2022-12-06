using MappingTest;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using ProtoMappingGenerator;
using Serilog;

namespace HydrationTests;

public class HydrationTests : GenericHydrationTests
{
    [Test]
    public void ShouldHydrateCorrectlyWithReflection()
    {
        var reflectionHydrator = new ReflectionDictionaryHydrator();
        var result = reflectionHydrator.GetHydrated<HydrationTestClass>(TestData);
        AssertCorrectContent(result);
    }

    [Test]
    public void ShouldHydrateCorrectlyWithGeneratedCode()
    {
        var autoMocker = new AutoMocker(MockBehavior.Loose);
        var hydrator = autoMocker.CreateInstance<DictionaryHydrator>();
        var result = hydrator.GetHydrated<GeneratedHydrationTestClass>(TestData);
        AssertCorrectContent(result);
    }

    [Test]
    public void ShouldUseFallbackHydratorIfNotIHydratable()
    {
        var autoMocker = new AutoMocker(MockBehavior.Loose);
        var hydrator = autoMocker.CreateInstance<DictionaryHydrator>();
        var result = hydrator.GetHydrated<HydrationTestClass>(TestData);
        autoMocker.GetMock<IFallbackDictionaryHydrator>()
            .Verify(x => x.Hydrate(It.IsAny<HydrationTestClass>(), TestData), Times.Once);
    }

    [Test]
    public void ShouldNotUseFallbackHydratorIfIHydratable()
    {
        var autoMocker = new AutoMocker(MockBehavior.Loose);
        var hydrator = autoMocker.CreateInstance<DictionaryHydrator>();
        var result = hydrator.GetHydrated<GeneratedHydrationTestClass>(TestData);
        autoMocker.GetMock<IFallbackDictionaryHydrator>()
            .Verify(x => x.Hydrate(It.IsAny<HydrationTestClass>(), TestData), Times.Never);
    }
}
