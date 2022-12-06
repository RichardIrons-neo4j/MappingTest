using System.Text;
using HydrationPrototype.RoslynHelpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace HydrationPrototype.Generators;

public abstract class PropertyHydrationGenerator<TMarkerAttribute> : ISourceGenerator
    where TMarkerAttribute : Attribute
{
    protected class SyntaxContextReceiver : ISyntaxContextReceiver
    {
        public readonly List<ClassCompileInfo> ClassesToGenerateMappers = new();

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (
                context.TryGetClassCompileInfo(out var info)
                && info.HasAttribute<TMarkerAttribute>()
                && info.IsPartial())
            {
                ClassesToGenerateMappers.Add(info);
            }
        }
    }

    public virtual void Execute(GeneratorExecutionContext context)
    {
        List<string> logLines = new() { $"// Receiver type is {context.SyntaxContextReceiver?.GetType().Name}" };

        if (context.SyntaxContextReceiver is SyntaxContextReceiver rcv)
        {
            logLines.Add($"// Receiver has {rcv.ClassesToGenerateMappers.Count} entries to generate for");
            logLines.AddRange(rcv.ClassesToGenerateMappers.Select(c => $"//  - {c.Symbol.Name}"));
        }

        context.AddSource(GetType().Name + ".log", SourceText.From(string.Join("\n", logLines), Encoding.UTF8));

        if (context.SyntaxContextReceiver is not SyntaxContextReceiver receiver)
        {
            return;
        }

        foreach (var classCompileInfo in receiver.ClassesToGenerateMappers)
        {
            var sourceCode = GetGeneratedSourceCode(classCompileInfo);
            context.AddSource(
                $"{classCompileInfo.Symbol.GetFullName()}.{GetType().Name}",
                SourceText.From(sourceCode, Encoding.UTF8));
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SyntaxContextReceiver());
    }

    public abstract string GetGeneratedSourceCode(ClassCompileInfo classCompileInfo);
    protected abstract string GetPropertySetter(IPropertySymbol property, string sourceProperty, Attribute attribute);

    protected string GetPropertySetterCode<TAttribute>(
        ClassCompileInfo classCompileInfo,
        Func<TAttribute, string?> getNameOverride) where TAttribute : Attribute
    {
        var lines = classCompileInfo.Symbol.GetPropertiesAndSourceNames(getNameOverride)
            .Select(p => GetPropertySetter(p.symbol, p.sourceName, p.attribute));

        return string.Join("\n", lines);
    }
}
