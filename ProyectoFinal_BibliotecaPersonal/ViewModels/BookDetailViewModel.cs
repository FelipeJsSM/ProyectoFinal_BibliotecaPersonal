using System.Windows.Input;
using ProyectoFinal_BibliotecaPersonal.Models;
using ProyectoFinal_BibliotecaPersonal.Services;

namespace ProyectoFinal_BibliotecaPersonal.ViewModels
{
    [QueryProperty(nameof(Book), "Book")]
    public class BookDetailViewModel : BindableObject
    {
        private readonly DatabaseService _database;
        private Book _book;

        public Book Book
        {
            get => _book;
            set { _book = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public BookDetailViewModel(DatabaseService database)
        {
            _database = database;
            Book = new Book();

            SaveCommand = new Command(async () => await SaveBookAsync());
            DeleteCommand = new Command(async () => await DeleteBookAsync());
        }

        private async Task SaveBookAsync()
        {
            if (string.IsNullOrWhiteSpace(Book.Title) || string.IsNullOrWhiteSpace(Book.Author))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El título y el autor son obligatorios", "OK");
                return;
            }

            await _database.SaveBookAsync(Book);
            await Shell.Current.GoToAsync("..");
        }

        private async Task DeleteBookAsync()
        {
            if (Book.Id != 0)
            {
                await _database.DeleteBookAsync(Book);
            }
            await Shell.Current.GoToAsync("..");
        }
    }
}