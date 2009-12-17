using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;

namespace Arebis.CodeGeneration.MsBuild
{
    internal class MsBuildHost
        : IGenerationHost
    {
        internal MsBuildHost(TaskLoggingHelper log)
        {
            this.Logger = log;
        }

        private readonly TaskLoggingHelper Logger;

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
            get { return Environment.NewLine; }
        }

        public void Log(string fmt, params object[] args)
        {
            Logger.LogMessage(fmt, args);
        }

        public void WriteFile(string filename, string content)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
