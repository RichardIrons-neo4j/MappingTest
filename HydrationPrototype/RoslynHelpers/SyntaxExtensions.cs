using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HydrationPrototype.RoslynHelpers;

public static class SyntaxExtensions
{
    public static bool IsPartial(this ClassDeclarationSyntax classDeclarationSyntax)
    {
        return classDeclarationSyntax
            .Modifiers
            .Any(m => m.IsKind(SyntaxKind.PartialKeyword));
    }
}
