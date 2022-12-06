using Neo4j.Driver;

namespace MappingTest;

public class ExampleQuery
{
    private readonly IDriver _driver;

    public ExampleQuery()
    {
        _driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "password"));
    }

    public async Task<List<IRecord>> GetRecordsAsync()
    {
        const string query = """
            MATCH (movie:Movie)<-[relationship:ACTED_IN|DIRECTED]-(person:Person)
            WHERE movie.title =~ '(The|A) .*'
            RETURN movie, relationship, person
            """;

        await using var session = _driver.AsyncSession();

        return await session.ExecuteReadAsync(
            async tx =>
            {
                var cursor = await tx.RunAsync(query);
                return await cursor.ToListAsync();
            });
    }
}
