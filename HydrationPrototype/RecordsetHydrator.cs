using HydrationPrototype.Interfaces;
using Neo4j.Driver;

namespace HydrationPrototype;

public static class RecordsetHydrator
{
    private static T GetHydratedObject<T>(IRecord record, Dictionary<string, object> instances)
        where T : IRecordHydratable, new()
    {
        var result = new T();
        result.HydrateFromRecord(record, instances);
        return result;
    }

    public static IEnumerable<T> AsObjects<T>(this IEnumerable<IRecord> records) where T : IRecordHydratable, new()
    {
        var instances = new Dictionary<string, object>();
        return records.Select(r => GetHydratedObject<T>(r, instances));
    }

    public static IEnumerable<T> AsObjects<T>(this IEnumerable<IRecord> records, string nodeName)
        where T : INodeHydratable, new()
    {
        var instances = new Dictionary<string, T>();
        foreach (var record in records)
        {
            var node = record[nodeName].As<INode>();

            if (!instances.TryGetValue(node.ElementId, out var item))
            {
                item = new T();
                item.HydrateFromNode(node);
                instances[node.ElementId] = item;
            }

            yield return item;
        }
    }
}
