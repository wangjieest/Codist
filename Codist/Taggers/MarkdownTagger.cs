﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using AppHelpers;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Codist.Taggers
{
	[Export(typeof(IViewTaggerProvider))]
	[ContentType(Constants.CodeTypes.Code)]
	[TagType(typeof(IClassificationTag))]
	sealed class MarkdownTaggerProvider : IViewTaggerProvider
	{
		static readonly ClassificationTag[] _HeaderClassificationTypes = new ClassificationTag[7];

		public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag {
			if (Config.Instance.Features.MatchFlags(Features.SyntaxHighlight) == false) {
				return null;
			}
			if (textView.TextBuffer.LikeContentType(Constants.CodeTypes.Markdown) == false) {
				return null;
			}
			if (_HeaderClassificationTypes[1] == null) {
				InitHeaderClassificationTypes();
			}
			return textView.Properties.GetOrCreateSingletonProperty(() => new MarkdownTagger(textView)) as ITagger<T>;
		}

		static void InitHeaderClassificationTypes() {
			var r = ServicesHelper.Instance.ClassificationTypeRegistry;
			_HeaderClassificationTypes[1] = new ClassificationTag(r.GetClassificationType(Constants.MarkdownHeading1));
			_HeaderClassificationTypes[2] = new ClassificationTag(r.GetClassificationType(Constants.MarkdownHeading2));
			_HeaderClassificationTypes[3] = new ClassificationTag(r.GetClassificationType(Constants.MarkdownHeading3));
			_HeaderClassificationTypes[4] = new ClassificationTag(r.GetClassificationType(Constants.MarkdownHeading4));
			_HeaderClassificationTypes[5] = new ClassificationTag(r.GetClassificationType(Constants.MarkdownHeading5));
			_HeaderClassificationTypes[6] = new ClassificationTag(r.GetClassificationType(Constants.MarkdownHeading6));
		}

		sealed class MarkdownTagger : CachedTaggerBase
		{
			public MarkdownTagger(ITextView textView) : base(textView) {
			}
			protected override bool DoFullParseAtFirstLoad => true;
			protected override void Parse(SnapshotSpan span, ICollection<TaggedContentSpan> results) {
				var t = span.GetText();
				if (t.Length < 1 || t[0] != '#') {
					return;
				}
				int c = 1, w = 0;
				for (int i = 1; i < t.Length; i++) {
					switch (t[i]) {
						case '#': if (w == 0) { ++c; } continue;
						case ' ':
						case '\t': ++w; continue;
					}
					break;
				}
				w += c;
				results.Add(new TaggedContentSpan(_HeaderClassificationTypes[c], span, w, t.Length - w));
			}
		}
	}
}
