using System;
using EqualityGenerator.Factories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EqualityGenerator
{
    [Generator]
    public class EqualityGenerator : ISourceGenerator
    {
        private readonly GeneratorDebugHelper _debugger;
        private const string LocalDir = @"E:\Projects\github\Malwis\test\generators\EqualityGenTest\";

        public EqualityGenerator()
        {
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

#if DEBUG
            try
            {
#endif
                ISyntaxReceiver contextSyntaxReceiver = context.SyntaxReceiver;
                if (!(contextSyntaxReceiver is EqualitySyntaxReceiver equalityReceiver))
                {
                    return;
                }

                foreach (ClassDeclarationSyntax equalityClass in equalityReceiver.EqualityClasses)
                {
                    SourceText generatedEqualityExtension = EqualsFactory.GenerateEqualsSourceCode(equalityClass);
                    string fileName = $"{equalityClass.Identifier.Text}_auto_equality.g.cs";

                    context.AddSource(fileName, generatedEqualityExtension);
                    _debugger.DebugLine(generatedEqualityExtension.ToString(), $"Generated Source: {fileName}");
                }

                _debugger.DebugLine("Processing Finished", "Executing");
#if DEBUG
            }
            catch (Exception e)
            {
                _debugger.DebugLine(e.Message);
                throw;
            }
            finally
            {
                _debugger.Save();
            }
#endif
        }

        public void Initialize(GeneratorInitializationContext context)
        { 
            _debugger.DebugLine("Initializing");
            
            context.RegisterForSyntaxNotifications(() => new EqualitySyntaxReceiver(_debugger));
            
            _debugger.DebugLine("Done Initializing");
        }
    }
}
