using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp;

namespace ServiceStackGenerators
{
    internal static class SourceGeneratorExtensions
    {
        internal static string GetNamespaceName(this TypeDeclarationSyntax typeDeclarationSyntax)
        {
            string namespaceName = null;
            var parent = typeDeclarationSyntax.Parent;
            while (!parent.IsKind(SyntaxKind.NamespaceDeclaration))
            {
                parent = parent.Parent;
            }
            foreach (var token in parent.DescendantTokens())
            {
                if (token.IsKind(SyntaxKind.IdentifierToken))
                {
                    namespaceName = token.ValueText;
                    break;
                }
            }
            return namespaceName;
        }

        internal static bool IsDecoratedWithAttribute(
        this TypeDeclarationSyntax cdecl, string attributeName) =>
        cdecl.AttributeLists
            .SelectMany(x => x.Attributes)
            .Any(x => x.Name.ToString() == attributeName);

        internal static bool IsAuthFeatureRegistered(this TypeDeclarationSyntax typeDecSyntax)
        {
            if(!typeDecSyntax.IsAppHostClass() && !typeDecSyntax.HasDeclaredInterface("IConfigureAppHost"))
            {
                return false;
            }
            foreach (var member in typeDecSyntax.Members)
            {
                if (!member.IsKind(SyntaxKind.MethodDeclaration))
                {
                    continue;
                }

                var methodSyn = (MethodDeclarationSyntax)member;

                if (methodSyn.Identifier.ValueText == "Configure" &&
                    methodSyn.ParameterList.Parameters.Any(x =>
                    x.Type.ToString() == "Container" ||
                    x.Type.ToString() == "IAppHost"))
                {
                    // HACK, could walk statements to ensure `Plugins.Add` was taking an AuthFeature?
                    return methodSyn.Body.ToString().Contains("new AuthFeature");
                }
            }

            return false;
        }

        internal static bool HasDeclaredBaseClass(this TypeDeclarationSyntax typeDecSyntax, string baseClassName)
        {
            var hasBaseClassName = false;
            if (typeDecSyntax.BaseList != null)
            {
                var hasBaseClass = typeDecSyntax.BaseList.Types.Count > 0;
                if (hasBaseClass)
                {
                    foreach (var type in typeDecSyntax.BaseList.Types)
                    {
                        hasBaseClassName = type.ToString() == baseClassName;
                        if (hasBaseClassName)
                            break;
                    }
                }
            }
            return hasBaseClassName;
        }

        internal static bool HasDeclaredInterface(this TypeDeclarationSyntax typeDecSyntax, string interfaceName)
        {
            var hasBaseClassName = false;
            if (typeDecSyntax.BaseList != null)
            {
                var hasBaseClass = typeDecSyntax.BaseList.Types.Count > 0;
                if (hasBaseClass)
                {
                    foreach (var type in typeDecSyntax.BaseList.Types)
                    {
                        hasBaseClassName = type.ToString() == interfaceName;
                        if (hasBaseClassName)
                            break;
                    }
                }
            }
            return hasBaseClassName;
        }

        internal static bool IsAppHostClass(this TypeDeclarationSyntax typeDecSyntax)
        {
            return typeDecSyntax.HasDeclaredBaseClass("AppHostBase");
        }

        internal static bool IsServiceClass(this TypeDeclarationSyntax cdecl)
        {
            return cdecl.HasDeclaredBaseClass("Service");
        }

        internal static bool IsEntryPoint(this TypeDeclarationSyntax cdecl)
        {
            var fullName = cdecl.ToFullString();
            if (!(cdecl.IsKind(SyntaxKind.ClassDeclaration) &&
                cdecl.Identifier.ValueText == "Program"))
            {
                return false;
            }

            foreach (var member in cdecl.Members)
            {
                if (!member.IsKind(SyntaxKind.MethodDeclaration))
                {
                    continue;
                }
                foreach (var token in member.DescendantTokens())
                {
                    if (!token.IsKind(SyntaxKind.IdentifierToken))
                    {
                        continue;
                    }
                    if (token.ValueText == "Main")
                        return true;
                }
            }

            return false;
        }
    }
}
