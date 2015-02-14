// MIT License Copyright 2010-2013 jmis
// See LICENSE.txt or http://opensource.org/licenses/MIT
// See AUTHORS.txt for a complete list of all contributors

using System;
using VisualClojure.Parsing;

namespace VisualClojure.Editor.InputHandling
{
	public class TokenChangedEventArgs : EventArgs
	{
		private readonly IndexToken _indexToken;

		public TokenChangedEventArgs(IndexToken indexToken)
		{
			_indexToken = indexToken;
		}

		public IndexToken IndexToken
		{
			get { return _indexToken; }
		}
	}
}