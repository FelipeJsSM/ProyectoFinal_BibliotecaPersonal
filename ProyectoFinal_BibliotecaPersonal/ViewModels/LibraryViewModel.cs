using System.Collections.ObjectModel;
using System.Windows.Input;
using ProyectoFinal_BibliotecaPersonal.Models;
using ProyectoFinal_BibliotecaPersonal.Services;

namespace ProyectoFinal_BibliotecaPersonal.ViewModels
{
    public class LibraryViewModel : BindableObject
    {
        private readonly DatabaseService _database;

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                SearchCommand.Execute(null);
            }
        }

        public ObservableCollection<string> Genres { get; set; } = new ObservableCollection<string>();

        private string _selectedGenre;
        public string SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre = value;
                OnPropertyChanged();
                FilterByGenreCommand.Execute(null); 
            }
        }

        public ICommand FilterByGenreCommand { get; }

        public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>();

        public int TotalBooks => Books.Count;
        public int ReadBooks => Books.Count(b => b.IsRead);

        public ICommand AddBookCommand { get; }
        public ICommand ViewBookCommand { get; }
        public ICommand FilterCommand { get; }
        public ICommand SearchCommand { get; }

        public LibraryViewModel(DatabaseService database)
        {
            _database = database;

            AddBookCommand = new Command(async () => await Shell.Current.GoToAsync("AddBookPage"));
            FilterCommand = new Command<string>(async (status) => await ApplyFilter(status));
            SearchCommand = new Command(async () => await LoadBooksAsync());
            ViewBookCommand = new Command<Book>(async (book) => await GoToBookDetails(book));
            FilterByGenreCommand = new Command(async () => await FilterByGenreAsync());
        }

        public async Task LoadBooksAsync()
        {
            IEnumerable<Book> booksFromDb;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                booksFromDb = await _database.GetBooksAsync();
            }
            else
            {
                var booksForSearch = await _database.GetBooksAsync();
                booksFromDb = booksForSearch.Where(b =>
                    b.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    b.Author.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            Books.Clear();
            foreach (var book in booksFromDb)
            {
                Books.Add(book);
            }

            var booksForGenres = await _database.GetBooksAsync();
            var distinctGenres = booksForGenres.Select(b => b.Genre).Where(g => !string.IsNullOrWhiteSpace(g)).Distinct().ToList();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Genres.Clear();
                Genres.Add("Todos los géneros");
                foreach (var genre in distinctGenres)
                {
                    Genres.Add(genre);
                }
            });

            OnPropertyChanged(nameof(TotalBooks));
            OnPropertyChanged(nameof(ReadBooks));
        }

        private async Task FilterByGenreAsync()
        {
            if (string.IsNullOrEmpty(SelectedGenre)) return;

            try
            {
                IEnumerable<Book> filtered;

                if (SelectedGenre == "Todos los géneros")
                    filtered = await _database.GetBooksAsync();
                else
                    filtered = await _database.GetBooksByGenreAsync(SelectedGenre);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Books.Clear();
                    foreach (var book in filtered)
                    {
                        Books.Add(book);
                    }
                    OnPropertyChanged(nameof(TotalBooks));
                    OnPropertyChanged(nameof(ReadBooks));
                });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task GoToBookDetails(Book book)
        {
            if (book == null) return;

            var navigationParameter = new Dictionary<string, object>
            {
                { "Book", book }
            };

            await Shell.Current.GoToAsync("BookDetailPage", navigationParameter);
        }

        private async Task ApplyFilter(string status)
        {
            try
            {
                IEnumerable<Book> filtered;

                if (status == "Read")
                    filtered = await _database.GetReadBooksAsync();
                else if (status == "Unread")
                    filtered = await _database.GetUnreadBooksAsync();
                else
                    filtered = await _database.GetBooksAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Books.Clear();
                    foreach (var book in filtered)
                    {
                        Books.Add(book);
                    }
                    OnPropertyChanged(nameof(TotalBooks));
                    OnPropertyChanged(nameof(ReadBooks));
                });
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error en Filtro", ex.Message, "OK");
            }
        }
    }
}