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
    public partial class NoteEntryPage : ContentPage
    {
        public NoteEntryPage()
        {
            InitializeComponent();   
        }

        protected override void OnAppearing()
        {
            var note = (Note)BindingContext;
            if (!string.IsNullOrEmpty(note.FileName))
            {
                editor.Text = File.ReadAllText(note.FileName);
            }
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var note = (Note)BindingContext;
            if (string.IsNullOrEmpty(note.FileName))
            {
                //Create and save
                note.FileName = Path.Combine(Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData), 
                    $"{Path.GetRandomFileName()}.notes.txt");
            } 
 
            File.WriteAllText(note.FileName, editor.Text);

            await Navigation.PopModalAsync();
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var note = (Note)BindingContext;

            if (File.Exists(note.FileName))
            {
                File.Delete(note.FileName);
            }
            editor.Text = string.Empty;
            await Navigation.PopModalAsync();

        }
    }
}