using ProyectoFinal_BibliotecaPersonal.Services;

public class StatisticsViewModel : BindableObject
{
    private readonly DatabaseService _database;

    public int TotalLibros { get; set; }
    public int Leidos { get; set; }
    public int Pendientes { get; set; }
    public Dictionary<string, int> LibrosPorGenero { get; set; } = new Dictionary<string, int>();

    public StatisticsViewModel(DatabaseService database)
    {
        _database = database;
    }

    public async Task LoadStatisticsAsync()
    {
        var todos = await _database.GetBooksAsync();

        TotalLibros = todos.Count;
        Leidos = todos.Count(b => b.IsRead);
        Pendientes = TotalLibros - Leidos;

        LibrosPorGenero = todos
            .Where(b => !string.IsNullOrWhiteSpace(b.Genre))
            .GroupBy(b => b.Genre)
            .ToDictionary(g => g.Key, g => g.Count());

        OnPropertyChanged(nameof(TotalLibros));
    }
}