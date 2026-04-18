using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;   
using ProyectoFinal_BibliotecaPersonal.Services;  
using ProyectoFinal_BibliotecaPersonal.Views;     

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

            builder.Services.AddHttpClient<BookApiService>(client => {
                client.BaseAddress = new Uri("https://www.googleapis.com/books/v1/");
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
