using Neo4j.Driver;

namespace HydrationPrototype.Interfaces;

public interface IRecordHydratable
{
    void HydrateFromRecord(IRecord record, IDictionary<string, object> instances);
}
