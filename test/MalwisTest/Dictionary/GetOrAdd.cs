using Malwis.Extensions.Dictionary;

namespace MalwisTest.Dictionary;

public class GetOrAdd
{
    private static readonly IDictionary<string, string> BaseDictionary = new Dictionary<string, string>
    {
        {"key1", "value1"}, {"key2", "value2"}, {"key3", "value3"}
    };

    [Fact]
    public void GetOrAdd_Add()
    {
        // Arrange
        IDictionary<string, string> copy = new Dictionary<string, string>(BaseDictionary);

        const string key = "key4";
        const string expectedValue = "defaultValue";
        
        // Act
        string value = copy.GetOrAdd(key, expectedValue);

        // Assert
        Assert.True(copy.ContainsKey(key));
        Assert.Equal(expectedValue, copy[key]);
        
        Assert.Equal(expectedValue, value);
    }

    [Fact]
    public void GetOrAdd_Get()
    {
        // Arrange
        IDictionary<string, string> copy = new Dictionary<string, string>(BaseDictionary);

        const string key = "key2";
        string expectedValue = BaseDictionary[key];

        // Act
        string value = copy.GetOrAdd(key, expectedValue);
        
        // Assert
        Assert.True(copy.ContainsKey(key));
        Assert.Equal(expectedValue, copy[key]);

        Assert.Equal(expectedValue, value);
    }

    [Fact]
    public void GetOrAdd_CreationFunc()
    {
        // Arrange
        IDictionary<string, string> copy = new Dictionary<string, string>(BaseDictionary);

        const string key = "key4";
        string ValueCreator() => "defaultValue";
        string expectedValue = ValueCreator();

        // Act
        string value = copy.GetOrAdd(key, ValueCreator);
        
        // Assert
        Assert.True(copy.ContainsKey(key));
        Assert.Equal(expectedValue, copy[key]);

        Assert.Equal(expectedValue, value);
        
    }
}
