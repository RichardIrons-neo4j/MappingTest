namespace HydrationPrototype;

public class NodeHydratableBase
{
    private Dictionary<Type, List<object>> _hydratedRelationships = new();

    public void AddRelationship<T>(object relation)
    {
        List<object> list;
        if (!_hydratedRelationships.TryGetValue(typeof(T), out list))
        {
            list = new List<object>();
            _hydratedRelationships[typeof(T)] = list;
        }

        list.Add(relation);
    }

    public IEnumerable<object> GetRelations<T>()
    {
        if (_hydratedRelationships.TryGetValue(typeof(T), out var list))
        {
            return list;
        }

        return Enumerable.Empty<object>();
    }
}
