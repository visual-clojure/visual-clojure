﻿// MIT License Copyright 2010-2013 jmis
// See LICENSE.txt or http://opensource.org/licenses/MIT
// See AUTHORS.txt for a complete list of all contributors

using System.ComponentModel.Composition;
using VisualClojure.Editor.Tagger;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace VisualClojure.Editor.Classification
{
	[Export(typeof(IViewTaggerProvider))]
	[ContentType("Clojure")]
	[TagType(typeof(ClassificationTag))]
	internal sealed class ClojureClassifierProvider : IViewTaggerProvider
	{
		[Export]
		[Name("Clojure")]
		[BaseDefinition("code")]
		internal static ContentTypeDefinition ClojureContentType = null;

		[Export]
		[FileExtension(".clj")]
		[ContentType("Clojure")]
		internal static FileExtensionToContentTypeDefinition ClojureFileType = null;

		[Export]
		[FileExtension(".cljs")]
		[ContentType("Clojure")]
		internal static FileExtensionToContentTypeDefinition ClojureScriptFileType = null;

		[Import]
		internal IClassificationTypeRegistryService ClassificationTypeRegistry = null;

		[Import]
		internal IViewTagAggregatorFactoryService aggregatorFactory = null;

		public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
		{
			ITagAggregator<ClojureTokenTag> clojureTagAggregator = aggregatorFactory.CreateTagAggregator<ClojureTokenTag>(textView);

			return new ClojureClassifier(buffer, clojureTagAggregator, ClassificationTypeRegistry) as ITagger<T>;
		}
	}
}
