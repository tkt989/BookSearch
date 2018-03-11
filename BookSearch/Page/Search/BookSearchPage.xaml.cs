﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using BookSearch.Service;
using System.Runtime.CompilerServices;
using BookSearch.Model;

namespace BookSearch.Page.Search
{
    public partial class BookSearchPage : ContentPage
    {
        private string isbn;
        BookRepository bookRepository;

        public BookSearchPage()
        {
            InitializeComponent();
        }

        public BookSearchPage(string isbn) : this()
        {
            bookRepository = new BookRepository();
            this.isbn = isbn;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                    {
                        await bookRepository.SearchInFavorites(isbn, list =>
                        {
                            listView.ItemsSource = list;
                        });
                    });
            });
            
            Task.Run(async () =>
            {
                var info = await bookRepository.SearchBookInfo(isbn);

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (info == null)
                    {
                        BookTitle.Text = "本の情報が見つかりませんでした。";
                        Thumbnail.Source = null;
                        return;
                    }

                    BookTitle.Text = info.Title;
                    Thumbnail.Source = new UriImageSource() { Uri = new Uri(info.Thumbnail) };
                });
            });
        }

        public void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is SearchResult)) return;

            var result = e.Item as SearchResult;
            Navigation.PushAsync(new SearchDetailPage(result));
        }
    }
}
