using System;
using System.Collections.Generic;
using System.Text;

namespace Arebis.CommandLineTools.CodeGenerator
{
	public class CommandLineInfo
	{
		private List<string> arguments;
		private Dictionary<string, string> settings;
		private bool isHelpRequested;

		public CommandLineInfo(string[] args)
		{
			this.arguments = new List<string>();
			this.settings = new Dictionary<string, string>();

			string setting = null;
			foreach (string item in args)
			{
				if (item.StartsWith("/"))
				{
					if (item.Equals("/?") || item.ToLower().Equals("/help"))
					{
						this.isHelpRequested = true;
					}
					else
					{
						setting = item.Substring(1);
					}
				}
				else
				{
					if (setting == null)
					{
						this.arguments.Add(item);
					}
					else
					{
						this.settings[setting] = item;
						setting = null;
					}
				}
			}
		}

		public bool IsHelpRequested
		{
			get { return isHelpRequested; }
			set { isHelpRequested = value; }
		}

		public List<string> Arguments
		{
			get { return arguments; }
			set { arguments = value; }
		}

		public Dictionary<string, string> Settings
		{
			get { return settings; }
			set { settings = value; }
		}
	}
}
