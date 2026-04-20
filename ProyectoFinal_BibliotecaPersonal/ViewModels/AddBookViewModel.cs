using System.Windows.Input;
using ProyectoFinal_BibliotecaPersonal.Models;
using ProyectoFinal_BibliotecaPersonal.Services;

namespace ProyectoFinal_BibliotecaPersonal.ViewModels
{
    public class AddBookViewModel : BindableObject
    {
        private readonly DatabaseService _database;
        private Book _book;

        public Book Book
        {
            get => _book;
            set { _book = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddBookViewModel(DatabaseService database)
        {
            _database = database;
            // ¡CRÍTICO! Inicializamos un libro vacío en blanco
            Book = new Book();

            SaveCommand = new Command(async () => await SaveBookAsync());
            CancelCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        private async Task SaveBookAsync()
        {
            if (string.IsNullOrWhiteSpace(Book.Title) || string.IsNullOrWhiteSpace(Book.Author))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El título y el autor son obligatorios", "OK");
                return;
            }

            await _database.SaveBookAsync(Book);

            await Application.Current.MainPage.DisplayAlert("Éxito", "Libro agregado a tu biblioteca", "OK");

            await Shell.Current.GoToAsync("..");
        }
    }
}