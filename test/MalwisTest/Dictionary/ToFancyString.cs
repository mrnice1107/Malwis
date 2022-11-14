using Malwis.Extensions.Dictionary;

namespace MalwisTest.Dictionary;

[UsesVerify]
public class ToFancyString
{
    private static readonly Dictionary<object, object> TestDict = new()
    {
        {"strKey", 3},
        {4, "strValue" },
        {1.2, -10 }
    };

    [Fact]
    public Task TestToFancyString()
    {
        string fancyString = TestDict.ToFancyString();

        return Verify(fancyString);
    }

    [Fact]
    public Task TestToFancyString_DoWhiteSpaces()
    {
        string fancyString = TestDict.ToFancyString(doWhiteSpaces: true);

        return Verify(fancyString);
    }

    [Fact]
    public Task TestToFancyString_DoNewLines()
    {
        string fancyString = TestDict.ToFancyString(doNewLines: true);

        return Verify(fancyString);
    }

    [Fact]
    public Task TestToFancyString_WhiteSpaceNewLine()
    {
        string fancyString = TestDict.ToFancyString(doWhiteSpaces:true, doNewLines:true);

        return Verify(fancyString);
    }

    [Fact]
    public Task TestToFancyString_Empty()
    {
        string fancyString = new Dictionary<object, object>().ToFancyString();

        return Verify(fancyString);
    }
}
