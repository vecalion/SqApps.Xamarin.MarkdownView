using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using MvvmHelpers.Commands;
using Xamarin.Forms;

namespace MarkdownView.ViewModels
{
    public class ParagraphBlockViewModel : BaseBlockViewModel
    {
        public void GenerateFormattedString()
        {
            var result = new FormattedString();

            foreach (var part in Spans)
            {
                var styleName = "SpanRegular"; 

                if (part.IsBold && part.IsItalic)
                {
                    styleName = "SpanBoldItalic";
                }
                else if (part.IsBold)
                {
                    styleName = "SpanBold";
                }
                else if (part.IsItalic)
                {
                    styleName = "SpanItalic";
                }

                var span = new Span
                {
                    Text = part.Text,
                };

                if (!string.IsNullOrEmpty(part.Link))
                {
                    var tap = new TapGestureRecognizer();
                    tap.SetBinding(TapGestureRecognizer.CommandProperty, "OpenUrlCommand");
                    tap.CommandParameter = part.Link;
                    span.GestureRecognizers.Add(tap);

                    styleName = "SpanUnderline";
                }

                // span.Style = (Style)Application.  .Application.Current.Resources[styleName];
                span.Style = (Style)Application.Current.Resources[styleName];

                result.Spans.Add(span);
            }

            FormattedText = result;
        }

        AsyncCommand<string> openUrlCommand;
        public AsyncCommand<string> OpenUrlCommand => openUrlCommand ?? new AsyncCommand<string>(HandleOpen);

        Task HandleOpen(string url)
        {
            return Xamarin.Essentials.Browser.OpenAsync(url);
        }

        public FormattedString FormattedText { get; set; }

        public List<SpanInline> Spans { get; internal set; }

        public ParagraphBlockViewModel()
        {
            Spans = new List<SpanInline>();
        }
    }
}
