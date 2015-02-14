﻿// MIT License Copyright 2010-2013 jmis
// See LICENSE.txt or http://opensource.org/licenses/MIT
// See AUTHORS.txt for a complete list of all contributors

using System.IO;
using System.Runtime.InteropServices;
using VisualClojure.Utilities;
using Microsoft.VisualStudio.Project;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace VisualClojure.Project.Launching
{
	public class ProjectLauncher
	{
		private readonly ProjectNode _project;
		private readonly IProvider<LaunchParameters> _launchParametersProvider;
		private readonly LaunchParametersValidator _validator;

		public ProjectLauncher(ProjectNode project, IProvider<LaunchParameters> launchParametersProvider, LaunchParametersValidator validator)
		{
			_project = project;
			_launchParametersProvider = launchParametersProvider;
			_validator = validator;
		}

		public void Execute()
		{
			var launchParameters = _launchParametersProvider.Get();
			_validator.Validate(launchParameters);

			var launchInfo = CreateClojureLaunchInfo(launchParameters);
			if (launchParameters.StartupFileType == StartupFileType.Executable) launchInfo = CreateExecutableLaunchInfo(launchParameters);
			VsShellUtilities.LaunchDebugger(_project.Site, launchInfo);
		}

		private static VsDebugTargetInfo CreateClojureLaunchInfo(LaunchParameters launchParameters)
		{
			var info = new VsDebugTargetInfo();
			info.cbSize = (uint)Marshal.SizeOf(info);
			info.dlo = DEBUG_LAUNCH_OPERATION.DLO_CreateProcess;
			info.bstrExe = launchParameters.RunnerPath;
			info.bstrCurDir = launchParameters.ApplicationPath;
			info.fSendStdoutToOutputWindow = 0;
			info.grfLaunch = (uint)__VSDBGLAUNCHFLAGS2.DBGLAUNCH_MergeEnv;
			info.bstrArg = "-i " + launchParameters.StartupFile;
			info.bstrRemoteMachine = launchParameters.RemoteDebugMachine;
			info.clsidCustom = launchParameters.DebugType;
			return info;
		}

		private static VsDebugTargetInfo CreateExecutableLaunchInfo(LaunchParameters launchParameters)
		{
			var info = new VsDebugTargetInfo();
			info.cbSize = (uint)Marshal.SizeOf(info);
			info.dlo = DEBUG_LAUNCH_OPERATION.DLO_CreateProcess;
			info.bstrExe = Path.Combine(launchParameters.ApplicationPath, launchParameters.StartupFile);
			info.bstrCurDir = launchParameters.ApplicationPath;
			info.fSendStdoutToOutputWindow = 0;
			info.grfLaunch = (uint)__VSDBGLAUNCHFLAGS2.DBGLAUNCH_MergeEnv;
			info.bstrArg = launchParameters.StartupArguments;
			info.bstrRemoteMachine = launchParameters.RemoteDebugMachine;
			info.clsidCustom = launchParameters.DebugType;
			return info;
		}
	}
}