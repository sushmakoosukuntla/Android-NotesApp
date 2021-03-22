using Notes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Notes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var notes = new List<Note>();
            var files = Directory.EnumerateFiles(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "*.notes.txt");
            foreach (var filename in files)
            {
                var note = new Note
                {
                    Text = File.ReadAllText(filename),
                    FileName = filename,
                    Date = File.GetCreationTime(filename)
                };
                notes.Add(note);
            }
            NotesListView.ItemsSource = notes.OrderBy(n => n.Date).ToList();
        }

        private async void OnNoteAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NoteEntryPage
            {
                BindingContext = new Note()
            });
        }

        private async void OnNoteItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushModalAsync(new NoteEntryPage
                {
                    BindingContext = (Note)e.SelectedItem
                });
            }
        }
    }
}