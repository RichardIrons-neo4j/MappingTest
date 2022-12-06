// ReSharper disable ClassNeverInstantiated.Global
namespace Neo4jRoslynTests;

[AttributeUsage(AttributeTargets.Class)]
public class NoParamsAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
public class OneParamAttribute : Attribute
{
    public string Value { get; }

    public OneParamAttribute(string value)
    {
        Value = value;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class TwoParamAttribute : Attribute
{
    public string Value1 { get; }
    public int Value2 { get; }

    public TwoParamAttribute(string value1, int value2)
    {
        Value1 = value1;
        Value2 = value2;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class MixedValueAttribute : Attribute
{
    public string ConstructorParam { get; }
    public string StringProperty { get; set; } = string.Empty;
    public int IntProperty { get; set; }

    public MixedValueAttribute(string constructorParam)
    {
        ConstructorParam = constructorParam;
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class MultiUseAttribute : Attribute
{
    public int Value { get; }

    public MultiUseAttribute(int value)
    {
        Value = value;
    }
}
