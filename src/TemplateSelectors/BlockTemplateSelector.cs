using System;
using MarkdownView.ViewModels;
using MarkdownView.Views.Blocks;
using Xamarin.Forms;

namespace MarkdownView.TemplateSelectors
{
    // TODO: Rename file
    public class BlockTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var type = item.GetType();

            if (type == typeof(HeaderBlockViewModel))
            {
                return new DataTemplate(typeof(HeaderBlockView));
            }

            if (type == typeof(ParagraphBlockViewModel))
            {
                return new DataTemplate(typeof(ParagraphBlockView));
            }

            throw new ArgumentException();
        }
    }
}
