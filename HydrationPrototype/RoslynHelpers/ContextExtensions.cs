using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HydrationPrototype.RoslynHelpers;

public static class ContextExtensions
{
    public static bool TryGetClassCompileInfo(this GeneratorSyntaxContext context, out ClassCompileInfo classCompileInfo)
    {
        if (context.Node is not ClassDeclarationSyntax classDeclarationSyntax)
        {
            classCompileInfo = null!;
            return false;
        }

        var symbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax)!;
        classCompileInfo = new ClassCompileInfo(
            classDeclarationSyntax,
            (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax)!);

        return true;
    }
}
