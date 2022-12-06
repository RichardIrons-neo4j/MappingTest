using ProtoMappingGenerator;

namespace HydrationTests;

public class HydrationTestClass
{
    public string String1 { get; set; } = string.Empty;
    public string String2 { get; set; } = string.Empty;
    public string String3 { get; set; } = string.Empty;
    public string String4 { get; set; } = string.Empty;

    public int Int1 { get; set; }
    public int Int2 { get; set; }
    public int Int3 { get; set; }
    public int Int4 { get; set; }
}

[GenerateHydrator]
public partial class GeneratedHydrationTestClass : HydrationTestClass
{

}
