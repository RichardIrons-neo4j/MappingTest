using NUnit.Framework;

namespace HydrationTests;

public class GenericHydrationTests
{
    protected Dictionary<string, object> TestData = new()
    {
        ["String1"] = "String1",
        ["String2"] = "String2",
        ["String3"] = "String3",
        ["String4"] = "String4",
        ["Int1"] = 1,
        ["Int2"] = 2,
        ["Int3"] = 3,
        ["Int4"] = 4,
    };

    protected void AssertCorrectContent(HydrationTestClass obj)
    {
        Assert.AreEqual("String1", obj.String1);
        Assert.AreEqual("String2", obj.String2);
        Assert.AreEqual("String3", obj.String3);
        Assert.AreEqual("String4", obj.String4);
        Assert.AreEqual(1, obj.Int1);
        Assert.AreEqual(2, obj.Int2);
        Assert.AreEqual(3, obj.Int3);
        Assert.AreEqual(4, obj.Int4);
    }
}
