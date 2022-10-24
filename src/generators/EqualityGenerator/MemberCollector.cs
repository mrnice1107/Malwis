using System.Collections.Generic;
using System.Linq;
using EqualityGenerator.Extensions;
using EqualityGeneratorAttributes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EqualityGenerator
{
    internal static class MemberCollector
    {
        internal static IEnumerable<PropertyDeclarationSyntax> GetEqualityProperties(TypeDeclarationSyntax equalityClass) => 
            from member in equalityClass.Members where MemberIsPropertyWithoutIgnoreAttribute(member) select member as PropertyDeclarationSyntax;

        private static bool MemberIsPropertyWithoutIgnoreAttribute(MemberDeclarationSyntax member) =>
            member is PropertyDeclarationSyntax &&
            !member.HasAttribute(nameof(EqualityIgnoreAttribute), nameof(EqualityIgnoreAttribute).Replace("Attribute",""));
    }
}
