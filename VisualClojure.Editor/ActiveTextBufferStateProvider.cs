﻿// MIT License Copyright 2010-2013 jmis
// See LICENSE.txt or http://opensource.org/licenses/MIT
// See AUTHORS.txt for a complete list of all contributors

using System.Collections.Generic;
using VisualClojure.Parsing;
using VisualClojure.Utilities;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace VisualClojure.Editor
{
	public class ActiveTextBufferStateProvider : IProvider<LinkedList<Token>>
	{
		private readonly IVsEditorAdaptersFactoryService _vsEditorAdaptersFactoryService;
		private readonly IVsTextManager _vsTextManager;

		public ActiveTextBufferStateProvider(IVsEditorAdaptersFactoryService vsEditorAdaptersFactoryService, IVsTextManager vsTextManager)
		{
			_vsEditorAdaptersFactoryService = vsEditorAdaptersFactoryService;
			_vsTextManager = vsTextManager;
		}

		public LinkedList<Token> Get()
		{
			IVsTextView activeView = null;
			_vsTextManager.GetActiveView(0, null, out activeView);
			IWpfTextView wpfTextView = _vsEditorAdaptersFactoryService.GetWpfTextView(activeView);
			return TokenizedBufferBuilder.TokenizedBuffers[wpfTextView.TextBuffer].CurrentState;
		}
	}
}