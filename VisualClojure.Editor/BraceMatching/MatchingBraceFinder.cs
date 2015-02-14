﻿// MIT License Copyright 2010-2013 jmis
// See LICENSE.txt or http://opensource.org/licenses/MIT
// See AUTHORS.txt for a complete list of all contributors

using System.Collections.Generic;
using VisualClojure.Editor.InputHandling;
using VisualClojure.Editor.TextBuffer;
using VisualClojure.Parsing;
using VisualClojure.Utilities;

namespace VisualClojure.Editor.BraceMatching
{
	public class MatchingBraceFinder
	{
		private readonly ITextBufferAdapter _textBuffer;
		private readonly Entity<LinkedList<Token>> _tokenizedBuffer;

		public MatchingBraceFinder(ITextBufferAdapter textBuffer, Entity<LinkedList<Token>> tokenizedBuffer)
		{
			_textBuffer = textBuffer;
			_tokenizedBuffer = tokenizedBuffer;
		}

		public MatchingBracePair FindMatchingBraces(int caretPosition)
		{
			IndexTokenNode tokenNode = _tokenizedBuffer.CurrentState.FindTokenAtIndex(caretPosition);
			if (tokenNode == null) return new MatchingBracePair(null, null);
			IndexTokenNode previousNode = tokenNode.Previous();

			if (_textBuffer.Length == caretPosition && tokenNode.IndexToken.Token.Type.IsBraceEnd())
			{
				return FindMatchingBracePair(tokenNode);
			}

			if (previousNode != null && previousNode.IndexToken.Token.Type.IsBraceEnd() && caretPosition == tokenNode.IndexToken.StartIndex)
			{
				return FindMatchingBracePair(previousNode);
			}

			if (tokenNode.IndexToken.Token.Type.IsBraceStart()) return FindMatchingBracePair(tokenNode);
			return new MatchingBracePair(null, null);
		}

		private static MatchingBracePair FindMatchingBracePair(IndexTokenNode tokenNode)
		{
			IndexToken matchingBrace = ScanForMatchingBrace(tokenNode);
			if (matchingBrace == null) return new MatchingBracePair(tokenNode.IndexToken, null);
			return new MatchingBracePair(tokenNode.IndexToken, matchingBrace);
		}

		private static IndexToken ScanForMatchingBrace(IndexTokenNode tokenNode)
		{
			TokenType matchingType = tokenNode.IndexToken.Token.Type.MatchingBraceType();
			IndexTokenNode candidateToken = tokenNode;
			bool matchForward = tokenNode.IndexToken.Token.Type.IsBraceStart();
			int braceCounter = 0;

			while (candidateToken != null)
			{
				if (candidateToken.IndexToken.Token.Type == matchingType) braceCounter--;
				if (candidateToken.IndexToken.Token.Type == tokenNode.IndexToken.Token.Type) braceCounter++;
				if (braceCounter == 0) return candidateToken.IndexToken;
				candidateToken = matchForward ? candidateToken.Next() : candidateToken.Previous();
			}

			return null;
		}
	}
}