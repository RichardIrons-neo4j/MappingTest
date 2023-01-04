using System.Text;
using Microsoft.CodeAnalysis;

namespace HydrationPrototype;

public static class SymbolExtensions
{
    private static Dictionary<ISymbol, string> _fullNames = new(SymbolEqualityComparer.IncludeNullability);
    public static string GetFullName(this ISymbol symbol)
    {
        if (_fullNames.TryGetValue(symbol, out var result))
        {
            return result;
        }

        var spaces = new List<string>();
        var containing = symbol;
        while (containing != null)
        {
            spaces.Insert(0, containing.Name);
            containing = containing.ContainingNamespace;
        }

        result = new string(string.Join(".", spaces).Skip(1).ToArray());
        _fullNames[symbol] = result;
        return result;
    }

    public static bool MatchesTypeByName<T>(this ISymbol symbol)
    {
        return symbol.GetFullMetadataName() == typeof(T).FullName;
    }

    public static bool IsAttribute<T>(this AttributeData attributeData) where T : Attribute
    {
        return attributeData.AttributeClass?.MatchesTypeByName<T>() ?? false;
    }

    public static IEnumerable<ITypeSymbol> GetBaseTypesAndThis(this ITypeSymbol type)
    {
        var current = type;
        while (current != null)
        {
            yield return current;

            current = current.BaseType;
        }
    }

    public static IEnumerable<T> GetAttributeInstances<T>(this ISymbol symbol) where T : Attribute
    {
        var relevantAttributes = symbol
            .GetAttributes()
            .Where(att => att.IsAttribute<T>());

        foreach (var attribute in relevantAttributes)
        {
            var constructorArgs = attribute.ConstructorArguments.Select(a => a.Value).ToArray();
            var instance = Activator.CreateInstance(typeof(T), constructorArgs);

            foreach (var namedArgument in attribute.NamedArguments)
            {
                var (name, value) = (namedArgument.Key, namedArgument.Value.Value);
                typeof(T).GetProperty(name)?.SetMethod.Invoke(instance, new[] { value });
            }

            yield return (T)instance;
        }
    }

    public static bool TryGetSingleAttribute<T>(this ISymbol symbol, out T attribute)
        where T : Attribute
    {
        using var instances = symbol.GetAttributeInstances<T>().GetEnumerator();
        if (instances.MoveNext())
        {
            attribute = instances.Current!;
            if (!instances.MoveNext())
            {
                return true;
            }
        }

        attribute = null!;
        return false;
    }

    public static bool HasAttribute<T>(this ISymbol symbol) where T : Attribute
    {
        return symbol.GetAttributeInstances<T>().Any();
    }

    public static IEnumerable<IPropertySymbol> GetPropertyMembers(
        this INamedTypeSymbol symbol,
        Func<IPropertySymbol, bool>? predicate = null,
        bool includeInherited = true)
    {
        predicate ??= ps => true;
        var membersToCheck = includeInherited
            ? symbol.GetBaseTypesAndThis().SelectMany(s => s.GetMembers())
            : symbol.GetMembers();

        return membersToCheck
            .OfType<IPropertySymbol>()
            .Where(predicate);
    }

    public static IEnumerable<(IPropertySymbol symbol, string sourceName, T attribute)> GetPropertiesAndSourceNames<T>(
        this INamedTypeSymbol symbol,
        Func<T, string?> getSourceName) where T : Attribute
    {
        var propertySymbols = symbol.GetPropertyMembers(ps => !ps.IsReadOnly)
            .Select(ps => new { ps, attr = ps.GetAttributeInstances<T>().FirstOrDefault() })
            .Where(ps => ps.attr is not null);

        foreach (var propertySymbol in propertySymbols)
        {
            var sourceName = getSourceName(propertySymbol.attr) ?? propertySymbol.ps.Name.ToLower();
            yield return (propertySymbol.ps, sourceName, propertySymbol.attr);
        }
    }

    private static Dictionary<ISymbol, string> _fullMetadataNames = new(SymbolEqualityComparer.IncludeNullability);

    public static string GetFullMetadataName(this ISymbol typeSymbol)
    {
        if (_fullMetadataNames.TryGetValue(typeSymbol, out var result))
        {
            return result;
        }

        result = typeSymbol.GetFullMetadataNameInternal();
        _fullMetadataNames[typeSymbol] = result;
        return result;
    }

    private static string GetFullMetadataNameInternal(this ISymbol s)
    {
        if (s.IsRootNamespace())
        {
            return string.Empty;
        }

        var sb = new StringBuilder(s.GetDisplayName(), 500);
        var last = s;

        s = s.ContainingSymbol;

        while (!IsRootNamespace(s))
        {
            if (s is ITypeSymbol && last is ITypeSymbol)
            {
                sb.Insert(0, '+');
            }
            else
            {
                sb.Insert(0, '.');
            }

            sb.Insert(0, s.GetDisplayName());
            s = s.ContainingSymbol;
        }

        return sb.ToString();
    }

    public static string GetDisplayName(this ISymbol symbol)
    {
        if (symbol is not INamedTypeSymbol { IsGenericType: true } nts)
        {
            return symbol.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
        }

        var typeArgumentNames = nts.TypeArguments.Select(ta => ta.GetFullName());
        var typeArgString = $"<{string.Join(", ", typeArgumentNames)}>";
        return $"{symbol.Name}{typeArgString}";
    }

    private static bool IsRootNamespace(this ISymbol symbol)
    {
        return symbol is INamespaceSymbol { IsGlobalNamespace: true };
    }
}
