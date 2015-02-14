﻿// MIT License Copyright 2010-2013 jmis
// See LICENSE.txt or http://opensource.org/licenses/MIT
// See AUTHORS.txt for a complete list of all contributors

using System.Windows.Controls;
using System.Windows.Threading;
using VisualClojure.Utilities;

namespace VisualClojure.Repl
{
	public class TextBoxWriter
	{
		private readonly TextBox _interactiveTextBox;
		private readonly Entity<ReplState> _replEntity;

		public TextBoxWriter(TextBox interactiveTextBox, Entity<ReplState> replEntity)
		{
			_interactiveTextBox = interactiveTextBox;
			_replEntity = replEntity;
		}

		public void WriteToTextBox(string output)
		{
			_interactiveTextBox.Dispatcher.Invoke(
				DispatcherPriority.Normal,
				new DispatcherOperationCallback(
					delegate
					{
						_interactiveTextBox.AppendText(output);
						_interactiveTextBox.ScrollToEnd();
						_replEntity.CurrentState =
							_replEntity.CurrentState.ChangePromptPosition(
								_interactiveTextBox.Text.
									Length);
						return null;
					}), null);
		}
	}
}