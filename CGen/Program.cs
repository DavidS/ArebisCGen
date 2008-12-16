using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using Arebis.CodeGenerator.Templated;
using System.Xml;
using System.IO;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.Remoting;

namespace Arebis.CommandLineTools.CodeGenerator
{
	[System.Diagnostics.DebuggerStepThrough]
	public class Program
    {
		public static NameValueCollection settings = new NameValueCollection();

		public static void Main(string[] args)
		{
			CommandLineInfo cmdlineinfo = new CommandLineInfo(args);

			// Show help if requested:
			if (cmdlineinfo.IsHelpRequested)
			{
				string help = Properties.Resources.HelpText;
				help = help.Replace("{executable}", Assembly.GetEntryAssembly().GetName().Name);
				help = help.Replace("{copyright}", ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright);
				Console.WriteLine(help);
				Environment.Exit(0);
			}

			// Load settings from file (if any):
			if (cmdlineinfo.Arguments.Count > 0)
			{
				try
				{
					XmlDocument settingsfile = new XmlDocument();
					settingsfile.Load(cmdlineinfo.Arguments[0]);
					foreach (XmlNode node in settingsfile.SelectNodes(@"/settings/add"))
					{
						settings.Add(node.Attributes["name"].Value, node.Attributes["value"].Value);
					}
				}
				catch (FileNotFoundException)
				{
					Console.Error.WriteLine("Settings file '{0}' not found.", cmdlineinfo.Arguments[0]);
					Environment.Exit(1);
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine("Error reading file '{0}': {1}", cmdlineinfo.Arguments[0], ex.Message);
					Environment.Exit(1);
				}
			}

			// Add settings from commandline:
			foreach (KeyValuePair<string, string> setting in cmdlineinfo.Settings)
			{
				settings.Remove(setting.Key);
				foreach (string value in setting.Value.Split(';'))
				{
					settings.Add(setting.Key, value);
				}
			}

			// Add exe directory to referencepath:
			settings.Add("referencepath", new FileInfo(Assembly.GetEntryAssembly().Location).Directory.FullName);

			// Check template is given:
			if (settings["template"] == null)
			{
				Console.WriteLine("No template given. Try /? for help.");
				Environment.Exit(0);
			}

			// Create a generator instance and execute the initial template:
			CodeGenerator.Generator generator = new CodeGenerator.Generator();
			generator.Settings = settings;
			generator.TemplateParameters = new object[0];
			int exitcode = generator.ExecuteTemplate();

			// Exit generator:
			Environment.Exit(exitcode);
		}
    }
}
