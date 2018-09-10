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
        private const string _razorTemplateBaseFolder = "EmailTemplates";

        public static Assembly CompileTemplate(string razorTemplateFileName, string dynamicAssemblyNamespace, string dynamicDllName)
        {


            PortableExecutableReference referenceToCallingAssembly = MetadataReference.CreateFromFile(Assembly.GetCallingAssembly().Location); // we will need to know what piece of code has called *this* code, so we need to make a note of this at first opportunity (this will be later added to a dynmic compilation item)
            RazorProjectEngine razorProjectEngine;
            RazorProjectFileSystem razorProjectFileSystem = InitialiseTemplateProject(dynamicAssemblyNamespace, out razorProjectEngine);
            RazorProjectItem razorProjectItem = GetRazorProjectItem(razorProjectFileSystem, razorTemplateFileName);
            SyntaxTree cSharpSyntaxTree = GenerateSyntaxTree(razorProjectItem, razorProjectEngine);
            CSharpCompilation cSharpCompilation = CompileDynamicAssembly(dynamicDllName, cSharpSyntaxTree, referenceToCallingAssembly);
            return StreamAssemblyInMemory(cSharpCompilation);
        }



        private static RazorProjectFileSystem InitialiseTemplateProject(string dynamicAssemblyNamespace, out RazorProjectEngine razorProjectEngine)
        {
            // points to the local path
            RazorProjectFileSystem razorProjectFileSystem = RazorProjectFileSystem.Create(".");

            // customize the default engine a little bit
            razorProjectEngine = RazorProjectEngine.Create(RazorConfiguration.Default, razorProjectFileSystem, (builder) =>
            {
                InheritsDirective.Register(builder);

                // define a namespace for the Template class
                builder.SetNamespace(dynamicAssemblyNamespace);
            });

            return razorProjectFileSystem;
        }



        private static RazorProjectItem GetRazorProjectItem(RazorProjectFileSystem razorProjectFileSystem, string razorTemplateFileName)
        {
            // get a razor-templated file.   You could just use a ".txt" file here, but recommend leaving it as ".cshtml" to make it more apparent what the file should contain (i.e. razor-infused markup)
            return razorProjectFileSystem.GetItem($"{_razorTemplateBaseFolder}/{razorTemplateFileName}");
        }



        private static  SyntaxTree GenerateSyntaxTree(RazorProjectItem razorProjectItem, RazorProjectEngine razorProjectEngine)
        {
            RazorCodeDocument razorCodeDocument = razorProjectEngine.Process(razorProjectItem);
            RazorCSharpDocument razorCSharpDocument = razorCodeDocument.GetCSharpDocument();
            return CSharpSyntaxTree.ParseText(razorCSharpDocument.GeneratedCode);
        }



        private static CSharpCompilation CompileDynamicAssembly(string dynamicDllName, SyntaxTree cSharpSyntaxTree, PortableExecutableReference referenceToCallingAssembly)
        {
            return CSharpCompilation.Create(dynamicDllName, new[] { cSharpSyntaxTree },
                new[]
                {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(typeof(RazorCompiledItemAttribute).Assembly.Location),
                    referenceToCallingAssembly, 
                    MetadataReference.CreateFromFile(Assembly.GetExecutingAssembly().Location), // get a reference to "this" code
                    //MetadataReference.CreateFromFile(typeof(RazorEngineBaseTemplate<TTemplateViewModel>).Assembly.Location), // this commented-out line is an alternative to the above, so you can see an example of the syntax that you might use to bring in other pieces of code    
                    MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location),"System.Runtime.dll")),
                    MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location),"System.Collections.dll")),
                    MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location),"netstandard.dll")),
                 },
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }




        private static Assembly StreamAssemblyInMemory(CSharpCompilation cSharpCompilation)
        {
            Assembly dynamicAssembly;

            using (var memoryStream = new MemoryStream())
            {
                var result = cSharpCompilation.Emit(memoryStream);
                if (!result.Success)
                {
                    string errorMessage = (string.Join(Environment.NewLine, result.Diagnostics));
                    throw new Exception(errorMessage);
                }
                else
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    dynamicAssembly = Assembly.Load(memoryStream.ToArray());
                }
            }

            return dynamicAssembly;
        }
    }
}
