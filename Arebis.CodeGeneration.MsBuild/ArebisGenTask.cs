
namespace Arebis.CodeGeneration.MsBuild
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Arebis.CodeGenerator.Templated;

    public class ArebisGenTask
        : Task
    {
        [Required]
        public ITaskItem[] Templates
        {
            get;
            set;
        }

        [Output]
        public ITaskItem[] GeneratedFiles
        {
            get;
            set;
        }

        public override bool Execute()
        {
            // Default to T3 syntax:
            new Arebis.CodeGenerator.Templated.Syntax.T3Syntax().Setup(null);
            var generatedFileNames = new List<string>(this.Templates.Length);
            Log.LogMessage("Starting Arebis Generation");
            foreach (var task in this.Templates)
            {
                string inputFileName = task.ItemSpec;
                string outputFileName = Path.ChangeExtension(inputFileName, ".Designer.cs");
                string result;

                try
                {
                    // Build code string
                    var builder = new CSCodeBuilder();
                    var info = new TemplateInfo(inputFileName, new MsBuildHost(Log));
                    builder.TemplateInfo = info;
                    info.Parse();
                    result = builder.CreateCode();

                    try
                    {
                        using (var destination = new FileStream(outputFileName, FileMode.Create))
                        {
                            var bytes = Encoding.UTF8.GetBytes(result);
                            destination.Write(bytes, 0, bytes.Length);
                        }
                        generatedFileNames.Add(outputFileName);
                    }
                    catch (Exception ex)
                    {
                        Log.LogError("Error while writing to [{0}]", outputFileName);
                        Log.LogErrorFromException(ex, true, true, inputFileName);
                    }
                }
                catch (Exception ex)
                {
                    Log.LogError("Error while compiling [{0}]", inputFileName);
                    Log.LogErrorFromException(ex, true, true, inputFileName);
                }
            }
            GeneratedFiles = generatedFileNames.Select(name => new TaskItem(name)).ToArray();
            Log.LogMessage("Finished Arebis Generation");
            return !Log.HasLoggedErrors;
        }
    }
}
