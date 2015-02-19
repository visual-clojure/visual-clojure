// MIT License Copyright 2010-2013 jmis
// See LICENSE.txt or http://opensource.org/licenses/MIT
// See AUTHORS.txt for a complete list of all contributors

using System.Diagnostics;
using VisualClojure.Utilities;

namespace VisualClojure.Deployment
{
    public class ReplUtilities
    {
        private static readonly string clojureClrRuntime = string.Format("{0}\\{1}-{2}", EnvironmentVariables.VsClojureRuntimesDir, Constants.CLOJURECLR, Constants.VERSION);
        
        public static Process CreateReplProcess()
        {
            return CreateReplProcess(clojureClrRuntime, "");
        }

        public static Process CreateReplProcess(string replPath, string projectPath)
        {
            var process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo
                {
                    FileName = "\"" + replPath + "\\Clojure.Main.exe\"",
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };

            return process;
        }
    }
}