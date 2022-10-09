using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EqualityGenerator.Extensions
{
    internal static class MemberDeclarationExtension
    {
        internal static bool HasAttribute(this MemberDeclarationSyntax memberDeclaration, string fullAttribute, string shortAttribute)
        {
            if (memberDeclaration == null)
            {
                throw new ArgumentNullException(nameof(memberDeclaration));
            }

            foreach (AttributeListSyntax attributeList in memberDeclaration.AttributeLists)
            {
                if (AttributeListHasAttribute(attributeList, fullAttribute, shortAttribute))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool AttributeListHasAttribute(AttributeListSyntax attributeList, string fullAttribute,
            string shortAttribute)
        {
            foreach (AttributeSyntax attribute in attributeList.Attributes)
            {
                if (AttributeIsAttributeString(attribute, fullAttribute, shortAttribute))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool AttributeIsAttributeString(AttributeSyntax attribute, string fullAttribute, string shortAttribute) =>
            attribute.Name is IdentifierNameSyntax nameSyntax && (
                nameSyntax.Identifier.Text.Equals(fullAttribute) || 
                nameSyntax.Identifier.Text.Equals(shortAttribute));
    }
}
