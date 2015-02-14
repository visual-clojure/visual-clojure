// MIT License Copyright 2010-2013 jmis
// See LICENSE.txt or http://opensource.org/licenses/MIT
// See AUTHORS.txt for a complete list of all contributors

using VisualClojure.Utilities;
using VisualClojure.Editor.Options;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace VisualClojure.Editor.AutoIndent
{
	public class SmartIndentAdapter : ISmartIndent
	{
		private readonly ClojureSmartIndent _clojureSmartIndent;
		private readonly IProvider<EditorOptions> _optionsBuilder;

		public SmartIndentAdapter(ClojureSmartIndent clojureSmartIndent, IProvider<EditorOptions> optionsBuilder)
		{
			_clojureSmartIndent = clojureSmartIndent;
			_optionsBuilder = optionsBuilder;
		}

		public void Dispose()
		{
		}

		public int? GetDesiredIndentation(ITextSnapshotLine line)
		{
			return _clojureSmartIndent.GetDesiredIndentation(line.Start.Position, _optionsBuilder.Get());
		}
	}
}