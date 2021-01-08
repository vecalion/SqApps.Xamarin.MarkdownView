using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using MarkdownView.ViewModels;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;

namespace MarkdownView
{
    internal static class Parser
    {
        enum Controls
        {
            Bold,
            Italic
        }

        public static IEnumerable<BaseBlockViewModel> ParseBlocks(string text)
        {
            var document = new MarkdownDocument();
            document.Parse(text);

            foreach (var element in document.Blocks)
            {
                if (element is HeaderBlock header)
                {
                    if (header.Inlines.First() is MarkdownInline inline)
                    {
                        yield return new HeaderBlockViewModel { Header = inline.ToString() };
                    }
                }

                if (element is ParagraphBlock paragraph)
                {
                    yield return CreateParagraphViewModel(paragraph);
                }

                if (!(element is ListBlock list)) continue;
                
                for (var i = 0; i < list.Items.Count; i++)
                {
                    var item = list.Items[i];

                    if (!(item.Blocks.First() is ParagraphBlock paragraphBlock))
                    {
                        continue;
                    }

                    yield return CreateParagraphViewModel(paragraphBlock, $"{i + 1}. "); ;
                }
            }  
        }

        static ParagraphBlockViewModel CreateParagraphViewModel(ParagraphBlock paragraph, string prefix = null)
        {
            var viewModel = new ParagraphBlockViewModel();

            if (!string.IsNullOrEmpty(prefix))
            {
                viewModel.Spans.Add(new SpanInline { Text = prefix });
            }

            foreach (var inline in paragraph.Inlines)
            {
                var span = GetInlineTextWithFlags(inline);
                if (span == null)
                {
                    continue;
                }

                viewModel.Spans.Add(span);
            }

            viewModel.GenerateFormattedString();
            return viewModel;
        }

        static SpanInline GetInlineTextWithFlags(MarkdownInline inline, SpanInline accum = null)
        {
            accum ??= new SpanInline();

            switch (inline)
            {
                case TextRunInline textRun:
                    accum.Text = textRun.Text;
                    return accum;
                case BoldTextInline bold:
                    accum.IsBold = true;
                    return GetInlineTextWithFlags(bold.Inlines.First(), accum);
                case ItalicTextInline italic:
                    accum.IsItalic = true;
                    return GetInlineTextWithFlags(italic.Inlines.First(), accum);
                case MarkdownLinkInline link:
                    accum.Link = link.Url;
                    return GetInlineTextWithFlags(link.Inlines.First(), accum);
                default:
                    return null;
            }
        }
    }
}
