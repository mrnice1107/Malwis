using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace EqualityGenerator.Factories
{
    internal static class EqualsOperatorFactory
    {
        static EqualsOperatorFactory()
        {
            LeftIdentifierName = IdentifierName("left");
            RightIdentifierName = IdentifierName("right");
        }

        private static readonly IdentifierNameSyntax LeftIdentifierName;
        private static readonly IdentifierNameSyntax RightIdentifierName;
        internal static ParameterListSyntax GetParametersTypeToObject(BaseTypeDeclarationSyntax equalityClass) => 
            ParameterList(SeparatedList<ParameterSyntax>(
            new SyntaxNodeOrToken[]
            {
                Parameter(LeftIdentifierName.Identifier)
                    .WithAttributeLists(EqualsFactory.NotNullAttributeList)
                    .WithType(IdentifierName(equalityClass.Identifier)),
                Token(SyntaxKind.CommaToken), Parameter(RightIdentifierName.Identifier)
                    .WithAttributeLists(EqualsFactory.NotNullAttributeList)
                    .WithType(
                        PredefinedType(
                            Token(SyntaxKind.ObjectKeyword)))
            }));
        
        internal static ParameterListSyntax GetParametersTypeToType(BaseTypeDeclarationSyntax equalityClass) => 
            ParameterList(
            SeparatedList<ParameterSyntax>(
                new SyntaxNodeOrToken[]
                {
                    Parameter(LeftIdentifierName.Identifier)
                        .WithAttributeLists(EqualsFactory.NotNullAttributeList)
                        .WithType(IdentifierName(equalityClass.Identifier)),
                    Token(SyntaxKind.CommaToken), Parameter(RightIdentifierName.Identifier)
                        .WithAttributeLists(EqualsFactory.NotNullAttributeList)
                        .WithType(IdentifierName(equalityClass.Identifier)),
                }));

        internal static OperatorDeclarationSyntax GenerateNotEqualsTypeTypeOperatorMethod(ParameterListSyntax parametersTypeToType) =>
            OperatorDeclaration(
                    EqualsFactory.BooleanType,
                    Token(SyntaxKind.ExclamationEqualsToken))
                .WithModifiers(EqualsFactory.PublicStaticModifier)
                .WithParameterList(parametersTypeToType)
                .WithExpressionBody(
                    ArrowExpressionClause(
                        PrefixUnaryExpression(
                            SyntaxKind.LogicalNotExpression,
                            ParenthesizedExpression(
                                BinaryExpression(
                                    SyntaxKind.EqualsExpression,
                                    LeftIdentifierName,
                                    RightIdentifierName)))))
                .WithSemicolonToken(EqualsFactory.SemicolonToken);

        internal static OperatorDeclarationSyntax GenerateEqualsEqualsTypeTypeOperatorMethod(ParameterListSyntax parametersTypeToType) =>
            OperatorDeclaration(
                    EqualsFactory.BooleanType,
                    Token(SyntaxKind.EqualsEqualsToken))
                .WithModifiers(EqualsFactory.PublicStaticModifier)
                .WithParameterList(
                    parametersTypeToType)
                .WithExpressionBody(
                    ArrowExpressionClause(
                        BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            IsPatternExpression(
                                LeftIdentifierName,
                                UnaryPattern(
                                    ConstantPattern(
                                        LiteralExpression(
                                            SyntaxKind.NullLiteralExpression)))),
                            InvocationExpression(
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        LeftIdentifierName,
                                        EqualsFactory.EqualsMethodName))
                                .WithArgumentList(
                                    ArgumentList(
                                        SingletonSeparatedList(
                                            Argument(
                                                RightIdentifierName)))))))
                .WithSemicolonToken(EqualsFactory.SemicolonToken);

        internal static OperatorDeclarationSyntax GenerateNotEqualsTypeObjOperatorMethod(ParameterListSyntax parametersTypeToObject) =>
            OperatorDeclaration(
                    EqualsFactory.BooleanType,
                    Token(SyntaxKind.ExclamationEqualsToken))
                .WithModifiers(EqualsFactory.PublicStaticModifier)
                .WithParameterList(parametersTypeToObject)
                .WithExpressionBody(
                    ArrowExpressionClause(
                        PrefixUnaryExpression(
                            SyntaxKind.LogicalNotExpression,
                            ParenthesizedExpression(
                                BinaryExpression(
                                    SyntaxKind.EqualsExpression,
                                    LeftIdentifierName,
                                    RightIdentifierName)))))
                .WithSemicolonToken(EqualsFactory.SemicolonToken);

        internal static OperatorDeclarationSyntax GenerateEqualsEqualsTypeObjOperatorMethod(ParameterListSyntax parametersTypeToObject) =>
            OperatorDeclaration(
                    EqualsFactory.BooleanType,
                    Token(SyntaxKind.EqualsEqualsToken))
                .WithModifiers(EqualsFactory.PublicStaticModifier)
                .WithParameterList(parametersTypeToObject)
                .WithExpressionBody(
                    ArrowExpressionClause(
                        BinaryExpression(
                            SyntaxKind.LogicalAndExpression,
                            IsPatternExpression(
                                LeftIdentifierName,
                                UnaryPattern(
                                    ConstantPattern(
                                        LiteralExpression(
                                            SyntaxKind.NullLiteralExpression)))),
                            InvocationExpression(
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        LeftIdentifierName,
                                        EqualsFactory.EqualsMethodName))
                                .WithArgumentList(
                                    ArgumentList(
                                        SingletonSeparatedList(
                                            Argument(
                                                RightIdentifierName)))))))
                .WithSemicolonToken(EqualsFactory.SemicolonToken);
    }
}
