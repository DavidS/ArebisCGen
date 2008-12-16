using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace Arebis.CodeGeneration.VisualStudio
{
	/// <summary>
	/// An IFileWriter implementation that will attempt to add new files
	/// to a Visual Studio project file.
	/// </summary>
	/// <remarks>
	/// The extensions of project files to look for, can be overriden by
	/// setting the "vsprojectextensions" setting to a semi-colon separated list
	/// of file extensions. I.e. ".csproj;.vbproj".
	/// </remarks>
	[System.Diagnostics.DebuggerStepThrough]
	public class ProjectManagerFileWriter : BaseFileWriter
	{
		private List<string> projectExtensions = new List<string>(new String[] {".csproj", ".vbproj"});

		public override IGenerationHost Host
		{
			get { return base.Host; }
			set
			{
				// Set host:
				base.Host = value;

				// Retrieve project extensions from settings (if set):
				string[] extensions = this.Host.Settings.GetValues("vsprojectextensions") ?? new string[0];
				this.projectExtensions = new List<string>();
				foreach (string extlist in extensions)
				{
					foreach (string ext in extlist.Split(';'))
					{
						this.projectExtensions.Add(ext);
					}
				}
			}
		}

		public override void WriteFile(string filename, string content)
		{
			// Get file info:
			FileInfo fileinfo = new FileInfo(filename);

			// Add file to project only if new file:
			if (fileinfo.Exists == false) this.AddFileToProject(fileinfo);

			// Write the project item file:
			base.WriteFile(filename, content);
		}

		public virtual void AddFileToProject(FileInfo fileinfo)
		{
			// Find project file:
			FileInfo projectinfo = this.FindProjectFile(fileinfo);
			if (projectinfo == null) return;

			// Decide on elementname to add:
			string elementName;
			switch (fileinfo.Extension.ToLower())
			{
				case ".cs":
					elementName = "Compile";
					break;
				case ".vb":
					elementName = "Compile";
					break;
				default:
					elementName = "Content";
					break;
			}

			// Load project file:
			XmlDocument projdoc = new XmlDocument();
			projdoc.Load(projectinfo.FullName);

			// Search for an ItemGroup in file:
			XmlElement itemGroup = null;
			foreach (XmlElement groupItem in projdoc.DocumentElement.GetElementsByTagName("ItemGroup"))
			{
				itemGroup = groupItem;
				break;
			}

			// If no ItemGroup found, create one:
			if (itemGroup == null)
			{
				itemGroup = projdoc.CreateElement("ItemGroup", projdoc.NamespaceURI);
				projdoc.AppendChild(itemGroup);
			}
			
			// Add file in itemgroup:
			XmlElement item = projdoc.CreateElement(elementName, itemGroup.NamespaceURI);
			XmlAttribute itemattr = projdoc.CreateAttribute("Include");
			itemattr.Value = fileinfo.FullName.Replace(projectinfo.Directory.FullName + "\\", "");
			item.Attributes.Append(itemattr);
			itemGroup.AppendChild(item);

			// Save the projectfile:
			this.Host.Log("Added file \"{0}\" to project \"{1}\".", fileinfo.FullName, projectinfo.FullName);
			this.Host.WriteFile(projectinfo.FullName, projdoc.OuterXml);
		}

		public virtual FileInfo FindProjectFile(FileInfo fileinfo)
		{ 
			// Search for project files in file directory and parent directories:
			FileInfo projectinfo = null;
			DirectoryInfo dir = fileinfo.Directory;
			while (dir != null)
			{
				// Search for a project file in dir:
				if (dir.Exists)
				{
					foreach (string projext in this.projectExtensions)
					{
						foreach (FileInfo proj in dir.GetFiles("*" + projext))
						{
							projectinfo = proj;
							goto found;
						}
					}
				}

				// Move to parent directory:
				dir = dir.Parent;
			}
		found:
			return projectinfo;
		}

	}
}
