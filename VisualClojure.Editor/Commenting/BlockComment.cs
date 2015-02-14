// MIT License Copyright 2010-2013 jmis
// See LICENSE.txt or http://opensource.org/licenses/MIT
// See AUTHORS.txt for a complete list of all contributors

using System.Collections.Generic;
using VisualClojure.Editor.TextBuffer;

namespace VisualClojure.Editor.Commenting
{
	public class BlockComment
	{
		private readonly ITextBufferAdapter _textBuffer;

		public BlockComment(ITextBufferAdapter textBuffer)
		{
			_textBuffer = textBuffer;
		}

		public void Execute()
		{
			List<string> lines = _textBuffer.GetSelectedLines();
			List<string> commentedLines = new List<string>();
			foreach (string line in lines) commentedLines.Add(";" + line);
			_textBuffer.ReplaceSelectedLines(commentedLines);
		}
	}
}