using System;
using System.Reflection;
using EmailTemplateDemo.EmailTemplates;
using RazorEngineTemplating;


namespace EmailTemplateDemo
{
    class Program
    {
        static void Main(string[] args)
        {


            // Setup an MVC-style ViewModel
            var templateModel = new SampleEmailTemplateModel {
                EmailTagline = "Markup Generated With the Razor Engine !"
            };

            templateModel.ListCollectionItems.Add(new SampleEmailTemplateModelCollectionItem { CollectionItemDescription = "This is the first item (we can show a collection of data)" });
            templateModel.ListCollectionItems.Add(new SampleEmailTemplateModelCollectionItem { CollectionItemDescription = "This is the second item (so our demo is demonstrating iteration)" });

            // define some comfiguration items used by the dynamic assembly
            string dynamicAssemblyNamespace = "EmailTemplateDynamicAssembly";
            string dynamicDllName = "EmailTemplateDynamicAssemblyDllName";

            // dynamically compile a template assembly
            Assembly templateAssembly = RazorEngineDynamicCompilerHelper<SampleEmailTemplateModel>.CompileTemplate("SampleEmailTemplate.cshtml", dynamicAssemblyNamespace, dynamicDllName);

            // use the compiled assembly, merging with a populated ViewModel and emitting to console.
            Console.Write(RazorEngineTemplateHelper<SampleEmailTemplateModel>.MergeViewModelReturnMarkup(templateModel, templateAssembly, dynamicAssemblyNamespace));

            Console.ReadKey();
        }
    }
}
