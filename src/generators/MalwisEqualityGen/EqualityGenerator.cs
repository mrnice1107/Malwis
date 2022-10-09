﻿using System;
using EqualityGeneratorAttributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EqualityGenerator
{
    [Generator]
    public class EqualityGenerator : ISourceGenerator
    {
        private readonly GeneratorDebugHelper _debugger;

        public EqualityGenerator()
        {
            //GeneratorDebugHelper.AttachDebugger();
            _debugger = new GeneratorDebugHelper(@"E:\Projects\github\Malwis\test\generators\EqualityGenTest\", debugMode: GeneratorDebugHelper.DebuggingOutputMode.File);
        }

        public void Execute(GeneratorExecutionContext context)
        {

            // exec
            
            
            
            _debugger.Save();
        }

        public void Initialize(GeneratorInitializationContext context)
        {

            // init :D

            context.RegisterForSyntaxNotifications(() => new EqualitySyntaxReceiver(_debugger));
        }
    }

    internal class EqualitySyntaxReceiver : ISyntaxReceiver
    {
        private static readonly string EqualityAttributeName = nameof(EqualityAttribute);
        private static readonly string EqualityAttributeNameShort = EqualityAttributeName.Replace("Attribute", "");
        private readonly GeneratorDebugHelper _debugger;

        public EqualitySyntaxReceiver(GeneratorDebugHelper debugger) => _debugger = debugger;

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is ClassDeclarationSyntax classNode))
            {
                return;
            }

            bool hasEqualityAttribute = HasEqualityAttribute(classNode);

            _debugger.DebugLine(classNode);
            _debugger.DebugLine(hasEqualityAttribute.ToString());
        }

        private static bool HasEqualityAttribute(ClassDeclarationSyntax classNode)
        {
            if (classNode == null)
            {
                throw new ArgumentNullException(nameof(classNode));
            }

            foreach (AttributeListSyntax attributeList in classNode.AttributeLists)
            {
                if (AttributeListHasEqualityAttribute(attributeList))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool AttributeListHasEqualityAttribute(AttributeListSyntax attributeList)
        {
            foreach (AttributeSyntax attribute in attributeList.Attributes)
            {
                if (AttributeIsEqualityAttribute(attribute))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool AttributeIsEqualityAttribute(AttributeSyntax attribute) =>
            attribute.Name is IdentifierNameSyntax nameSyntax &&
            (nameSyntax.Identifier.Text.Equals(EqualityAttributeName) || nameSyntax.Identifier.Text.Equals(EqualityAttributeNameShort));
    }
    
}
