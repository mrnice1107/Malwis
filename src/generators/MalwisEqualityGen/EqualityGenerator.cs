using Microsoft.CodeAnalysis;

namespace EqualityGenerator
{
    [Generator]
    public class EqualityGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {

            // exec

        }

        public void Initialize(GeneratorInitializationContext context)
        {

#if DEBUG
            GeneratorDebugHelper.AttachDebugger();
#endif

            // init :D

        }
    }
}
