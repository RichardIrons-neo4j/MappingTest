using HydrationPrototype.Attributes;
using HydrationPrototype.Interfaces;
using HydrationPrototype.RoslynHelpers;
using Microsoft.CodeAnalysis;

namespace HydrationPrototype.Generators;

[Generator]
public class NodeHydratorGenerator : PropertyHydrationGenerator<GenerateHydrationFromNodeAttribute>
{
    protected override string GetPropertySetter(IPropertySymbol property, string sourceProperty, Attribute attribute)
    {
        return $"{property.Name} = node.Properties[\"{sourceProperty}\"].As<{property.Type.GetFullName()}>();"; }

    public override string GetGeneratedSourceCode(ClassCompileInfo classCompileInfo)
    {
        var propertySetterCode =
            GetPropertySetterCode<HydrateFromPropertyAttribute>(classCompileInfo, a => a.PropertyName);

        return $@" // <autogenerated />
using System;
using System.Collections.Generic;
using HydrationPrototype.Interfaces;
using Neo4j.Driver;
namespace {classCompileInfo.Symbol.ContainingNamespace.Name};
public partial class {classCompileInfo.Syntax.Identifier} : {nameof(INodeHydratable)}
{{
    public void HydrateFromNode(INode node)
    {{
{propertySetterCode}
    }}

    private Dictionary<Type, List<object>> _hydrated_relationships = new();
    public void AddRelationship<T>(object relation)
    {{
        List<object> list;
        if(!_hydrated_relationships.TryGetValue(typeof(T), out list))
        {{
            list = new();
            _hydrated_relationships[typeof(T)] = list;
        }}
        list.Add(relation);
    }}

    public IEnumerable<object> GetRelations<T>()
    {{
        if(_hydrated_relationships.TryGetValue(typeof(T), out var list))
        {{
            return list;
        }}
        return Enumerable.Empty<object>();
    }}
}}";
    }
}
