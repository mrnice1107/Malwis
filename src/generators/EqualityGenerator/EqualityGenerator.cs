using System;
using System.Collections.Generic;
using System.Text;
using EqualityGenerator.Extensions;
using EqualityGeneratorAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace EqualityGenerator
{
    [Generator]
    public class EqualityGenerator : ISourceGenerator
    {
        private readonly GeneratorDebugHelper _debugger;
        private const string LocalDir = @"E:\Projects\github\Malwis\test\generators\EqualityGenTest\";

        public EqualityGenerator()
        {
            //GeneratorDebugHelper.AttachDebugger();
            _debugger = new GeneratorDebugHelper(LocalDir, debugMode: GeneratorDebugHelper.DebuggingOutputMode.File);

            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                _debugger.DebugLine(args.Name);
                _debugger.DebugLine(args.RequestingAssembly.FullName);
                
                return null;
            };
        }

        public void Execute(GeneratorExecutionContext context)
        {
            _debugger.DebugLine("Starting Processing", "Executing");

            ISyntaxReceiver contextSyntaxReceiver = context.SyntaxReceiver;
            if (!(contextSyntaxReceiver is EqualitySyntaxReceiver equalityReceiver))
            {
                return;
            }

            foreach (ClassDeclarationSyntax equalityClass in equalityReceiver.EqualityClasses)
            {
                SourceText generatedEqualityExtension = GenerateEqualityExtension(equalityClass);
                string fileName = $"{equalityClass.Identifier.Text}_auto_equality.g.cs";

                context.AddSource(fileName, generatedEqualityExtension);
                _debugger.DebugLine(generatedEqualityExtension.ToString(), $"Generated Source: {fileName}");
            }

            _debugger.DebugLine("Processing Finished", "Executing");
            _debugger.Save();
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            _debugger.DebugLine("Initializing");
            
            context.RegisterForSyntaxNotifications(() => new EqualitySyntaxReceiver(_debugger));
            
            _debugger.DebugLine("Done Initializing");
        }
        
        private static SourceText GenerateEqualityExtension(TypeDeclarationSyntax equalityClass)
        {
            FileScopedNamespaceDeclarationSyntax namespaceSyntax = equalityClass.GetParentOfType<FileScopedNamespaceDeclarationSyntax>();

            StringBuilder builder = new StringBuilder();
            List<PropertyDeclarationSyntax> equalityProperties = GetEqualityProperties(equalityClass);
            
            CompilationUnitSyntax classExtension = CompilationUnit()
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
                                        .WithModifiers(
                                            equalityClass.Modifiers
                                            )
                                        .WithMembers(
                                            SingletonList<MemberDeclarationSyntax>(
                                                TempCreateMethod(builder.ToString())
                                                )
                                        ))
                                )))
                .NormalizeWhitespace();
            return classExtension.GetText(Encoding.UTF8);
        }

        private static MethodDeclarationSyntax TempCreateMethod(string comment)
        {
            return MethodDeclaration(
                    PredefinedType(
                        Token(SyntaxKind.VoidKeyword)),
                    Identifier("Print"))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PrivateKeyword)))
                .WithParameterList(
                    ParameterList(
                        SingletonSeparatedList(
                            Parameter(
                                    Identifier("input"))
                                .WithType(
                                    PredefinedType(
                                        Token(SyntaxKind.StringKeyword))))))
                .WithBody(
                    Block()
                        .WithCloseBraceToken(
                            Token(
                                TriviaList(
                                    Comment($"// {comment}")),
                                SyntaxKind.CloseBraceToken,
                                TriviaList())));
        }

        private static List<PropertyDeclarationSyntax> GetEqualityProperties(TypeDeclarationSyntax equalityClass)
        {
            List<PropertyDeclarationSyntax> list = new List<PropertyDeclarationSyntax>();
            
            foreach (MemberDeclarationSyntax member in equalityClass.Members)
            {
                if (MemberIsPropertyWithoutIgnoreAttribute(member))
                {
                    list.Add(member as PropertyDeclarationSyntax);
                }
            }

            return list;
        }

        private static bool MemberIsPropertyWithoutIgnoreAttribute(MemberDeclarationSyntax member) =>
            member is PropertyDeclarationSyntax &&
            !member.HasAttribute(nameof(EqualityIgnoreAttribute), nameof(EqualityIgnoreAttribute).Replace("Attribute",""));
    }
}
