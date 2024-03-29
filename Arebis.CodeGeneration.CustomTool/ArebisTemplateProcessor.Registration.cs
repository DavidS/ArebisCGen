﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.VisualStudio.TextTemplating.VSHost;
using Microsoft.Win32;

namespace Arebis.CodeGeneration.CustomTool
{
    public partial class ArebisTemplateProcessor : BaseCodeGeneratorWithSite
    {
        #region Registration

        // You have to make sure that the value of this field (CustomToolGuid) is exactly 
        // the same as the value of the Guid attribure (at the top of the class)
        private static Guid CustomToolGuid =
            new Guid("{216D97DB-F5B3-4b14-B3A9-F9C9CEA95339}");

        private static Guid CSharpCategory =
            new Guid("{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}");

        private static Guid VBCategory =
            new Guid("{164B10B9-B200-11D0-8C61-00A0C91E29D5}");

        private const string CustomToolName = "Arebis Template Processor";

        private const string CustomToolDescription = "Generates the actual Generate() method behind an Arebis template";

        private const string KeyFormat
            = @"SOFTWARE\Microsoft\VisualStudio\{0}\Generators\{1}\{2}";

        protected static void Register(Version vsVersion, Guid categoryGuid)
        {
            string subKey = String.Format(KeyFormat,
                vsVersion, categoryGuid.ToString("B"), CustomToolName);

            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(subKey))
            {
                key.SetValue("", CustomToolDescription);
                key.SetValue("CLSID", CustomToolGuid.ToString("B"));
                key.SetValue("GeneratesDesignTimeSource", 1);
            }
        }

        protected static void Unregister(Version vsVersion, Guid categoryGuid)
        {
            string subKey = String.Format(KeyFormat,
                vsVersion, categoryGuid.ToString("B"), CustomToolName);

            Registry.LocalMachine.DeleteSubKey(subKey, false);
        }

        // Versions:
        // 9.0 = VS.NET 2008
        // 10.0 = VS.NET 2010
        // 11.0 = VS.NET 2012
        // 12.0 = VS.NET 2013
        [ComRegisterFunction]
        public static void RegisterClass(Type t)
        {
            foreach (var version in new[] { new Version(9, 0), new Version(10, 0), new Version(11, 0), new Version(12, 0) })
            {
                Register(version, CSharpCategory);
                Register(version, VBCategory);
            }
        }

        [ComUnregisterFunction]
        public static void UnregisterClass(Type t)
        {
            foreach (var version in new[] { new Version(9, 0), new Version(10, 0), new Version(11, 0), new Version(12, 0) })
            {
                Unregister(version, CSharpCategory);
                Unregister(version, VBCategory);
            }
        }

        #endregion
    }
}
