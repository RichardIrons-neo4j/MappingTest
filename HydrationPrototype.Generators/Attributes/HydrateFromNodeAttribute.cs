namespace HydrationPrototype.Attributes;

public class HydrateFromNodeAttribute : Attribute
{
    public string? NodeName { get; }

    public HydrateFromNodeAttribute(string? nodeName = null)
    {
        NodeName = nodeName;
    }
}
