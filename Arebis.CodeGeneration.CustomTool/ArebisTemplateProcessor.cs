using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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
            return Encoding.UTF8.GetBytes("// generated code\n");
        }

        public override string GetDefaultExtension()
        {
            return ".cs";
        }

    }


}
