using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Arebis.CodeGenerator.Templated
{
    public interface ICodeBuilder : IDisposable
    {
        ITemplateInfo TemplateInfo { get; set; }
        /// <summary>
        /// Builds the source code in-memory.
        /// Use Compile() if you're only interested in the generated Type.
        /// </summary>
        string CreateCode();
        bool Compile();
        Type CompiledType { get; }
        IList<CompilerError> CompilerErrors { get; }
    }
}
