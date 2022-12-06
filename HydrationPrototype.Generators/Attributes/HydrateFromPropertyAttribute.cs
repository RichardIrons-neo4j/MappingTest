namespace HydrationPrototype.Attributes;

public class HydrateFromPropertyAttribute : Attribute
{
    public string? PropertyName { get; }

    public HydrateFromPropertyAttribute(string? propertyName = null)
    {
        PropertyName = propertyName;
    }
}
