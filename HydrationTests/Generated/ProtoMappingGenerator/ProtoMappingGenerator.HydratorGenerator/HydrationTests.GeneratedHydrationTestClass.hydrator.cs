using ProtoMappingGenerator;
namespace HydrationTests;
public partial class GeneratedHydrationTestClass : IDictionaryHydratable
{
    public void Hydrate(Dictionary<string, object> source)
    {
        String1 = (System.String)source["String1"];
        String2 = (System.String)source["String2"];
        String3 = (System.String)source["String3"];
        String4 = (System.String)source["String4"];
        Int1 = (System.Int32)source["Int1"];
        Int2 = (System.Int32)source["Int2"];
        Int3 = (System.Int32)source["Int3"];
        Int4 = (System.Int32)source["Int4"];
    }
}