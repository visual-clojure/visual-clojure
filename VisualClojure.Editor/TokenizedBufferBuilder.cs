﻿// MIT License Copyright 2010-2013 jmis
// See LICENSE.txt or http://opensource.org/licenses/MIT
// See AUTHORS.txt for a complete list of all contributors

using System.Collections.Generic;
using VisualClojure.Parsing;
using VisualClojure.Utilities;
using Microsoft.VisualStudio.Text;

namespace VisualClojure.Editor
{
	public class TokenizedBufferBuilder
	{
		private readonly Tokenizer _tokenizer;
		public static Dictionary<ITextBuffer, Entity<LinkedList<Token>>> TokenizedBuffers = new Dictionary<ITextBuffer, Entity<LinkedList<Token>>>();

		public TokenizedBufferBuilder(Tokenizer tokenizer)
		{
			_tokenizer = tokenizer;
		}

		public void CreateTokenizedBuffer(ITextBuffer buffer)
		{
			var tokenizedBuffer = new Entity<LinkedList<Token>>();
			tokenizedBuffer.CurrentState = _tokenizer.Tokenize(buffer.CurrentSnapshot.GetText());
			TokenizedBuffers.Add(buffer, tokenizedBuffer);
		}

		public void RemoveTokenizedBuffer(ITextBuffer buffer)
		{
			if (TokenizedBuffers.ContainsKey(buffer)) TokenizedBuffers.Remove(buffer);
		}
	}
}