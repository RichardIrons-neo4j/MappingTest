using Neo4j.Driver;

namespace HydrationPrototype.Interfaces;

public interface IRelationshipHydratable
{
    void HydrateFromRelationship(IRelationship relationship, IDictionary<string, object> instances);
}
