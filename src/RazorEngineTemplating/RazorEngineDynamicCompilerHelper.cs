using System;
using System.IO;
using System.Reflection;

using Microsoft.AspNetCore.Razor.Hosting;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace RazorEngineTemplating
{
    public static class RazorEngineDynamicCompilerHelper<TTemplateViewModel>
    {
        //TODO Add attributes

        public static Assembly CompileTemplate(string razorTemplateFileName, string dynamicAssemblyNamespace, string dynamicDllName)
        {
            //TODO Add method code
        }



        private static RazorProjectFileSystem InitialiseTemplateProject(string dynamicAssemblyNamespace, out RazorProjectEngine razorProjectEngine)
        {
            //TODO Add method code
        }



        private static RazorProjectItem GetRazorProjectItem(RazorProjectFileSystem razorProjectFileSystem, string razorTemplateFileName)
        {
            //TODO Add method code
        }



        private static  SyntaxTree GenerateSyntaxTree(RazorProjectItem razorProjectItem, RazorProjectEngine razorProjectEngine)
        {
            //TODO Add method code
        }



        private static CSharpCompilation CompileDynamicAssembly(string dynamicDllName, SyntaxTree cSharpSyntaxTree, PortableExecutableReference referenceToCallingAssembly)
        {
            //TODO Add method code
        }




        private static Assembly StreamAssemblyInMemory(CSharpCompilation cSharpCompilation)
        {
            //TODO Add method code
        }
    }
}
