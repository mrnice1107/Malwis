using System.Collections.Generic;
using EqualityGenerator.Extensions;
using EqualityGeneratorAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EqualityGenerator
{
    internal class EqualitySyntaxReceiver : ISyntaxReceiver
    {
        internal List<ClassDeclarationSyntax> EqualityClasses { get; }

        private readonly GeneratorDebugHelper _debugger;

        public EqualitySyntaxReceiver(GeneratorDebugHelper debugger)
        {
            EqualityClasses = new List<ClassDeclarationSyntax>();
            _debugger = debugger;
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is ClassDeclarationSyntax classNode))
            {
                return;
            }

            _debugger.DebugLine($"Class found: {syntaxNode.ToFullString()}", "EqualitySyntaxReceiver");

            if (!classNode.HasAttribute(nameof(AutoEqualityAttribute), nameof(AutoEqualityAttribute).Replace("Attribute","")))
            {
                return;
            }

            _debugger.DebugLine("has equality attribute");
            EqualityClasses.Add(classNode);
        }
    }
}
