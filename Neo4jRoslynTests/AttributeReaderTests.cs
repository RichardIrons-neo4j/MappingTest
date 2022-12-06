using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using HydrationPrototype;
using HydrationPrototype.RoslynHelpers;

namespace Neo4jRoslynTests;

public class AttributeReaderTests
{
    private record RoslynClassContext(
        SyntaxTree SyntaxTree,
        SemanticModel SemanticModel,
        ClassDeclarationSyntax ClassDeclarationSyntax,
        ISymbol ClassSymbol);

    private RoslynClassContext GetContext(string sourceCode, string className)
    {
        //The location of the .NET assemblies
        var assemblyLocation = typeof(object).Assembly.Location;
        var assemblyPath = Path.GetDirectoryName(assemblyLocation)!;

        MetadataReference[] standardReferences = {
            MetadataReference.CreateFromFile(assemblyLocation),
            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "mscorlib.dll")),
            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.dll")),
            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll")),
            MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll"))
        };

        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        var compilation = CSharpCompilation.Create("MyCompilation")
            .AddSyntaxTrees(syntaxTree)
            .AddReferences(standardReferences)
            .AddReferences(MetadataReference.CreateFromFile(this.GetType().Assembly.Location));

        var semanticModel = compilation.GetSemanticModel(syntaxTree);

        var syntaxRoot = syntaxTree.GetRoot();
        var classNode = syntaxRoot
            .DescendantNodes()
            .OfType<ClassDeclarationSyntax>()
            .First(c => c.Identifier.Text == className);

        var symbol = semanticModel.GetDeclaredSymbol(classNode)!;

        return new(syntaxTree, semanticModel, classNode, symbol);
    }

    [Test]
    public void ShouldReadSimpleAttribute()
    {
        const string sourceCode = """
            using Neo4jRoslynTests;
            [NoParamsAttribute]
            public class TestClass
            {
            }
            """;

        var (_, _, _, symbol) = GetContext(sourceCode, "TestClass");

        var attributes = symbol.GetAttributeInstances<NoParamsAttribute>().ToList();
        attributes.Should().HaveCount(1);
        attributes.First().Should().Match(a => a is NoParamsAttribute);
    }

    [Test]
    public void ShouldReadOneParamAttribute()
    {
        const string sourceCode = """
            using Neo4jRoslynTests;
            [OneParam("expected")]
            public class TestClass
            {
            }
            """;

        var (_, _, _, symbol) = GetContext(sourceCode, "TestClass");

        var attributes = symbol.GetAttributeInstances<OneParamAttribute>().ToList();
        attributes.Should().HaveCount(1);
        attributes.First().Value.Should().Be("expected");
    }

    [Test]
    public void ShouldReadTwoParamAttribute()
    {
        const string sourceCode = """
            using Neo4jRoslynTests;
            [TwoParam("expected", 69)]
            public class TestClass
            {
            }
            """;

        var (_, _, _, symbol) = GetContext(sourceCode, "TestClass");

        var attributes = symbol.GetAttributeInstances<TwoParamAttribute>().ToList();
        attributes.Should().HaveCount(1);
        attributes.First().Value1.Should().Be("expected");
        attributes.First().Value2.Should().Be(69);
    }

    [Test]
    public void ShouldReadMixedValueAttributeWithoutProperties()
    {
        const string sourceCode = """
            using Neo4jRoslynTests;
            [MixedValue("abc")]
            public class TestClass
            {
            }
            """;

        var (_, _, _, symbol) = GetContext(sourceCode, "TestClass");
        var attributes = symbol.GetAttributeInstances<MixedValueAttribute>().ToList();
        attributes.Should().HaveCount(1);
        attributes.First().ConstructorParam.Should().Be("abc");
    }

    [Test]
    public void ShouldReadMixedValueAttributeWithOneProperty()
    {
        const string sourceCode = """
            using Neo4jRoslynTests;
            [MixedValue("abc", IntProperty = 69)]
            public class TestClass
            {
            }
            """;

        var (_, _, _, symbol) = GetContext(sourceCode, "TestClass");
        var attributes = symbol.GetAttributeInstances<MixedValueAttribute>().ToList();
        attributes.Should().HaveCount(1);
        attributes.First().ConstructorParam.Should().Be("abc");
        attributes.First().IntProperty.Should().Be(69);
    }

    [Test]
    public void ShouldReadMixedValueAttributeWithOtherProperty()
    {
        const string sourceCode = """
            using Neo4jRoslynTests;
            [MixedValue("abc", StringProperty = "def")]
            public class TestClass
            {
            }
            """;

        var (_, _, _, symbol) = GetContext(sourceCode, "TestClass");
        var attributes = symbol.GetAttributeInstances<MixedValueAttribute>().ToList();
        attributes.Should().HaveCount(1);
        attributes.First().ConstructorParam.Should().Be("abc");
        attributes.First().StringProperty.Should().Be("def");
    }

    [Test]
    public void ShouldReadMixedValueAttributeWithBothProperties()
    {
        const string sourceCode = """
            using Neo4jRoslynTests;
            [MixedValue("abc", StringProperty = "def", IntProperty = 69)]
            public class TestClass
            {
            }
            """;

        var (_, _, _, symbol) = GetContext(sourceCode, "TestClass");
        var attributes = symbol.GetAttributeInstances<MixedValueAttribute>().ToList();
        attributes.Should().HaveCount(1);
        attributes.First().ConstructorParam.Should().Be("abc");
        attributes.First().StringProperty.Should().Be("def");
        attributes.First().IntProperty.Should().Be(69);
    }

    [Test]
    public void ShouldCorrectlyGetSingleAttribute()
    {
        const string sourceCode = """
            using Neo4jRoslynTests;
            [OneParam("expected")]
            public class TestClass
            {
            }
            """;

        var (_, _, _, symbol) = GetContext(sourceCode, "TestClass");

        bool success = symbol.TryGetSingleAttribute<OneParamAttribute>(out var attribute);
        success.Should().BeTrue();
        if (success)
        {
            attribute!.Value.Should().Be("expected");
        }
    }

    [Test]
    public void ShouldCorrectlyFailToGetSingleAttribute()
    {
        const string sourceCode = """
            using Neo4jRoslynTests;
            public class TestClass
            {
            }
            """;

        var (_, _, _, symbol) = GetContext(sourceCode, "TestClass");

        bool success = symbol.TryGetSingleAttribute<MultiUseAttribute>(out var attribute);
        success.Should().BeFalse();
    }

    [Test]
    public void MultipleAttributesShouldFailGettingSingleAttribute()
    {
        const string sourceCode = """
            using Neo4jRoslynTests;
            [MultiUseAttribute(1)]
            [MultiUseAttribute(2)]
            [MultiUseAttribute(3)]
            public class TestClass
            {
            }
            """;

        var (_, _, _, symbol) = GetContext(sourceCode, "TestClass");

        bool success = symbol.TryGetSingleAttribute<MultiUseAttribute>(out var attribute);
        success.Should().BeFalse();
    }

    [Test]
    public void ShouldCorrectlyGetMultipleAttributes()
    {
        const string sourceCode = """
            using Neo4jRoslynTests;
            [MultiUseAttribute(0)]
            [MultiUseAttribute(1)]
            [MultiUseAttribute(2)]
            public class TestClass
            {
            }
            """;

        var (_, _, _, symbol) = GetContext(sourceCode, "TestClass");

        var attributes = symbol.GetAttributeInstances<MultiUseAttribute>().ToArray();
        attributes.Should().HaveCount(3);
        attributes[0].Value.Should().Be(0);
        attributes[1].Value.Should().Be(1);
        attributes[2].Value.Should().Be(2);
    }
}
