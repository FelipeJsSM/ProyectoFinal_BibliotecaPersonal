using Microsoft.Extensions.Logging;
using ProyectoFinal_BibliotecaPersonal.Services;
using ProyectoFinal_BibliotecaPersonal.Views;
using ProyectoFinal_BibliotecaPersonal.ViewModels;

namespace ProyectoFinal_BibliotecaPersonal
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<DatabaseService>();

            builder.Services.AddHttpClient<BookApiService>(client => {
                client.BaseAddress = new Uri("https://www.googleapis.com/books/v1/");
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            builder.Services.AddTransient<LibraryViewModel>();
            builder.Services.AddTransient<BookDetailViewModel>();
            builder.Services.AddTransient<SearchViewModel>();
            builder.Services.AddTransient<StatisticsViewModel>();

            builder.Services.AddTransient<LibraryPage>();
            builder.Services.AddTransient<BookDetailPage>();
            builder.Services.AddTransient<SearchPage>();
            builder.Services.AddTransient<StatisticsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}