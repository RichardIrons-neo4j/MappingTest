using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HydrationPrototype.RoslynHelpers;

public record ClassCompileInfo(ClassDeclarationSyntax Syntax, INamedTypeSymbol Symbol)
{
    public bool HasAttribute<T>() where T : Attribute => Symbol.HasAttribute<T>();
    public bool IsPartial() => Syntax.IsPartial();
};
