using System;

namespace EqualityGeneratorAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class EqualityIgnoreAttribute : Attribute
    {
    }

}
