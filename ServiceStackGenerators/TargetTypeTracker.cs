using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace ServiceStackGenerators
{
    public class TargetTypeTracker : ISyntaxContextReceiver
    {
        public int ServiceClassesFound { get; set; }
        public int ProgramClassesFound { get; set; }
        public int AuthAttributeFound { get; set; }
        public int AuthFeatureFound { get; set; }
        public int ModularStartupFound { get; set; }
        public int AppHostsFound { get; set; }


        public string Namespace { get; set; }
        public string AssemblyRefServiceName { get; set; }

        public bool NeedsAppHost => AppHostsFound == 0;

        public bool NeedsEntryPoint => ProgramClassesFound == 0;

        public bool NeedsAuthConfig => AuthFeatureFound == 0 && AuthAttributeFound > 0;

        public bool NeedsModularStartup => ModularStartupFound == 0;

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is TypeDeclarationSyntax typeDecSyntax)
            {
                if(Namespace == null)
                {
                    Namespace = typeDecSyntax.GetNamespaceName();
                }

                if(typeDecSyntax.IsServiceClass())
                {
                    ServiceClassesFound++;
                    AssemblyRefServiceName = typeDecSyntax.Identifier.ValueText;
                    if(typeDecSyntax.IsDecoratedWithAttribute("Authenticate"))
                    {
                        AuthAttributeFound++;
                    }
                }

                if(typeDecSyntax.IsEntryPoint())
                {
                    ProgramClassesFound++;
                    this.Namespace = typeDecSyntax.GetNamespaceName();
                }

                if(typeDecSyntax.IsAuthFeatureRegistered())
                {
                    AuthFeatureFound++;
                }

                if(typeDecSyntax.HasDeclaredBaseClass("ModularStartup"))
                {
                    ModularStartupFound++;
                }

                if(typeDecSyntax.HasDeclaredBaseClass("AppHostBase"))
                {
                    AppHostsFound++;
                }
            }
                
        }
    }
}