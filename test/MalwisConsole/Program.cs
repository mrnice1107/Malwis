// See https://aka.ms/new-console-template for more information

using Malwis.Extensions.Dictionary;

Dictionary<string, int> testDict = new()
    {
        {"_1",0 },
        {"_2",0 },
        {"_3",1 },
        {"_4",1 }
    };

Console.WriteLine(testDict.ToFancyString());
Console.WriteLine(testDict.ToFancyString(doWhiteSpaces: true));
Console.WriteLine(testDict.ToFancyString(doNewLines: true));
