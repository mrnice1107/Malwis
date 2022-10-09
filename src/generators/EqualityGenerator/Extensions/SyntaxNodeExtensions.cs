using System;
using Microsoft.CodeAnalysis;

namespace EqualityGenerator.Extensions
{
    public static class SyntaxNodeExtensions
    {
        public static T GetParentOfType<T>(this SyntaxNode syntaxNode)
        {
            SyntaxNode parent = syntaxNode.Parent;
            while (true)
            {
                switch (parent)
                {
                    case null:
                        throw new Exception();
                    case T t:
                        return t;
                    default:
                        parent = parent.Parent;
                        break;
                }
            }
        }
    }
}
