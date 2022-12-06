namespace HydrationPrototype.Attributes;

public class HydrateFromRelationshipAttribute : Attribute
{
    public string RelationshipName { get; }

    public string Type { get; set; }

    public HydrateFromRelationshipAttribute(string relationshipName, string type)
    {
        RelationshipName = relationshipName;
        Type = type;
    }
}
