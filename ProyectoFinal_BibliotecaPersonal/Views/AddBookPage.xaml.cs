using ProyectoFinal_BibliotecaPersonal.ViewModels;

namespace ProyectoFinal_BibliotecaPersonal.Views;

public partial class AddBookPage : ContentPage
{
	public AddBookPage(AddBookViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}