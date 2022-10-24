using System.Collections.Generic;
using System.Linq;
using System.Text;
using EqualityGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace EqualityGenerator.Factories
{
    internal static class EqualsFactory
    {
        static EqualsFactory()
        {
            SemicolonToken = Token(SyntaxKind.SemicolonToken);
            BooleanType = PredefinedType(Token(SyntaxKind.BoolKeyword));
            
            EquatableInterfaceIdentifier = Identifier("IEquatable");
            EqualsMethodName = IdentifierName("Equals");
            
            PublicModifier = TokenList(Token(SyntaxKind.PublicKeyword));
            PublicOverrideModifier = TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.OverrideKeyword));
            PublicStaticModifier = TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword));
            
            IdentifierNameSyntax allowNullAttribute = IdentifierName("AllowNull");
            NotNullAttributeList = SingletonList(
                AttributeList(
                    SingletonSeparatedList(
                        Attribute(allowNullAttribute)))
            );

            /*IdentifierNameSyntax pureAttribute = IdentifierName("Pure");
            MethodAttributes = List(
                    new[]
                    {
                        AttributeList(
                            SingletonSeparatedList(
                                Attribute(pureAttribute)))
                    });*/
        }

        private static readonly SyntaxToken EquatableInterfaceIdentifier;

        internal static readonly SyntaxList<AttributeListSyntax> NotNullAttributeList;
        //internal static readonly SyntaxList<AttributeListSyntax> MethodAttributes;

        internal static readonly IdentifierNameSyntax EqualsMethodName;
        
        internal static readonly SyntaxToken SemicolonToken;
        internal static readonly TypeSyntax BooleanType;
        
        internal static readonly SyntaxTokenList PublicModifier;
        internal static readonly SyntaxTokenList PublicOverrideModifier;
        internal static readonly SyntaxTokenList PublicStaticModifier;

        /*private static readonly UsingDirectiveSyntax UsingSysDirectContracts = UsingDirective(
            QualifiedName(
                QualifiedName(
                    IdentifierName("System"),
                    IdentifierName("Diagnostics")),
                IdentifierName("Contracts")));*/
            
        private static readonly UsingDirectiveSyntax UsingSysDirectCodeAnalysis = UsingDirective(
            QualifiedName(
                QualifiedName(
                    IdentifierName("System"),
                    IdentifierName("Diagnostics")),
                IdentifierName("CodeAnalysis")));

        internal static SourceText GenerateEqualsSourceCode(TypeDeclarationSyntax equalityClass)
        {
            FileScopedNamespaceDeclarationSyntax namespaceSyntax =
                equalityClass.GetParentOfType<FileScopedNamespaceDeclarationSyntax>();
            IEnumerable<IdentifierNameSyntax> equalityPropertyNames = MemberCollector
                .GetEqualityProperties(equalityClass).Select(s => IdentifierName(s.Identifier));

            CompilationUnitSyntax classExtension = CompilationUnit()
                .WithUsings(List(new[] { UsingSysDirectCodeAnalysis }))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        namespaceSyntax
                            .WithNamespaceKeyword(
                                Token(
                                    TriviaList(Comment("// <generated-code/>")),
                                    SyntaxKind.NamespaceKeyword,
                                    TriviaList()
                                ))
                            .WithMembers(
                                // class stuff here
                                SingletonList<MemberDeclarationSyntax>(
                                    ClassDeclaration(equalityClass.Identifier.Text)
                                        .WithModifiers(equalityClass.Modifiers)
                                        .WithBaseList(
                                            GenerateEquatableInterface(
                                                IdentifierName(equalityClass.Identifier)))
                                        .WithMembers(
                                            List(GenerateEqualityMethods(equalityClass, equalityPropertyNames))
                                        )))))
                .NormalizeWhitespace();
            return classExtension.GetText(Encoding.UTF8);
        }

        internal static IsPatternExpressionSyntax GetIsNullExpression(IdentifierNameSyntax objectToCheckName) 
            => IsPatternExpression(
                objectToCheckName,
            UnaryPattern(
                ConstantPattern(
                    LiteralExpression(
                        SyntaxKind.NullLiteralExpression))));
        
        private static BaseListSyntax GenerateEquatableInterface(TypeSyntax equalityClassIdentifier) =>
            BaseList(
                SingletonSeparatedList<BaseTypeSyntax>(
                    SimpleBaseType(GenericName(EquatableInterfaceIdentifier)
                        .WithTypeArgumentList(
                            TypeArgumentList(
                                SingletonSeparatedList(
                                    equalityClassIdentifier))))));

        private static IEnumerable<MemberDeclarationSyntax> GenerateEqualityMethods(BaseTypeDeclarationSyntax equalityClass, IEnumerable<IdentifierNameSyntax> equalityPropertyNames)
        {
            ParameterListSyntax parTypeObj = EqualsOperatorFactory.GetParametersTypeToObject(equalityClass);
            ParameterListSyntax parTypeType = EqualsOperatorFactory.GetParametersTypeToType(equalityClass);
            ParameterListSyntax parToType = EqualsMethodFactory.GetParametersTypeToObject(equalityClass);
            
            List<MemberDeclarationSyntax> equalsOperatorMethods = new List<MemberDeclarationSyntax>
            {
                EqualsOperatorFactory.GenerateEqualsEqualsTypeObjOperatorMethod(parTypeObj),
                EqualsOperatorFactory.GenerateNotEqualsTypeObjOperatorMethod(parTypeObj),
                EqualsOperatorFactory.GenerateEqualsEqualsTypeTypeOperatorMethod(parTypeType),
                EqualsOperatorFactory.GenerateNotEqualsTypeTypeOperatorMethod(parTypeType),
                
                EqualsMethodFactory.GenerateEqualsTypeMethod(parToType, equalityPropertyNames),
                EqualsMethodFactory.GenerateEqualsObjMethod(equalityClass),
            };

            return equalsOperatorMethods;
        }
    }
}
