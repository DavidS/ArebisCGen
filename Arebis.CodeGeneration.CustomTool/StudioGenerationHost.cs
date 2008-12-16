using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Arebis.CodeGeneration.CustomTool
{
    public class StudioGenerationHost : IGenerationHost
    {
        #region IGenerationHost Members

        public void CallTemplate(string templatefile, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void CallTemplateToFile(string templatefile, string outputfile, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public NameValueCollection Settings
        {
            get { throw new NotImplementedException(); }
        }

        public void WriteOutput(string str)
        {
            throw new NotImplementedException();
        }

        public string NewLineString
        {
            get { return "\n"; }
        }

        public void Log(string fmt, params object[] args)
        {
        }

        public void WriteFile(string filename, string content)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
