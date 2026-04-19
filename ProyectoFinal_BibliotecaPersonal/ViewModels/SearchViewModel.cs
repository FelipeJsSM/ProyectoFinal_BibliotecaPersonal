using System.Collections.ObjectModel;
using System.Windows.Input;
using ProyectoFinal_BibliotecaPersonal.Models;
using ProyectoFinal_BibliotecaPersonal.Services;

namespace ProyectoFinal_BibliotecaPersonal.ViewModels
{
    public class SearchViewModel : BindableObject
    {
        private readonly BookApiService _apiService;
        private readonly DatabaseService _databaseService;

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set { _searchQuery = value; OnPropertyChanged(); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Book> SearchResults { get; set; } = new ObservableCollection<Book>();

        public ICommand SearchCommand { get; }
        public ICommand AddBookCommand { get; }

        public SearchViewModel(BookApiService apiService, DatabaseService databaseService)
        {
            _apiService = apiService;
            _databaseService = databaseService;

            SearchCommand = new Command(async () => await PerformSearch());
            AddBookCommand = new Command<Book>(async (book) => await AddBookToLibrary(book));
        }

        private async Task PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery)) return;

            IsBusy = true;
            SearchResults.Clear();

            try
            {
                var results = await _apiService.SearchBooksAsync(SearchQuery);

                if (results != null)
                {
                    foreach (var book in results)
                    {
                        SearchResults.Add(book);
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo conectar con Google Books.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AddBookToLibrary(Book book)
        {
            try
            {
                await _databaseService.SaveBookAsync(book);
                await Application.Current.MainPage.DisplayAlert("Éxito", $"'{book.Title}' se guardó en tu biblioteca.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo guardar el libro.", "OK");
            }
        }
    }
}