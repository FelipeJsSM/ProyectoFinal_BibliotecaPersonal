using System.Collections.ObjectModel;
using System.Windows.Input;
using ProyectoFinal_BibliotecaPersonal.Models;
using ProyectoFinal_BibliotecaPersonal.Services;

namespace ProyectoFinal_BibliotecaPersonal.ViewModels
{
    public class LibraryViewModel : BindableObject
    {
        private readonly DatabaseService _database;

        public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>();

        public ICommand AddBookCommand { get; }
        public ICommand ViewBookCommand { get; }

        public LibraryViewModel(DatabaseService database)
        {
            _database = database;

            AddBookCommand = new Command(async () => await Shell.Current.GoToAsync("BookDetailPage"));
            ViewBookCommand = new Command<Book>(async (book) => await GoToBookDetails(book));
        }

        public async Task LoadBooksAsync()
        {
            var booksFromDb = await _database.GetBooksAsync();
            Books.Clear();
            foreach (var book in booksFromDb)
            {
                Books.Add(book);
            }
        }

        private async Task GoToBookDetails(Book book)
        {
            if (book == null) return;

            // Empacamos el libro para enviarlo a la otra pantalla
            var navigationParameter = new Dictionary<string, object>
        {
            { "Book", book }
        };

            // Viajamos a la página pasándole los datos
            await Shell.Current.GoToAsync("BookDetailPage", navigationParameter);
        }
    }
}