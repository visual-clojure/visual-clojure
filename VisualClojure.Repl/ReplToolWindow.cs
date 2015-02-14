﻿// MIT License Copyright 2010-2013 jmis
// See LICENSE.txt or http://opensource.org/licenses/MIT
// See AUTHORS.txt for a complete list of all contributors

using System.Runtime.InteropServices;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell;

namespace VisualClojure.Repl
{
    [Guid("8C5C7302-ECC8-435D-AAFE-D0E5A0A02FE9")]
    public class ReplToolWindow : ToolWindowPane
    {
        private readonly TabControl _replManager;

        public TabControl TabControl
        {
            get { return _replManager; }
        }

        public ReplToolWindow()
            : this(new ReplTabControl())
        {
        }

        public ReplToolWindow(TabControl replManager) :
            base(null)
        {
            _replManager = replManager;
            Caption = "Clojure Repl Manager";
            BitmapResourceID = 301;
            BitmapIndex = 1;
            base.Content = _replManager;
        }
    }
}