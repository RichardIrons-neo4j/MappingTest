using ProtoMappingGenerator;
using Neo4j.Driver;
namespace HydrationTests;
public partial class GeneratedHydrationTestClass  : INodeHydratable
{
    public void HydrateFromNode(INode node)
    {
        String1 = node.Properties["String1"].As<System.String>();
        String2 = node.Properties["String2"].As<System.String>();
        String3 = node.Properties["String3"].As<System.String>();
        String4 = node.Properties["String4"].As<System.String>();
        Int1 = node.Properties["Int1"].As<System.Int32>();
        Int2 = node.Properties["Int2"].As<System.Int32>();
        Int3 = node.Properties["Int3"].As<System.Int32>();
        Int4 = node.Properties["Int4"].As<System.Int32>();
    }
}