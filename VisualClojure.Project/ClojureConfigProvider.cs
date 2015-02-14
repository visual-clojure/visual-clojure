// MIT License Copyright 2010-2013 jmis
// See LICENSE.txt or http://opensource.org/licenses/MIT
// See AUTHORS.txt for a complete list of all contributors

using VisualClojure.Project.Launching;
using VisualClojure.Project;
using Microsoft.VisualStudio.Project;

namespace VisualClojure.Project
{
	public class ClojureConfigProvider : ConfigProvider
	{
		private readonly ClojureProjectNode _clojureProjectNode;

		public ClojureConfigProvider(ClojureProjectNode clojureProjectNode) : base(clojureProjectNode)
		{
			_clojureProjectNode = clojureProjectNode;
		}

		protected override ProjectConfig CreateProjectConfiguration(string configName)
		{
			return new ClojureProjectConfig(
				_clojureProjectNode,
				configName,
				new ProjectLauncher(
					_clojureProjectNode,
					new LaunchParametersBuilder(_clojureProjectNode),
					new LaunchParametersValidator()));
		}
	}
}