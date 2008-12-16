using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.Client;
using System.IO;

namespace Arebis.CodeGeneration.VisualStudio
{
	/// <summary>
	/// An IFileWriter implementation that allows for add and checkout of files
	/// on Microsoft Team Foundation Source Control.
	/// </summary>
	/// <remarks>
	/// Following settings are expected by the TFSCFileWriter:<br/>
	/// "teamsystemserver", an URI pointing to the TeamSystem server;<br/>
	/// "teamsystemworkspace", path to the local workspace.<br/>
	/// To install the TFSCFileWriter, set the "filewriter" setting to
	/// "Arebis.CodeGeneration.VisualStudio.TFSCFileWriter, Arebis.CodeGeneration.VisualStudio"
	/// </remarks>
	[System.Diagnostics.DebuggerStepThrough]
	public class TFSCFileWriter : BaseFileWriter
	{
		private TeamFoundationServer tsServer;
		private VersionControlServer tsVcs;
		private Workspace tsWorkspace;

		public override IGenerationHost Host
		{
			get { return base.Host; }
			set
			{
				base.Host = value;

				try
				{
					this.tsServer = TeamFoundationServerFactory.GetServer(value.Settings["teamsystemserver"]);
					this.tsVcs = (VersionControlServer)tsServer.GetService(typeof(VersionControlServer));
					this.tsWorkspace = tsVcs.GetWorkspace(value.Settings["teamsystemworkspace"]);
				}
				catch (Exception ex)
				{
					throw new ApplicationException("Failed to initialize TFSCFileWriter: " + ex.Message);
				}
			}
		}

		public override void WriteFile(string filename, string content)
		{
			// Check if file has changed:
			try
			{
				string originalFile = File.ReadAllText(filename);

				// If content equal, do not proceed further:
				if (content.Equals(originalFile))
				{
					this.Host.Log("TFSCFileWriter: File '{0}' not changed.", filename);
					return;
				}
			}
			catch (IOException)
			{
				// Ignore errors.
			}

			// Retrieve file from source repository:
			string serverfile = tsWorkspace.GetServerItemForLocalItem(filename);
			tsWorkspace.Get(new GetRequest(serverfile, RecursionType.None, VersionSpec.Latest), GetOptions.Overwrite);
			try
			{
				tsVcs.DownloadFile(serverfile, filename);
			}
			catch (VersionControlException)
			{
				// Downloads of files that do not exist should fail.
				// This is expected behaviour.
			}

			// Edit/Write/Add the file:
			tsWorkspace.PendEdit(filename);
			try
			{
				base.WriteFile(filename, content);
				tsWorkspace.PendAdd(filename);
				this.Host.Log("TFSCFileWriter: File '{0}' written.", filename);
			}
			catch (Exception ex)
			{
				this.Host.Log("TFSCFileWriter: Error writing file \"{0}\" : {1}", filename, ex.Message);
			}
		}
	}
}
