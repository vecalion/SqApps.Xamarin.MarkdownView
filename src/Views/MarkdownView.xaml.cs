using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownView.ViewModels;
using Xamarin.Forms;

namespace MarkdownView.Views
{
    public partial class MarkdownView : ContentView
    {
        // public Style Style
        // {
        //     get { return (Style)GetValue(StyleProperty); }
        //     set { SetValue(StyleProperty, value); }
        // }
        //
        // public static readonly BindableProperty StyleProperty =
        //     BindableProperty.Create("Style", typeof(Style), typeof(VisualElement), default(Style),
        //         propertyChanged: (bindable, oldvalue, newvalue) => ((NavigableElement)bindable)._mergedStyle.Style = (Style)newvalue);
        
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create (nameof(Text), typeof(string), typeof(MarkdownView), null, propertyChanged: OnTextChanged);
        
        public string Text
        {
            get => (string)GetValue (TextProperty);
            set => SetValue (TextProperty, value);
        }
        
        static void OnTextChanged (BindableObject bindable, object oldValue, object newValue)
        {
            var view = (MarkdownView) bindable;
            var text = (string) newValue;
            
            view.Blocks = string.IsNullOrEmpty(text) ? new List<BaseBlockViewModel>() : Parser.ParseBlocks(text).ToList();
        }

        public static readonly BindableProperty BlocksProperty =
            BindableProperty.Create (nameof(Blocks), typeof(List<BaseBlockViewModel>), typeof(MarkdownView), null);
        
        public List<BaseBlockViewModel> Blocks
        {
            get => (List<BaseBlockViewModel>)GetValue (BlocksProperty);
            set => SetValue (BlocksProperty, value);
        }

        public MarkdownView()
        {
            InitializeComponent();
        }
    }
}
