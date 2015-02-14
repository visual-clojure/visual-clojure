﻿// MIT License Copyright 2010-2013 jmis
// See LICENSE.txt or http://opensource.org/licenses/MIT
// See AUTHORS.txt for a complete list of all contributors

using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using VisualClojure.Utilities;

namespace VisualClojure.Repl
{
	public class History
	{
		private readonly KeyboardExaminer _keyboardExaminer;
		private readonly Entity<ReplState> _replEntity;
		private readonly TextBox _interactiveTextBox;
		private LinkedListNode<string> _currentlySelectedHistoryItem;

		public History(KeyboardExaminer keyboardExaminer, Entity<ReplState> replEntity, TextBox interactiveTextBox)
		{
			_keyboardExaminer = keyboardExaminer;
			_replEntity = replEntity;
			_interactiveTextBox = interactiveTextBox;
		}

		public void PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && !_keyboardExaminer.IsShiftDown() && _interactiveTextBox.CaretIndex >= _replEntity.CurrentState.PromptPosition)
			{
				SubmitInputToHistory();
			}

			if (_interactiveTextBox.CaretIndex >= _replEntity.CurrentState.PromptPosition && e.Key == Key.Down)
			{
				ShowPreviousHistoryItem();
				e.Handled = true;
				return;
			}

			if (_interactiveTextBox.CaretIndex >= _replEntity.CurrentState.PromptPosition && e.Key == Key.Up)
			{
				ShowNextItemInHistory();
				e.Handled = true;
				return;
			}
		}

		private void ShowNextItemInHistory()
		{
			if (_currentlySelectedHistoryItem == null) _currentlySelectedHistoryItem = _replEntity.CurrentState.History.First;
			else if (_currentlySelectedHistoryItem.Next != null) _currentlySelectedHistoryItem = _currentlySelectedHistoryItem.Next;

			if (_currentlySelectedHistoryItem != null)
			{
				_interactiveTextBox.Text = _interactiveTextBox.Text.Remove(_replEntity.CurrentState.PromptPosition, _interactiveTextBox.Text.Length - _replEntity.CurrentState.PromptPosition);
				_interactiveTextBox.AppendText(_currentlySelectedHistoryItem.Value);
				_interactiveTextBox.CaretIndex = _interactiveTextBox.Text.Length;
			}
		}

		private void ShowPreviousHistoryItem()
		{
			if (_currentlySelectedHistoryItem != null)
			{
				_interactiveTextBox.Text = _interactiveTextBox.Text.Remove(_replEntity.CurrentState.PromptPosition, _interactiveTextBox.Text.Length - _replEntity.CurrentState.PromptPosition);

				if (_currentlySelectedHistoryItem.Previous == null)
				{
					_currentlySelectedHistoryItem = null;
				}
				else
				{
					_currentlySelectedHistoryItem = _currentlySelectedHistoryItem.Previous;
					_interactiveTextBox.AppendText(_currentlySelectedHistoryItem.Value);
				}

				_interactiveTextBox.CaretIndex = _interactiveTextBox.Text.Length;
			}
		}

		private void SubmitInputToHistory()
		{
			string userInput = _interactiveTextBox.Text.Substring(_replEntity.CurrentState.PromptPosition);
			LinkedList<string> history = _replEntity.CurrentState.History;

			if (_currentlySelectedHistoryItem != null && _currentlySelectedHistoryItem.Value == userInput)
			{
				history.Remove(_currentlySelectedHistoryItem.Value);
				history.AddFirst(_currentlySelectedHistoryItem.Value);
			}
			else if (!string.IsNullOrEmpty(userInput.Trim()))
			{
				history.AddFirst(userInput);
			}

			_replEntity.CurrentState = _replEntity.CurrentState.ChangeHistory(history);
			_currentlySelectedHistoryItem = null;
		}
	}
}
