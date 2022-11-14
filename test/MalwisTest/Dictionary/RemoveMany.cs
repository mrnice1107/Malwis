using Malwis.Extensions.Dictionary;

namespace MalwisTest.Dictionary;

public class RemoveMany
{
    private static readonly IDictionary<string, string> TestDictionary = new Dictionary<string, string>
    {
        {"key1", "value1"}, {"key2", "value2"}, {"key3", "value3"}
    };

    [Fact]
    public void RemoveManyTest()
    {
        IDictionary<string,string> removeMany = TestDictionary.RemoveMany("key1", "key3");

        Assert.DoesNotContain("key1", removeMany);
        Assert.DoesNotContain("key3", removeMany);
        Assert.Contains("key2", removeMany);
    }
}
