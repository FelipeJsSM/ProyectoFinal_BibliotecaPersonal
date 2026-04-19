using System.Collections.Generic;
using Microsoft.Maui.Controls;
using ProyectoFinal_BibliotecaPersonal.Drawables;

namespace ProyectoFinal_BibliotecaPersonal.Views;

public partial class StatisticsPage : ContentPage
{
	public StatisticsPage()
	{
		InitializeComponent();
        CargarEstadisticas();
	}

    private void CargarEstadisticas()
    {
        var drawable = (StatisticsDrawable)graphicsView.Drawable;

        drawable.TotalBooks = 50;
        drawable.ReadBooks = 35;
        drawable.UnreadBooks = 15;

        drawable.BooksByGenre = new Dictionary<string, int>
        {
            { "Fantasía", 10 },
            { "C.Ficción", 8 },
            { "Terror", 5 },
            { "Romance", 12 },
            { "Historia", 15 }
        };

        graphicsView.Invalidate();
    }
}