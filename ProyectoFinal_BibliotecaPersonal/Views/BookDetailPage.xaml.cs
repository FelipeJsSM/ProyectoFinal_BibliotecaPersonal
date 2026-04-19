using ProyectoFinal_BibliotecaPersonal.ViewModels;

namespace ProyectoFinal_BibliotecaPersonal.Views;

public partial class BookDetailPage : ContentPage
{
	public BookDetailPage(BookDetailViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}