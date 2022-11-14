using Malwis.Extensions.Dictionary;

namespace MalwisTest.Dictionary;

public class IsNullOrEmpty
{
    private static readonly IDictionary<string, string> NullDictionary = null;
    private static readonly IDictionary<string, string> EmptyDictionary = new Dictionary<string, string>();

    private static readonly IDictionary<string, string> NotEmptyDictionary = new Dictionary<string, string>
    {
        {"key", "value"}
    };


    [Fact]
    public void IsNullOrEmpty_IsNull() => Assert.True(DictionaryExtensions.DictionaryIsNullOrEmpty(NullDictionary));

    [Fact]
    public void IsNullOrEmpty_IsEmpty() => Assert.True(DictionaryExtensions.DictionaryIsNullOrEmpty(EmptyDictionary));

    [Fact]
    public void IsNullOrEmpty_IsNotEmpty() => Assert.False(DictionaryExtensions.DictionaryIsNullOrEmpty(NotEmptyDictionary));
}
