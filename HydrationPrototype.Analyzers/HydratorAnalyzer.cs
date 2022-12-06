using System.Collections.Immutable;
using HydrationPrototype.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace HydrationPrototype.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class HydratorAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "N4JHY001";

    private const string Title = "Mark class as partial";
    private const string MessageFormat = "Class marked with a hydration attribute must be marked partial";

    private const string Description = "Classes marked with a hydration attribute must be partial classes, because " +
                                       "the generator generates another part to the class that implements the hydration.";

    private const string Category = "Hydration";

    private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
        Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

    private static readonly ImmutableArray<DiagnosticDescriptor> RuleArray = ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze |
                                               GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ClassDeclaration);
    }

    private void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var cls = (ClassDeclarationSyntax)context.Node;
        var symbol = (ITypeSymbol)context.SemanticModel.GetDeclaredSymbol(cls)!;

        var generateMapperAttribute =
            symbol.GetAttributeInstances<GenerateHydrationFromNodeAttribute>().Any()
            || symbol.GetAttributeInstances<GenerateHydrationFromRecordAttribute>().Any();

        if (!generateMapperAttribute || cls.Modifiers.Any(mod => mod.IsKind(SyntaxKind.PartialKeyword)))
        {
            return;
        }

        var list = string.Join(",", cls.Modifiers.Select(m => m.Text));

        var diagnostic = Diagnostic.Create(Rule, cls.Identifier.GetLocation());
        context.ReportDiagnostic(diagnostic);
    }

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => RuleArray;
}
