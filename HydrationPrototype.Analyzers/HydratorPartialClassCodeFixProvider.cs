using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HydrationPrototype.Analyzers;

//[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(HydratorPartialClassCodeFixProvider)), Shared]
public class HydratorPartialClassCodeFixProvider : CodeFixProvider
{
    private const string Title = "Add partial keyword";

    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(HydratorAnalyzer.DiagnosticId);

    public sealed override FixAllProvider GetFixAllProvider()
    {
        return WellKnownFixAllProviders.BatchFixer;
    }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        var node = root?.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf()?.OfType<ClassDeclarationSyntax>()?.First();

        // Register a code action that will invoke the fix.
        context.RegisterCodeFix(
            CodeAction.Create(
                title: Title,
                createChangedDocument: c => FixAsync(context.Document, node!, c),
                equivalenceKey: nameof(HydratorPartialClassCodeFixProvider)),
            diagnostic);
    }

    private static async Task<Document> FixAsync(Document contextDocument, ClassDeclarationSyntax declaration, CancellationToken cancellationToken)
    {
        var newModifiers = declaration.Modifiers.Add(SyntaxFactory.Token(SyntaxKind.PartialKeyword));
        var newClass = declaration.WithModifiers(newModifiers);
        var oldRoot = await contextDocument.GetSyntaxRootAsync(cancellationToken);
        var newRoot = oldRoot!.ReplaceNode(declaration, newClass);
        return contextDocument.WithSyntaxRoot(newRoot);
    }
}
