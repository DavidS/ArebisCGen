using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Arebis.CodeGenerator.Templated;

using Microsoft.VisualStudio.TextTemplating.VSHost;

namespace Arebis.CodeGeneration.CustomTool
{

    /// <summary>
    /// A Visual Studio "Custom Tool" for generating the Generate() method. 
    /// Based on a blog post[1] by Aviad Ezra.
    /// </summary>
    /// [1] http://aviadezra.blogspot.com/2008_11_01_archive.html

    // You have to make sure that the value of this attribute (Guid) 
    // is exactly the same as the value of the field 'CustomToolGuid' 
    // (in the registration region)
    [Guid("216D97DB-F5B3-4b14-B3A9-F9C9CEA95339")]
    [ComVisible(true)]
    public partial class ArebisTemplateProcessor : BaseCodeGeneratorWithSite
    {

        protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
        {
            // Default to T3 syntax:
            new Arebis.CodeGenerator.Templated.Syntax.T3Syntax().Setup(null);
            
            // Build code string
            var builder = new CSCodeBuilder();
            var info = new TemplateInfo(inputFileName, new StudioGenerationHost());
            builder.TemplateInfo = info;
            info.Parse();
            string code = builder.CreateCode();

            // encode and return result as utf-8 bytes
            return Encoding.UTF8.GetBytes(code);
        }

        public override string GetDefaultExtension()
        {
            return ".Designer.cs";
        }

    }


}
