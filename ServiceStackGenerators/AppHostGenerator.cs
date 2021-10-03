using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;

namespace ServiceStackGenerators
{
    [Generator]
    public class AppHostGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                //Debugger.Launch();
            }
#endif 
            context.RegisterForSyntaxNotifications(() => new TargetTypeTracker());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var targetTypeTracker = context.SyntaxContextReceiver as TargetTypeTracker;
            if (targetTypeTracker.NeedsEntryPoint)
            {
                var code1 = string.Format(CodeTemplates.EntryPointTemplate, targetTypeTracker.Namespace);
                    context.AddSource(
                    "servicestack.apphost.entrypoint",
                    SourceText.From(code1,
                    Encoding.UTF8));
            }
            if (targetTypeTracker.NeedsAppHost)
            {
                context.AddSource(
                    "servicestack.apphost.host",
                    SourceText.From(string.Format(CodeTemplates.AppHostCodeTemplate, targetTypeTracker.Namespace,
                    "Lala",
                    targetTypeTracker.AssemblyRefServiceName),
                    Encoding.UTF8));
            }

            if (targetTypeTracker.NeedsModularStartup)
            {
                context.AddSource(
                    "servicestack.apphost.startup",
                    SourceText.From(string.Format(CodeTemplates.StartupTemplate, targetTypeTracker.Namespace),
                    Encoding.UTF8));
            }

            if (targetTypeTracker.NeedsAuthConfig)
            {
                context.AddSource(
                    "servicestack.apphost.authconfig",
                    SourceText.From(string.Format(CodeTemplates.AuthTemplate, targetTypeTracker.Namespace),
                    Encoding.UTF8));
            }

        }
    }
}
