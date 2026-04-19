using System.Collections.ObjectModel;
using ProyectoFinal_BibliotecaPersonal.Services;

namespace ProyectoFinal_BibliotecaPersonal.ViewModels
{
    public class StatisticsViewModel : BindableObject
    {
        private readonly DatabaseService _database;

        public int TotalLibros { get; set; } = 0;
        public int Leidos { get; set; } = 0;
        public int Pendientes { get; set; } = 0;
        public Dictionary<string, int> LibrosPorGenero { get; set; } = new Dictionary<string, int>();

        public StatisticsViewModel(DatabaseService database)
        {
            _database = database;
        }

        public async Task LoadStatisticsAsync()
        {
            try
            {
                var todos = await _database.GetBooksAsync();

                if (todos == null || !todos.Any())
                {
                    return;
                }

                TotalLibros = todos.Count;
                Leidos = todos.Count(b => b.IsRead);
                Pendientes = TotalLibros - Leidos;

                LibrosPorGenero = todos
                    .Where(b => !string.IsNullOrWhiteSpace(b.Genre))
                    .GroupBy(b => b.Genre)
                    .ToDictionary(g => g.Key, g => g.Count());

                OnPropertyChanged(nameof(TotalLibros));
            }
            catch (Exception)
            {
            }
        }
    }
}