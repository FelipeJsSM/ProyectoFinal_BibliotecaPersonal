using ProyectoFinal_BibliotecaPersonal.ViewModels;
using ProyectoFinal_BibliotecaPersonal.Drawables;

namespace ProyectoFinal_BibliotecaPersonal.Views;

public partial class StatisticsPage : ContentPage
{
    private readonly StatisticsViewModel _viewModel;

    public StatisticsPage(StatisticsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await _viewModel.LoadStatisticsAsync();
            var drawable = (StatisticsDrawable)graphicsView.Drawable;
            drawable.TotalBooks = _viewModel.TotalLibros;
            drawable.ReadBooks = _viewModel.Leidos;
            drawable.UnreadBooks = _viewModel.Pendientes;
            drawable.BooksByGenre = _viewModel.LibrosPorGenero;
            graphicsView.Invalidate();
        }
        catch (Exception ex)
        {
            // Esto atrapará si el GraphicsView o la base de datos están fallando
            await DisplayAlert("Error Oculto en Estadísticas", ex.Message, "OK");
        }
    }
}