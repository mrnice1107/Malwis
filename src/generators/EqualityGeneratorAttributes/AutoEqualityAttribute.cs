using System;

namespace EqualityGeneratorAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class AutoEqualityAttribute : Attribute
    {
    }
}
