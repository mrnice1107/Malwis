using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace EqualityGenerator.Factories
{
    internal static class EqualsMethodFactory
    {
        static EqualsMethodFactory()
        {
            ObjNameToken = IdentifierName("obj");
            OtherNameToken = IdentifierName("other");
            CastedNameToken = IdentifierName("casted");

            ParametersToObject = ParameterList(
                SingletonSeparatedList(
                    Parameter(ObjNameToken.Identifier)
                        .WithAttributeLists(EqualsFactory.NotNullAttributeList)
                        .WithType(
                            PredefinedType(
                                Token(SyntaxKind.ObjectKeyword)))));
        }

        private static readonly IdentifierNameSyntax ObjNameToken;
        private static readonly IdentifierNameSyntax CastedNameToken;
        private static readonly IdentifierNameSyntax OtherNameToken;
        
        private static readonly ParameterListSyntax ParametersToObject;
        
        internal static MemberDeclarationSyntax GenerateEqualsObjMethod(BaseTypeDeclarationSyntax equalityClassIdentifier) =>
            MethodDeclaration(
                    EqualsFactory.BooleanType,
                    EqualsFactory.EqualsMethodName.Identifier)
                .WithModifiers(EqualsFactory.PublicOverrideModifier)
                .WithParameterList(ParametersToObject)
                .WithExpressionBody(
                    ArrowExpressionClause(
                        BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            IsPatternExpression(
                                ObjNameToken,
                                DeclarationPattern(
                                    IdentifierName(equalityClassIdentifier.Identifier),
                                    SingleVariableDesignation(
                                        CastedNameToken.Identifier))),
                            InvocationExpression(
                                    EqualsFactory.EqualsMethodName)
                                .WithArgumentList(
                                    ArgumentList(
                                        SingletonSeparatedList(
                                            Argument(
                                                CastedNameToken)))))))
                .WithSemicolonToken(EqualsFactory.SemicolonToken);

        internal static ParameterListSyntax GetParametersTypeToObject(BaseTypeDeclarationSyntax equalityClass) =>
            ParameterList(
                SingletonSeparatedList(
                    Parameter(OtherNameToken.Identifier)
                        .WithAttributeLists(EqualsFactory.NotNullAttributeList)
                        .WithType(IdentifierName(equalityClass.Identifier))));
        
        internal static MemberDeclarationSyntax GenerateEqualsTypeMethod(
            ParameterListSyntax parametersTypeToType,
            IEnumerable<IdentifierNameSyntax> equalityPropertyNames) =>
            MethodDeclaration(
                    EqualsFactory.BooleanType,
                    EqualsFactory.EqualsMethodName.Identifier)
                .WithModifiers(EqualsFactory.PublicModifier)
                .WithParameterList(parametersTypeToType)
                .WithExpressionBody(ArrowExpressionClause(GenerateBinaryExpressions(equalityPropertyNames)))
                .WithSemicolonToken(
                    Token(SyntaxKind.SemicolonToken));

        private static ExpressionSyntax GenerateBinaryExpressions(IEnumerable<IdentifierNameSyntax> equalityPropertyNames)
        {
            ExpressionSyntax lastExpression = EqualsFactory.GetIsNullExpression(OtherNameToken);
            
            foreach (IdentifierNameSyntax propertySyntax in equalityPropertyNames)
            {
                BinaryExpressionSyntax rightEqualsExpression = BinaryExpression(
                    SyntaxKind.EqualsExpression,
                    propertySyntax,
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        OtherNameToken,
                        propertySyntax));
                
                lastExpression = BinaryExpression(SyntaxKind.LogicalAndExpression, lastExpression, rightEqualsExpression);
            }
            
            return lastExpression;
        }
    }
}
