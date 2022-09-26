namespace Malwis.Exceptions.Platform;

public class PlatformException : Exception
{
    public PlatformID Platform { get; }

    public PlatformException(PlatformID platform, string reason, Exception innerException) : base(reason, innerException) => Platform = platform;
    public PlatformException(PlatformID platform, string reason) : base(reason) => Platform = platform;
}


public class UnsupportedPlatformException : PlatformException
{
    public PlatformID ExpectedPlatform { get; }

    public UnsupportedPlatformException(PlatformID expected, PlatformID actual, string reason, Exception innerException) : base(actual, reason, innerException) => ExpectedPlatform = expected;
    public UnsupportedPlatformException(PlatformID expected, PlatformID actual, string reason) : base(actual, reason) => ExpectedPlatform = expected;
    public UnsupportedPlatformException(PlatformID expected, PlatformID actual) : base(actual, GetDefaultReason(expected, actual)) => ExpectedPlatform = expected;

    private static string GetDefaultReason(PlatformID expected, PlatformID actual) => $"Expected platform: {expected}, Actual: {actual}";
}
