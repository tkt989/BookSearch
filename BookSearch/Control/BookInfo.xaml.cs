using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BookSearch.Control
{
    public partial class BookInfo : ContentView
    {
        public static readonly BindableProperty IsLoadingProperty =
            BindableProperty.Create("IsLoading", typeof(bool), typeof(BookInfo), false);

        public bool IsLoading
        {
            get => (bool) GetValue(IsLoadingProperty);
            set
            {
                SetValue(IsLoadingProperty, value);
            }
        }

        public BookInfo()
        {
            InitializeComponent();

            IsLoading = true;

            BindingContextChanged += (sender, e) =>
            {
                if (BindingContext == null) return;

                var info = BindingContext as Model.BookInfo;
                var thumbnail = info?.Thumbnail;
                if (string.IsNullOrEmpty(thumbnail))
                {
                    Thumbnail.IsVisible = false;
                    ThumbnailPlaceholder.IsVisible = true;
                } else {
                    Thumbnail.Source = thumbnail;
                    Thumbnail.IsVisible = true;
                    ThumbnailPlaceholder.IsVisible = false;
                }

                var title = info?.Title ?? "本の情報が見つかりませんでした";
                Title.Text = title;
                Title.IsVisible = true;
                TitlePlaceholder.IsVisible = false;
            };
        }
    }
}
