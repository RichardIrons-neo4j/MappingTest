using Neo4j.Driver;

namespace HydrationPrototype.Interfaces;

public interface INodeHydratable
{
    void HydrateFromNode(INode node);

    void AddRelationship<T>(object relation);
    IEnumerable<object> GetRelations<T>();
}
